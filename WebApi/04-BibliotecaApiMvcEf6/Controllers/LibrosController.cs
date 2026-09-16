using System.Web.Http;
using BibliotecaApiMvcEf6.Data;
using BibliotecaApiMvcEf6.Dtos;
using BibliotecaApiMvcEf6.Models;
using BibliotecaApiMvcEf6.Repositories;
using BibliotecaApiMvcEf6.Services;

namespace BibliotecaApiMvcEf6.Controllers
{
    [RoutePrefix("api/libros")]
    public class LibrosController : ApiController
    {
        private readonly AppDbContext context;
        private readonly LibroService service;

        public LibrosController()
        {
            context = new AppDbContext();
            service = new LibroService(new EfLibroRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(LibroCreateDto dto)
        {
            string error = service.Crear(dto, out Libro libro);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/libros/{libro.Id}", libro);
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
