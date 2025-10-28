using System;

namespace TarjetaSube
{
    public class BoletoGratuito : Tarjeta
    {
        public BoletoGratuito(int numero) : base(numero)
        {
        }

        public new bool PagarBoleto(decimal tarifa)
        {
            // Totalmente gratuito - siempre puede pagar
            Console.WriteLine($"Viaje gratuito con Boleto Estudiantil. Saldo: ${Saldo}");
            return true;
        }
    }
}