using System.Web.Http;
using PedidosApiMvcEf6.Data;
using PedidosApiMvcEf6.Dtos;
using PedidosApiMvcEf6.Models;
using PedidosApiMvcEf6.Repositories;
using PedidosApiMvcEf6.Services;

namespace PedidosApiMvcEf6.Controllers
{
    [RoutePrefix("api/clientes")]
    public class ClientesController : ApiController
    {
        private readonly AppDbContext context;
        private readonly ClienteService service;

        public ClientesController()
        {
            context = new AppDbContext();
            service = new ClienteService(new EfClienteRepository(context));
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.ObtenerTodos());
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ClienteCreateDto dto)
        {
            string error = service.Crear(dto, out Cliente cliente);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

            return Created($"api/clientes/{cliente.Id}", cliente);
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
