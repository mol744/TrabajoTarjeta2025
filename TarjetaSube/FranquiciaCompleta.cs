using System;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(int numero) : base(numero)
        {
        }

        public new bool PagarBoleto(decimal tarifa)
        {
            // No restamos saldo - el viaje es gratuito
            Console.WriteLine($"Viaje gratuito con Franquicia Completa. Saldo: ${Saldo}");
            return true;
        }
        public virtual string ObtenerTipoTarjeta()
        {
            return "FranquiciaCompleta";
        }
    }
}