using System;
using PruebasUnitariasBasicas.Domain;
using PruebasUnitariasBasicas.Testing;

namespace PruebasUnitariasBasicas.Tests
{
    internal class CalculadoraTests
    {
        private readonly CalculadoraService _service = new CalculadoraService();

        public void Sumar_DeberiaRetornarTotal()
        {
            decimal result = _service.Sumar(10, 15);
            Assert.AreEqual(25m, result, nameof(Sumar_DeberiaRetornarTotal));
        }

        public void Dividir_DeberiaRetornarCociente()
        {
            decimal result = _service.Dividir(20, 4);
            Assert.AreEqual(5m, result, nameof(Dividir_DeberiaRetornarCociente));
        }

        public void Dividir_CuandoDivisorEsCero_DeberiaLanzarExcepcion()
        {
            Assert.Throws<DivideByZeroException>(
                () => _service.Dividir(20, 0),
                nameof(Dividir_CuandoDivisorEsCero_DeberiaLanzarExcepcion));
        }

        public void CalcularPorcentaje_DeberiaRetornarImporte()
        {
            decimal result = _service.CalcularPorcentaje(1000, 15);
            Assert.AreEqual(150m, result, nameof(CalcularPorcentaje_DeberiaRetornarImporte));
        }
    }
}
