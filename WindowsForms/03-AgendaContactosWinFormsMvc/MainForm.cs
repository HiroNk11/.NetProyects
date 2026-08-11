using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgendaContactosWinFormsMvc.Controllers;
using AgendaContactosWinFormsMvc.Models;
using AgendaContactosWinFormsMvc.Repositories;
using AgendaContactosWinFormsMvc.Services;

namespace AgendaContactosWinFormsMvc
{
    internal class MainForm : Form
    {
        private readonly ContactoController controller;
        private readonly DataGridView grid = new DataGridView();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtTelefono = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtBuscar = new TextBox();
        private int contactoSeleccionadoId;

        public MainForm()
        {
            controller = new ContactoController(new ContactoRepository(), new ContactoValidator());
            ConfigurarFormulario();
            RefrescarGrilla(controller.ObtenerTodos());
        }

        private void ConfigurarFormulario()
        {
            Text = "Agenda de contactos - MVC";
            Width = 820;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;

            Label lblNombre = CrearEtiqueta("Nombre", 20, 20);
            Label lblTelefono = CrearEtiqueta("Telefono", 20, 70);
            Label lblEmail = CrearEtiqueta("Email", 20, 120);
            Label lblBuscar = CrearEtiqueta("Buscar", 20, 210);

            ConfigurarTextBox(txtNombre, 120, 18);
            ConfigurarTextBox(txtTelefono, 120, 68);
            ConfigurarTextBox(txtEmail, 120, 118);
            ConfigurarTextBox(txtBuscar, 120, 208);
            txtBuscar.TextChanged += (sender, args) => RefrescarGrilla(controller.Buscar(txtBuscar.Text));

            Button btnGuardar = CrearBoton("Guardar", 20, 165);
            Button btnNuevo = CrearBoton("Nuevo", 125, 165);
            Button btnEliminar = CrearBoton("Eliminar", 230, 165);

            btnGuardar.Click += GuardarContacto;
            btnNuevo.Click += (sender, args) => LimpiarFormulario();
            btnEliminar.Click += EliminarContacto;

            grid.Left = 350;
            grid.Top = 20;
            grid.Width = 430;
            grid.Height = 420;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.MultiSelect = false;
            grid.SelectionChanged += SeleccionarContacto;

            Controls.AddRange(new Control[] { lblNombre, lblTelefono, lblEmail, lblBuscar, txtNombre, txtTelefono, txtEmail, txtBuscar, btnGuardar, btnNuevo, btnEliminar, grid });
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

        private void GuardarContacto(object sender, EventArgs e)
        {
            Contacto contacto = new Contacto
            {
                Id = contactoSeleccionadoId,
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            string error = controller.Guardar(contacto);

            if (!string.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error, "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LimpiarFormulario();
            RefrescarGrilla(controller.ObtenerTodos());
        }

        private void EliminarContacto(object sender, EventArgs e)
        {
            if (contactoSeleccionadoId == 0)
            {
                MessageBox.Show("Seleccione un contacto.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            controller.Eliminar(contactoSeleccionadoId);
            LimpiarFormulario();
            RefrescarGrilla(controller.ObtenerTodos());
        }

        private void SeleccionarContacto(object sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is Contacto contacto)
            {
                contactoSeleccionadoId = contacto.Id;
                txtNombre.Text = contacto.Nombre;
                txtTelefono.Text = contacto.Telefono;
                txtEmail.Text = contacto.Email;
            }
        }

        private void RefrescarGrilla(List<Contacto> contactos)
        {
            grid.DataSource = null;
            grid.DataSource = contactos;
        }

        private void LimpiarFormulario()
        {
            contactoSeleccionadoId = 0;
            txtNombre.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            grid.ClearSelection();
        }
    }
}
