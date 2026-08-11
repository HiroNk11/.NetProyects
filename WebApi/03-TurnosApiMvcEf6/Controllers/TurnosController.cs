using System.Web.Http;
using TurnosApiMvcEf6.Data;
using TurnosApiMvcEf6.Dtos;
using TurnosApiMvcEf6.Models;
using TurnosApiMvcEf6.Repositories;
using TurnosApiMvcEf6.Services;

namespace TurnosApiMvcEf6.Controllers
{
    [RoutePrefix("api/turnos")]
    public class TurnosController : ApiController
    {
        private readonly AppDbContext context;
        private readonly TurnoService service;

        public TurnosController()
        {
            context = new AppDbContext();
            service = new TurnoService(
                new EfPacienteRepository(context),
                new EfProfesionalRepository(context),
                new EfTurnoRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Turno turno = service.ObtenerPorId(id);

            if (turno == null)
            {
                return NotFound();
            }

            return Ok(turno);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(TurnoCreateDto dto)
        {
            string error = service.Crear(dto, out Turno turno);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/turnos/{turno.Id}", turno);
        }

        [HttpPost]
        [Route("{id:int}/cancelar")]
        public IHttpActionResult Cancelar(int id)
        {
            if (!service.Cancelar(id))
            {
                return NotFound();
            }

            return Ok();
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
