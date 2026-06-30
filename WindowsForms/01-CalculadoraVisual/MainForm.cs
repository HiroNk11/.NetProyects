using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculadoraVisual
{
    internal class MainForm : Form
    {
        private readonly Calculadora _calculadora = new Calculadora();
        private readonly TextBox _txtPrimerNumero = new TextBox();
        private readonly TextBox _txtSegundoNumero = new TextBox();
        private readonly Label _lblResultado = new Label();

        public MainForm()
        {
            Text = "Calculadora visual";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(420, 260);
            MinimumSize = new Size(420, 260);

            CrearControles();
        }

        private void CrearControles()
        {
            Label lblTitulo = new Label
            {
                Text = "Calculadora visual",
                Font = new Font(Font.FontFamily, 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            Label lblPrimerNumero = CrearEtiqueta("Primer numero:", 24, 70);
            Label lblSegundoNumero = CrearEtiqueta("Segundo numero:", 24, 110);

            _txtPrimerNumero.Location = new Point(150, 66);
            _txtPrimerNumero.Width = 220;

            _txtSegundoNumero.Location = new Point(150, 106);
            _txtSegundoNumero.Width = 220;

            Button btnSumar = CrearBoton("+", 24, 150, Sumar);
            Button btnRestar = CrearBoton("-", 104, 150, Restar);
            Button btnMultiplicar = CrearBoton("*", 184, 150, Multiplicar);
            Button btnDividir = CrearBoton("/", 264, 150, Dividir);

            _lblResultado.Text = "Resultado: -";
            _lblResultado.AutoSize = true;
            _lblResultado.Font = new Font(Font.FontFamily, 11, FontStyle.Bold);
            _lblResultado.Location = new Point(24, 205);

            Controls.Add(lblTitulo);
            Controls.Add(lblPrimerNumero);
            Controls.Add(lblSegundoNumero);
            Controls.Add(_txtPrimerNumero);
            Controls.Add(_txtSegundoNumero);
            Controls.Add(btnSumar);
            Controls.Add(btnRestar);
            Controls.Add(btnMultiplicar);
            Controls.Add(btnDividir);
            Controls.Add(_lblResultado);
        }

        private static Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                Location = new Point(x, y + 4)
            };
        }

        private static Button CrearBoton(string texto, int x, int y, EventHandler eventoClick)
        {
            Button boton = new Button
            {
                Text = texto,
                Width = 56,
                Height = 34,
                Location = new Point(x, y)
            };

            boton.Click += eventoClick;
            return boton;
        }

        private void Sumar(object sender, EventArgs e)
        {
            Calcular(_calculadora.Sumar);
        }

        private void Restar(object sender, EventArgs e)
        {
            Calcular(_calculadora.Restar);
        }

        private void Multiplicar(object sender, EventArgs e)
        {
            Calcular(_calculadora.Multiplicar);
        }

        private void Dividir(object sender, EventArgs e)
        {
            Calcular(_calculadora.Dividir);
        }

        private void Calcular(Func<double, double, double> operacion)
        {
            if (!double.TryParse(_txtPrimerNumero.Text, out double primerNumero) ||
                !double.TryParse(_txtSegundoNumero.Text, out double segundoNumero))
            {
                MessageBox.Show("Ingrese dos numeros validos.", "Datos invalidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                double resultado = operacion(primerNumero, segundoNumero);
                _lblResultado.Text = $"Resultado: {resultado:0.##}";
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message, "Operacion invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
