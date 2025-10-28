using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        public int Numero { get; private set; }
        public decimal Saldo { get; private set; }
        private const decimal SALDO_MAXIMO = 40000m;
        private static readonly List<int> MONTOS_PERMITIDOS = new List<int>
            { 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 };

        public Tarjeta(int numero)
        {
            Numero = numero;
            Saldo = 0;
        }

        public bool CargarSaldo(int montoACargar)
        {
            if (!MONTOS_PERMITIDOS.Contains(montoACargar))
            {
                Console.WriteLine($"Monto ${montoACargar} no permitido.");
                return false;
            }

            if (Saldo + montoACargar > SALDO_MAXIMO)
            {
                Console.WriteLine($"No se puede cargar. Saldo máximo: ${SALDO_MAXIMO}");
                return false;
            }

            Saldo += montoACargar;
            Console.WriteLine($"Carga exitosa. Nuevo saldo: ${Saldo}");
            return true;
        }

        public bool PagarBoleto(decimal tarifa)
        {
            if (Saldo < tarifa)
            {
                Console.WriteLine("Saldo insuficiente.");
                return false;
            }

            Saldo -= tarifa;
            Console.WriteLine($"Pago realizado. Nuevo saldo: ${Saldo}");
            return true;
        }
    }
}