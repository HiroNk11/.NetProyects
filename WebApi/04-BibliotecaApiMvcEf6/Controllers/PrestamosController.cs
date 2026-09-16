using System.Web.Http;
using BibliotecaApiMvcEf6.Data;
using BibliotecaApiMvcEf6.Dtos;
using BibliotecaApiMvcEf6.Models;
using BibliotecaApiMvcEf6.Repositories;
using BibliotecaApiMvcEf6.Services;

namespace BibliotecaApiMvcEf6.Controllers
{
    [RoutePrefix("api/prestamos")]
    public class PrestamosController : ApiController
    {
        private readonly AppDbContext context;
        private readonly PrestamoService service;

        public PrestamosController()
        {
            context = new AppDbContext();
            service = new PrestamoService(
                new EfSocioRepository(context),
                new EfLibroRepository(context),
                new EfPrestamoRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(PrestamoCreateDto dto)
        {
            string error = service.Crear(dto, out Prestamo prestamo);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/prestamos/{prestamo.Id}", prestamo);
        }

        [HttpPost]
        [Route("{id:int}/devolver")]
        public IHttpActionResult Devolver(int id)
        {
            if (!service.Devolver(id))
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
