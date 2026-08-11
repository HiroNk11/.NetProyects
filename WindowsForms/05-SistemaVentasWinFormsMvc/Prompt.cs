using System.Windows.Forms;

namespace SistemaVentasWinFormsMvc
{
    internal static class Prompt
    {
        public static string Mostrar(string mensaje, string titulo)
        {
            Form form = new Form { Width = 420, Height = 160, Text = titulo, StartPosition = FormStartPosition.CenterParent };
            Label label = new Label { Left = 20, Top = 20, Width = 360, Text = mensaje };
            TextBox textBox = new TextBox { Left = 20, Top = 50, Width = 360 };
            Button button = new Button { Text = "Aceptar", Left = 285, Width = 95, Top = 82, DialogResult = DialogResult.OK };

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(button);
            form.AcceptButton = button;

            return form.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }
}
