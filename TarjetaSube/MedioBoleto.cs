using System;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(int numero) : base(numero)
        {
        }

        public new bool PagarBoleto(decimal tarifa)
        {
            // Medio boleto paga la mitad
            decimal tarifaMedio = tarifa / 2;

            if (Saldo - tarifaMedio < SALDO_MINIMO)
            {
                Console.WriteLine("Saldo insuficiente. Límite negativo alcanzado.");
                return false;
            }

            Saldo -= tarifaMedio;
            Console.WriteLine($"Pago realizado con Medio Boleto. Tarifa: ${tarifaMedio}. Nuevo saldo: ${Saldo}");
            return true;
        }
    }
}