using System.Web.Http;
using PedidosApiMvcEf6.Data;
using PedidosApiMvcEf6.Dtos;
using PedidosApiMvcEf6.Models;
using PedidosApiMvcEf6.Repositories;
using PedidosApiMvcEf6.Services;

namespace PedidosApiMvcEf6.Controllers
{
    [RoutePrefix("api/pedidos")]
    public class PedidosController : ApiController
    {
        private readonly AppDbContext context;
        private readonly PedidoService service;

        public PedidosController()
        {
            context = new AppDbContext();
            service = new PedidoService(
                new EfClienteRepository(context),
                new EfProductoRepository(context),
                new EfPedidoRepository(context));
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
            Pedido pedido = service.ObtenerPorId(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(PedidoCreateDto dto)
        {
            string error = service.Crear(dto, out Pedido pedido);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/pedidos/{pedido.Id}", pedido);
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
