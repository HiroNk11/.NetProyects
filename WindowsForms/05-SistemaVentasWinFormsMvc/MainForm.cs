using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SistemaVentasWinFormsMvc.Controllers;
using SistemaVentasWinFormsMvc.Models;
using SistemaVentasWinFormsMvc.Repositories;
using SistemaVentasWinFormsMvc.Services;

namespace SistemaVentasWinFormsMvc
{
    internal class MainForm : Form
    {
        private readonly VentaController controller;
        private readonly ComboBox cmbClientes = new ComboBox();
        private readonly ComboBox cmbProductos = new ComboBox();
        private readonly NumericUpDown numCantidad = new NumericUpDown();
        private readonly DataGridView gridItems = new DataGridView();
        private readonly DataGridView gridVentas = new DataGridView();
        private readonly Label lblTotalVenta = new Label();
        private readonly Label lblTotalVendido = new Label();
        private readonly List<ItemVenta> itemsActuales = new List<ItemVenta>();

        public MainForm()
        {
            MemoryStore store = new MemoryStore();
            controller = new VentaController(new VentaService(
                new ClienteRepository(store),
                new ProductoRepository(store),
                new VentaRepository(store)));

            ConfigurarFormulario();
            CargarDatosDemo();
            RefrescarTodo();
        }

        private void ConfigurarFormulario()
        {
            Text = "Sistema de ventas - MVC";
            Width = 1020;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;

            Button btnCliente = CrearBoton("Nuevo cliente", 20, 20, 130);
            Button btnProducto = CrearBoton("Nuevo producto", 160, 20, 140);
            Button btnAgregarItem = CrearBoton("Agregar item", 20, 135, 130);
            Button btnRegistrarVenta = CrearBoton("Registrar venta", 160, 135, 140);

            btnCliente.Click += AgregarCliente;
            btnProducto.Click += AgregarProducto;
            btnAgregarItem.Click += AgregarItem;
            btnRegistrarVenta.Click += RegistrarVenta;

            Controls.Add(CrearEtiqueta("Cliente", 20, 70));
            cmbClientes.Left = 90;
            cmbClientes.Top = 68;
            cmbClientes.Width = 210;
            cmbClientes.DropDownStyle = ComboBoxStyle.DropDownList;

            Controls.Add(CrearEtiqueta("Producto", 20, 105));
            cmbProductos.Left = 90;
            cmbProductos.Top = 103;
            cmbProductos.Width = 210;
            cmbProductos.DropDownStyle = ComboBoxStyle.DropDownList;

            Controls.Add(CrearEtiqueta("Cantidad", 320, 105));
            numCantidad.Left = 390;
            numCantidad.Top = 103;
            numCantidad.Width = 70;
            numCantidad.Minimum = 1;
            numCantidad.Maximum = 999;

            gridItems.Left = 20;
            gridItems.Top = 190;
            gridItems.Width = 450;
            gridItems.Height = 300;
            gridItems.ReadOnly = true;
            gridItems.AllowUserToAddRows = false;
            gridItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridVentas.Left = 500;
            gridVentas.Top = 20;
            gridVentas.Width = 480;
            gridVentas.Height = 470;
            gridVentas.ReadOnly = true;
            gridVentas.AllowUserToAddRows = false;
            gridVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblTotalVenta.Left = 20;
            lblTotalVenta.Top = 505;
            lblTotalVenta.Width = 300;

            lblTotalVendido.Left = 500;
            lblTotalVendido.Top = 505;
            lblTotalVendido.Width = 300;

            Controls.AddRange(new Control[] { btnCliente, btnProducto, btnAgregarItem, btnRegistrarVenta, cmbClientes, cmbProductos, numCantidad, gridItems, gridVentas, lblTotalVenta, lblTotalVendido });
        }

        private void CargarDatosDemo()
        {
            controller.AgregarCliente("Consumidor Final", "cliente@demo.com");
            controller.AgregarProducto("Mouse", 8500, 10);
            controller.AgregarProducto("Teclado", 15000, 8);
            controller.AgregarProducto("Monitor", 120000, 4);
        }

        private void AgregarCliente(object sender, EventArgs e)
        {
            string nombre = Prompt.Mostrar("Nombre del cliente", "Nuevo cliente");
            string email = Prompt.Mostrar("Email del cliente", "Nuevo cliente");
            string error = controller.AgregarCliente(nombre, email);

            MostrarErrorSiExiste(error);
            RefrescarTodo();
        }

        private void AgregarProducto(object sender, EventArgs e)
        {
            string nombre = Prompt.Mostrar("Nombre del producto", "Nuevo producto");
            string precioTexto = Prompt.Mostrar("Precio", "Nuevo producto");
            string stockTexto = Prompt.Mostrar("Stock", "Nuevo producto");

            if (!decimal.TryParse(precioTexto, out decimal precio) || !int.TryParse(stockTexto, out int stock))
            {
                MessageBox.Show("Ingrese precio y stock validos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string error = controller.AgregarProducto(nombre, precio, stock);
            MostrarErrorSiExiste(error);
            RefrescarTodo();
        }

        private void AgregarItem(object sender, EventArgs e)
        {
            if (!(cmbProductos.SelectedItem is Producto producto))
            {
                MessageBox.Show("Seleccione un producto.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cantidad = (int)numCantidad.Value;

            if (cantidad > producto.Stock)
            {
                MessageBox.Show("La cantidad supera el stock disponible.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            itemsActuales.Add(new ItemVenta { Producto = producto, Cantidad = cantidad, PrecioUnitario = producto.Precio });
            RefrescarItems();
        }

        private void RegistrarVenta(object sender, EventArgs e)
        {
            string error = controller.RegistrarVenta(cmbClientes.SelectedItem as Cliente, itemsActuales.ToList());

            if (MostrarErrorSiExiste(error))
            {
                return;
            }

            itemsActuales.Clear();
            RefrescarTodo();
        }

        private bool MostrarErrorSiExiste(string error)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                return false;
            }

            MessageBox.Show(error, "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        private void RefrescarTodo()
        {
            cmbClientes.DataSource = null;
            cmbClientes.DataSource = controller.ObtenerClientes();

            cmbProductos.DataSource = null;
            cmbProductos.DataSource = controller.ObtenerProductos();

            gridVentas.DataSource = null;
            gridVentas.DataSource = controller.ObtenerVentas()
                .Select(venta => new { venta.Id, venta.Fecha, Cliente = venta.ClienteNombre, venta.Total })
                .ToList();

            RefrescarItems();
            lblTotalVendido.Text = $"Total vendido: ${controller.CalcularTotalVendido():0.00}";
        }

        private void RefrescarItems()
        {
            gridItems.DataSource = null;
            gridItems.DataSource = itemsActuales
                .Select(item => new { Producto = item.ProductoNombre, item.Cantidad, item.PrecioUnitario, item.Subtotal })
                .ToList();

            lblTotalVenta.Text = $"Total venta actual: ${itemsActuales.Sum(item => item.Subtotal):0.00}";
        }

        private Label CrearEtiqueta(string texto, int left, int top)
        {
            return new Label { Text = texto, Left = left, Top = top, Width = 70 };
        }

        private Button CrearBoton(string texto, int left, int top, int width)
        {
            return new Button { Text = texto, Left = left, Top = top, Width = width, Height = 32 };
        }
    }
}
