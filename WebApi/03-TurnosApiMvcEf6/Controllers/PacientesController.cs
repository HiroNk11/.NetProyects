using System.Web.Http;
using TurnosApiMvcEf6.Data;
using TurnosApiMvcEf6.Dtos;
using TurnosApiMvcEf6.Models;
using TurnosApiMvcEf6.Repositories;
using TurnosApiMvcEf6.Services;

namespace TurnosApiMvcEf6.Controllers
{
    [RoutePrefix("api/pacientes")]
    public class PacientesController : ApiController
    {
        private readonly AppDbContext context;
        private readonly PacienteService service;

        public PacientesController()
        {
            context = new AppDbContext();
            service = new PacienteService(new EfPacienteRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(PacienteCreateDto dto)
        {
            string error = service.Crear(dto, out Paciente paciente);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/pacientes/{paciente.Id}", paciente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
