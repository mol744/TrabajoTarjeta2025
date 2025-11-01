using System;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(int numero) : base(numero)
        {
        }

        public override bool PagarBoleto(decimal tarifa)
        {
            // Medio boleto paga la mitad

            if (Saldo - tarifa < SALDO_MINIMO)
            {
                Console.WriteLine("Saldo insuficiente. Límite negativo alcanzado.");
                return false;
            }

            Saldo -= tarifa;
            Console.WriteLine($"Pago realizado con Medio Boleto. Tarifa: ${tarifa}. Nuevo saldo: ${Saldo}");
            return true;
        }
    }
}