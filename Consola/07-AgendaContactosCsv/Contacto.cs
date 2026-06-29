namespace AgendaContactosCsv
{
    internal class Contacto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public string ObtenerResumen()
        {
            return $"{Id}. {Nombre} | Tel: {Telefono} | Email: {Email}";
        }
    }
}
