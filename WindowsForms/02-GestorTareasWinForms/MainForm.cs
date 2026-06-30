using System;
using System.Drawing;
using System.Windows.Forms;

namespace GestorTareasWinForms
{
    internal class MainForm : Form
    {
        private readonly GestorTareas _gestor = new GestorTareas();
        private readonly TextBox _txtDescripcion = new TextBox();
        private readonly DataGridView _grid = new DataGridView();

        public MainForm()
        {
            Text = "Gestor de tareas";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(640, 420);
            MinimumSize = new Size(640, 420);

            CrearControles();
            CargarDatosIniciales();
            RefrescarGrilla();
        }

        private void CrearControles()
        {
            Label lblTitulo = new Label
            {
                Text = "Gestor de tareas",
                Font = new Font(Font.FontFamily, 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            Label lblDescripcion = new Label
            {
                Text = "Descripcion:",
                AutoSize = true,
                Location = new Point(24, 76)
            };

            _txtDescripcion.Location = new Point(110, 72);
            _txtDescripcion.Width = 350;

            Button btnAgregar = CrearBoton("Agregar", 480, 70, Agregar);
            Button btnCompletar = CrearBoton("Completar", 24, 340, Completar);
            Button btnEliminar = CrearBoton("Eliminar", 130, 340, Eliminar);

            _grid.Location = new Point(24, 120);
            _grid.Size = new Size(580, 200);
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            Controls.Add(lblTitulo);
            Controls.Add(lblDescripcion);
            Controls.Add(_txtDescripcion);
            Controls.Add(btnAgregar);
            Controls.Add(_grid);
            Controls.Add(btnCompletar);
            Controls.Add(btnEliminar);
        }

        private static Button CrearBoton(string texto, int x, int y, EventHandler eventoClick)
        {
            Button boton = new Button
            {
                Text = texto,
                Width = 90,
                Height = 32,
                Location = new Point(x, y)
            };

            boton.Click += eventoClick;
            return boton;
        }

        private void CargarDatosIniciales()
        {
            _gestor.Agregar("Revisar ejercicios de consola");
            _gestor.Agregar("Practicar eventos de Windows Forms");
            _gestor.Agregar("Subir cambios a GitHub");
        }

        private void Agregar(object sender, EventArgs e)
        {
            string descripcion = _txtDescripcion.Text.Trim();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show("Ingrese una descripcion.", "Datos invalidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _gestor.Agregar(descripcion);
            _txtDescripcion.Clear();
            RefrescarGrilla();
        }

        private void Completar(object sender, EventArgs e)
        {
            int? id = ObtenerIdSeleccionado();

            if (!id.HasValue)
            {
                MessageBox.Show("Seleccione una tarea.", "Seleccion requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _gestor.Completar(id.Value);
            RefrescarGrilla();
        }

        private void Eliminar(object sender, EventArgs e)
        {
            int? id = ObtenerIdSeleccionado();

            if (!id.HasValue)
            {
                MessageBox.Show("Seleccione una tarea.", "Seleccion requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _gestor.Eliminar(id.Value);
            RefrescarGrilla();
        }

        private int? ObtenerIdSeleccionado()
        {
            if (_grid.CurrentRow == null)
            {
                return null;
            }

            return Convert.ToInt32(_grid.CurrentRow.Cells["Id"].Value);
        }

        private void RefrescarGrilla()
        {
            _grid.DataSource = null;
            _grid.DataSource = _gestor.ObtenerTodas();

            if (_grid.Columns["EstaCompletada"] != null)
            {
                _grid.Columns["EstaCompletada"].Visible = false;
            }
        }
    }
}
