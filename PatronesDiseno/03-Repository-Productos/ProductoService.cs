using System.Collections.Generic;

namespace RepositoryProductos
{
    internal class ProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public List<Producto> Listar()
        {
            return _repository.ObtenerTodos();
        }

        public bool Vender(int productoId, int cantidad)
        {
            Producto producto = _repository.ObtenerPorId(productoId);

            if (producto == null || cantidad <= 0 || producto.Stock < cantidad)
            {
                return false;
            }

            producto.DescontarStock(cantidad);
            _repository.Actualizar(producto);
            return true;
        }
    }
}
