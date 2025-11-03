using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TarjetaSube
{
    public abstract class Tarjeta
    {
        public int Numero { get; private set; }
        public decimal Saldo { get; set; }
        public decimal Acargar { get; set; }
        public const decimal SALDO_MAXIMO = 56000m;
        public const decimal SALDO_MINIMO = -1200m;
        private static readonly List<int> MONTOS_PERMITIDOS = new List<int>
            { 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 };

        public Tarjeta(int numero)
        {
            Numero = numero;
            Saldo = 0;
            Acargar = 0;
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
                decimal pendiente = nuevoSaldo - SALDO_MAXIMO;
                Acargar += pendiente;
                Console.WriteLine($"Saldo máximo alcanzado: ${SALDO_MAXIMO} , Saldo pendiente: ${Acargar} ");
                Saldo = SALDO_MAXIMO;
                return true;
            }

            Saldo = nuevoSaldo;
            Console.WriteLine($"Carga exitosa. Nuevo saldo: ${Saldo}");
            return true;
        }

        public virtual bool PagarBoleto(decimal tarifa)
        {
            if (Saldo - tarifa < SALDO_MINIMO)
            {
                Console.WriteLine("Saldo insuficiente.");
                return false;
            }
            Saldo -= tarifa;
            if (Acargar > 0)
            {
                AcreditarCarga();
            }
            Console.WriteLine($"Pago realizado. Nuevo saldo: ${Saldo}");
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

        public bool AcreditarCarga()
        {
            decimal espacioDisponible = SALDO_MAXIMO - Saldo;
            if (espacioDisponible >= Acargar)
            {
                Saldo += Acargar;
                Acargar = 0;
                Console.WriteLine($"Carga acreditada. Nuevo saldo: ${Saldo}");
                return true;
            }
            decimal nuevoSaldo = Saldo + Acargar;
            decimal pendiente = nuevoSaldo - SALDO_MAXIMO;
            Acargar = pendiente;
            Console.WriteLine($"Saldo máximo alcanzado: ${SALDO_MAXIMO} , Saldo pendiente: ${Acargar} ");
            Saldo = SALDO_MAXIMO;
            return true;

        }
    }
}   

