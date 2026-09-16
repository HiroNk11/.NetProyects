using System.Web.Http;
using BibliotecaApiMvcEf6.Data;
using BibliotecaApiMvcEf6.Dtos;
using BibliotecaApiMvcEf6.Models;
using BibliotecaApiMvcEf6.Repositories;
using BibliotecaApiMvcEf6.Services;

namespace BibliotecaApiMvcEf6.Controllers
{
    [RoutePrefix("api/socios")]
    public class SociosController : ApiController
    {
        private readonly AppDbContext context;
        private readonly SocioService service;

        public SociosController()
        {
            context = new AppDbContext();
            service = new SocioService(new EfSocioRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(SocioCreateDto dto)
        {
            string error = service.Crear(dto, out Socio socio);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/socios/{socio.Id}", socio);
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
