namespace GeneradorReportes
{
    internal interface IReporteDestino
    {
        void Guardar(string contenido);
    }
}
