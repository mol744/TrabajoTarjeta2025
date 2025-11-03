using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public abstract class Tarjeta
    {
        public int Numero { get; private set; }
        public decimal Saldo { get; set; }
        public const decimal SALDO_MAXIMO = 40000m;
        public const decimal SALDO_MINIMO = -1200m;
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

            decimal nuevoSaldo = Saldo + montoACargar;

            if (nuevoSaldo > SALDO_MAXIMO)
            {
                Console.WriteLine($"No se puede cargar. Saldo máximo: ${SALDO_MAXIMO}");
                return false;
            }

            Saldo = nuevoSaldo;
            Console.WriteLine($"Carga exitosa. Nuevo saldo: ${Saldo}");
            return true;
        }

        public virtual bool PagarBoleto(decimal tarifa)
        {
            if (Saldo - tarifa < SALDO_MINIMO)
            {
                    Console.WriteLine("Saldo insuficiente. Límite negativo alcanzado.");
                    return false;
            }

            Saldo -= tarifa;
            Console.WriteLine($"Pago COMPLETO realizado. Tarifa: ${tarifa}. Nuevo saldo: ${Saldo}");
            return true;
        }

        public virtual decimal ObtenerTarifa(decimal tarifa)
        {
            return tarifa;
        }

        public abstract string ObtenerTipoTarjeta();

        public decimal ConsultarSaldo()
        {
            return Saldo;
        }


        public int ConsultarID()
        {
            return Numero;
        }
    }
}