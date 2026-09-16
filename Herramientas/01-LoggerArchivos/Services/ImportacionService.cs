using LoggerArchivos.Logging;

namespace LoggerArchivos.Services
{
    internal class ImportacionService
    {
        private readonly ILogger _logger;

        public ImportacionService(ILogger logger)
        {
            _logger = logger;
        }

        public void Ejecutar()
        {
            _logger.Log(LogLevel.Info, "Iniciando importacion de clientes.");
            _logger.Log(LogLevel.Warning, "Se encontro un cliente sin telefono.");
            _logger.Log(LogLevel.Info, "Clientes validos procesados: 24.");
            _logger.Log(LogLevel.Error, "No se pudo conectar con el servicio de auditoria externo.");
            _logger.Log(LogLevel.Info, "Importacion finalizada.");
        }
    }
}
