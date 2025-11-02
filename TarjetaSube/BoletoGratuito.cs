using System;

namespace TarjetaSube
{
    public class BoletoGratuito : Tarjeta
    {
        public BoletoGratuito(int numero) : base(numero)
        {
        }

        public override bool PagarBoleto(decimal tarifa)
        {
            // Totalmente gratuito - siempre puede pagar
            Console.WriteLine($"Viaje gratuito con Boleto Estudiantil. Saldo: ${Saldo}");
            return true;
        }

        public new decimal ObtenerTarifa(decimal tarifa)
        {
            return 0m;
        }

        public override string ObtenerTipoTarjeta()
        {
            return "BoletoGratuito";
        }


    }
}