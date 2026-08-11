using System.Web.Http;
using TurnosApiMvcEf6.Data;
using TurnosApiMvcEf6.Dtos;
using TurnosApiMvcEf6.Models;
using TurnosApiMvcEf6.Repositories;
using TurnosApiMvcEf6.Services;

namespace TurnosApiMvcEf6.Controllers
{
    [RoutePrefix("api/profesionales")]
    public class ProfesionalesController : ApiController
    {
        private readonly AppDbContext context;
        private readonly ProfesionalService service;

        public ProfesionalesController()
        {
            context = new AppDbContext();
            service = new ProfesionalService(new EfProfesionalRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ProfesionalCreateDto dto)
        {
            string error = service.Crear(dto, out Profesional profesional);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/profesionales/{profesional.Id}", profesional);
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
