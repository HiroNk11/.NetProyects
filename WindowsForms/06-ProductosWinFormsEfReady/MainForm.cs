using System;
using System.Windows.Forms;
using ProductosWinFormsEfReady.Models;
using ProductosWinFormsEfReady.Repositories;
using ProductosWinFormsEfReady.Services;

namespace ProductosWinFormsEfReady
{
    internal class MainForm : Form
    {
        private readonly ProductoService service;
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtPrecio = new TextBox();
        private readonly CheckBox chkActivo = new CheckBox();
        private readonly DataGridView grid = new DataGridView();
        private int productoSeleccionadoId;

        public MainForm()
        {
            service = new ProductoService(new InMemoryProductoRepository());
            ConfigurarFormulario();
            RefrescarGrilla();
        }

        private void ConfigurarFormulario()
        {
            Text = "Productos - EF Ready";
            Width = 760;
            Height = 460;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Label { Text = "Nombre", Left = 20, Top = 25, Width = 80 });
            Controls.Add(new Label { Text = "Precio", Left = 20, Top = 70, Width = 80 });

            txtNombre.Left = 110;
            txtNombre.Top = 22;
            txtNombre.Width = 200;

            txtPrecio.Left = 110;
            txtPrecio.Top = 67;
            txtPrecio.Width = 200;

            chkActivo.Text = "Activo";
            chkActivo.Left = 110;
            chkActivo.Top = 110;
            chkActivo.Checked = true;

            Button btnGuardar = new Button { Text = "Guardar", Left = 20, Top = 150, Width = 90 };
            Button btnNuevo = new Button { Text = "Nuevo", Left = 120, Top = 150, Width = 90 };
            Button btnEliminar = new Button { Text = "Eliminar", Left = 220, Top = 150, Width = 90 };

            btnGuardar.Click += GuardarProducto;
            btnNuevo.Click += (sender, args) => Limpiar();
            btnEliminar.Click += EliminarProducto;

            grid.Left = 340;
            grid.Top = 20;
            grid.Width = 380;
            grid.Height = 340;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.SelectionChanged += SeleccionarProducto;

            Controls.AddRange(new Control[] { txtNombre, txtPrecio, chkActivo, btnGuardar, btnNuevo, btnEliminar, grid });
        }

        private void GuardarProducto(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Ingrese un precio valido.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Producto producto = new Producto
            {
                Id = productoSeleccionadoId,
                Nombre = txtNombre.Text,
                Precio = precio,
                Activo = chkActivo.Checked
            };

            string error = service.Guardar(producto);

            if (!string.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error, "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Limpiar();
            RefrescarGrilla();
        }

        private void EliminarProducto(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == 0)
            {
                MessageBox.Show("Seleccione un producto.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            service.Eliminar(productoSeleccionadoId);
            Limpiar();
            RefrescarGrilla();
        }

        private void SeleccionarProducto(object sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is Producto producto)
            {
                productoSeleccionadoId = producto.Id;
                txtNombre.Text = producto.Nombre;
                txtPrecio.Text = producto.Precio.ToString("0.00");
                chkActivo.Checked = producto.Activo;
            }
        }

        private void RefrescarGrilla()
        {
            grid.DataSource = null;
            grid.DataSource = service.ObtenerTodos();
        }

        private void Limpiar()
        {
            productoSeleccionadoId = 0;
            txtNombre.Clear();
            txtPrecio.Clear();
            chkActivo.Checked = true;
            grid.ClearSelection();
        }
    }
}
