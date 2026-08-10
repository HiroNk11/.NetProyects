using System;

namespace ControlGastos
{
    internal class Gasto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }

        public string ObtenerResumen()
        {
            return $"{Id}. {Fecha:dd/MM/yyyy} | {Categoria} | {Descripcion} | ${Importe:0.00}";
        }
    }
}
