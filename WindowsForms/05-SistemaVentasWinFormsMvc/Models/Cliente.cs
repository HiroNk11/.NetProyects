namespace SistemaVentasWinFormsMvc.Models
{
    internal class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
