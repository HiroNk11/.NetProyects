using System;

namespace CalculadoraVisual
{
    internal class Calculadora
    {
        public double Sumar(double primerNumero, double segundoNumero)
        {
            return primerNumero + segundoNumero;
        }

        public double Restar(double primerNumero, double segundoNumero)
        {
            return primerNumero - segundoNumero;
        }

        public double Multiplicar(double primerNumero, double segundoNumero)
        {
            return primerNumero * segundoNumero;
        }

        public double Dividir(double primerNumero, double segundoNumero)
        {
            if (segundoNumero == 0)
            {
                throw new DivideByZeroException("No se puede dividir por cero.");
            }

            return primerNumero / segundoNumero;
        }
    }
}
