# 02 - Factory - Metodos de pago

Aplicacion de consola en C# que usa el patron Factory para crear distintos metodos de pago.

## Funcionalidades

- Ingresar un importe
- Elegir metodo de pago
- Pagar en efectivo
- Pagar con tarjeta
- Pagar por transferencia
- Aplicar recargo o descuento segun el metodo

## Conceptos practicados

- Patron Factory
- Interfaces
- Polimorfismo
- Encapsulamiento de creacion de objetos
- Separacion entre creacion y uso
- Validaciones con `decimal.TryParse`

## Consigna

Crear una aplicacion donde el usuario elija un metodo de pago y una factory se encargue de devolver el objeto correcto.

## Mejora opcional

Agregar un metodo de pago con billetera virtual sin modificar `ProcesadorPago`.
