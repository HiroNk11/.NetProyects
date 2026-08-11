using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GestorProductosWinFormsSolid.Models;
using GestorProductosWinFormsSolid.Repositories;
using GestorProductosWinFormsSolid.Services;

namespace GestorProductosWinFormsSolid
{
    internal class MainForm : Form
    {
        private readonly ProductoService service;
        private readonly DataGridView grid = new DataGridView();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtCategoria = new TextBox();
        private readonly TextBox txtPrecio = new TextBox();
        private readonly TextBox txtStock = new TextBox();
        private readonly TextBox txtBuscar = new TextBox();
        private readonly Label lblTotal = new Label();
        private int productoSeleccionadoId;

        public MainForm()
        {
            service = new ProductoService(new ProductoRepository());
            ConfigurarFormulario();
            RefrescarGrilla(service.ObtenerTodos());
        }

        private void ConfigurarFormulario()
        {
            Text = "Gestor de productos - SOLID";
            Width = 900;
            Height = 540;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(CrearEtiqueta("Nombre", 20, 20));
            Controls.Add(CrearEtiqueta("Categoria", 20, 65));
            Controls.Add(CrearEtiqueta("Precio", 20, 110));
            Controls.Add(CrearEtiqueta("Stock", 20, 155));
            Controls.Add(CrearEtiqueta("Buscar", 20, 255));

            ConfigurarTextBox(txtNombre, 120, 18);
            ConfigurarTextBox(txtCategoria, 120, 63);
            ConfigurarTextBox(txtPrecio, 120, 108);
            ConfigurarTextBox(txtStock, 120, 153);
            ConfigurarTextBox(txtBuscar, 120, 253);
            txtBuscar.TextChanged += (sender, args) => RefrescarGrilla(service.Buscar(txtBuscar.Text));

            Button btnGuardar = CrearBoton("Guardar", 20, 205);
            Button btnNuevo = CrearBoton("Nuevo", 125, 205);
            Button btnEliminar = CrearBoton("Eliminar", 230, 205);

            btnGuardar.Click += GuardarProducto;
            btnNuevo.Click += (sender, args) => LimpiarFormulario();
            btnEliminar.Click += EliminarProducto;

            grid.Left = 350;
            grid.Top = 20;
            grid.Width = 510;
            grid.Height = 390;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.MultiSelect = false;
            grid.SelectionChanged += SeleccionarProducto;

            lblTotal.Left = 350;
            lblTotal.Top = 425;
            lblTotal.Width = 400;
            lblTotal.Height = 30;

            Controls.AddRange(new Control[] { txtNombre, txtCategoria, txtPrecio, txtStock, txtBuscar, btnGuardar, btnNuevo, btnEliminar, grid, lblTotal });
        }

        private Label CrearEtiqueta(string texto, int left, int top)
        {
            return new Label { Text = texto, Left = left, Top = top, Width = 90 };
        }

        private void ConfigurarTextBox(TextBox textBox, int left, int top)
        {
            textBox.Left = left;
            textBox.Top = top;
            textBox.Width = 200;
        }

        private Button CrearBoton(string texto, int left, int top)
        {
            return new Button { Text = texto, Left = left, Top = top, Width = 95, Height = 32 };
        }

        private void GuardarProducto(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || !int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Ingrese precio y stock validos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Producto producto = new Producto
            {
                Id = productoSeleccionadoId,
                Nombre = txtNombre.Text,
                Categoria = txtCategoria.Text,
                Precio = precio,
                Stock = stock
            };

            string error = service.Guardar(producto);

            if (!string.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error, "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LimpiarFormulario();
            RefrescarGrilla(service.ObtenerTodos());
        }

        private void EliminarProducto(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == 0)
            {
                MessageBox.Show("Seleccione un producto.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            service.Eliminar(productoSeleccionadoId);
            LimpiarFormulario();
            RefrescarGrilla(service.ObtenerTodos());
        }

        private void SeleccionarProducto(object sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is Producto producto)
            {
                productoSeleccionadoId = producto.Id;
                txtNombre.Text = producto.Nombre;
                txtCategoria.Text = producto.Categoria;
                txtPrecio.Text = producto.Precio.ToString("0.00");
                txtStock.Text = producto.Stock.ToString();
            }
        }

        private void RefrescarGrilla(List<Producto> productos)
        {
            grid.DataSource = null;
            grid.DataSource = productos;
            lblTotal.Text = $"Valor total de stock: ${service.CalcularValorTotal():0.00}";
        }

        private void LimpiarFormulario()
        {
            productoSeleccionadoId = 0;
            txtNombre.Clear();
            txtCategoria.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            grid.ClearSelection();
        }
    }
}
