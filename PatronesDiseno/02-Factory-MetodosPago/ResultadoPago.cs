namespace FactoryMetodosPago
{
    internal class ResultadoPago
    {
        public ResultadoPago(bool aprobado, string mensaje)
        {
            Aprobado = aprobado;
            Mensaje = mensaje;
        }

        public bool Aprobado { get; }
        public string Mensaje { get; }
    }
}
