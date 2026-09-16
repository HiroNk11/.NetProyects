using System;

namespace PruebasUnitariasBasicas.Domain
{
    internal class CalculadoraService
    {
        public decimal Sumar(decimal primerNumero, decimal segundoNumero)
        {
            return primerNumero + segundoNumero;
        }

        public decimal Dividir(decimal dividendo, decimal divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException("No se puede dividir por cero.");
            }

            return dividendo / divisor;
        }

        public decimal CalcularPorcentaje(decimal total, decimal porcentaje)
        {
            if (porcentaje < 0)
            {
                throw new ArgumentException("El porcentaje no puede ser negativo.", nameof(porcentaje));
            }

            return total * porcentaje / 100;
        }
    }
}
