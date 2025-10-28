using System;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(int numero) : base(numero)
        {
        }

        // La franquicia completa SIEMPRE puede pagar, sin importar el saldo
        public new bool PagarBoleto(decimal tarifa)
        {
            // No restamos saldo - el viaje es gratuito
            Console.WriteLine($"Viaje gratuito con Franquicia Completa. Saldo: ${Saldo}");
            return true;
        }
    }
}