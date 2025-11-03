using System;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(int numero) : base(numero)
        {
        }


        public override bool PagarBoleto(decimal tarifa)
        {
            // No restamos saldo - el viaje es gratuito
            Console.WriteLine($"Viaje gratuito con Franquicia Completa. Saldo: ${Saldo}");
            return true;
        }

        public new decimal ObtenerTarifa(decimal tarifa)
        {
            return 0m;
        }
        public override string ObtenerTipoTarjeta()
        {
            return "FranquiciaCompleta";
        }
    }
}