using System.Web.Http;
using PedidosApiMvcEf6.Data;
using PedidosApiMvcEf6.Dtos;
using PedidosApiMvcEf6.Models;
using PedidosApiMvcEf6.Repositories;
using PedidosApiMvcEf6.Services;

namespace PedidosApiMvcEf6.Controllers
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
