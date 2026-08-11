using System.Web.Http;
using ProductosApiMvcEf6.Data;
using ProductosApiMvcEf6.Dtos;
using ProductosApiMvcEf6.Models;
using ProductosApiMvcEf6.Repositories;
using ProductosApiMvcEf6.Services;

namespace ProductosApiMvcEf6.Controllers
{
    [RoutePrefix("api/productos")]
    public class ProductosController : ApiController
    {
        private readonly AppDbContext context;
        private readonly ProductoService service;

        public ProductosController()
        {
            context = new AppDbContext();
            service = new ProductoService(new EfProductoRepository(context));
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
            Producto producto = service.ObtenerPorId(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ProductoCreateDto dto)
        {
            string error = service.Crear(dto, out Producto producto);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/productos/{producto.Id}", producto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ProductoUpdateDto dto)
        {
            string error = service.Actualizar(id, dto, out Producto producto);

            if (error == "Producto no encontrado.")
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Ok(producto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            if (!service.Eliminar(id))
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
