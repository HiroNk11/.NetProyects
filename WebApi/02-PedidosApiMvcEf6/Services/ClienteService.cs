using System.Collections.Generic;
using PedidosApiMvcEf6.Dtos;
using PedidosApiMvcEf6.Models;
using PedidosApiMvcEf6.Repositories;

namespace PedidosApiMvcEf6.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository repository;

        public ClienteService(IClienteRepository repository)
        {
            this.repository = repository;
        }

        public List<Cliente> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public string Crear(ClienteCreateDto dto, out Cliente cliente)
        {
            cliente = null;

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
            {
                return "Ingrese un email valido.";
            }

            cliente = new Cliente
            {
                Nombre = dto.Nombre.Trim(),
                Email = dto.Email.Trim()
            };

            repository.Agregar(cliente);
            return string.Empty;
        }
    }
}
