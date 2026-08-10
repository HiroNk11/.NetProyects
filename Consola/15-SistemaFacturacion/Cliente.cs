namespace SistemaFacturacion
{
    internal class Cliente
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }

        public string ObtenerResumen()
        {
            return $"{Id}. {RazonSocial} | CUIT: {Cuit}";
        }
    }
}
