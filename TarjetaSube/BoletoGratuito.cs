using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaSube
{
    public class BoletoGratuito : Tarjeta
    {
        private List<DateTime> _viajesDelDia;

        public BoletoGratuito(int numero) : base(numero)
        {
            _viajesDelDia = new List<DateTime>();
        }

        public override bool PagarBoleto(decimal tarifa)
        {
            DateTime ahora = DateTime.Now;

            LimpiarViajesAntiguos(ahora);

            if (_viajesDelDia.Count >= 2)
            {
                if (Saldo - tarifa < SALDO_MINIMO)
                {
                    Console.WriteLine("Saldo insuficiente para viaje adicional.");
                    return false;
                }

                Saldo -= tarifa;
                _viajesDelDia.Add(ahora);
                Console.WriteLine($"Viaje adicional con Boleto Estudiantil - Tarifa completa: ${tarifa}. Nuevo saldo: ${Saldo}. Viajes gratuitos hoy: {_viajesDelDia.Count - 1}");
                return true;
            }
            else
            {
                _viajesDelDia.Add(ahora);
                Console.WriteLine($"Viaje gratuito con Boleto Estudiantil. Viajes gratuitos hoy: {_viajesDelDia.Count}/2. Saldo: ${Saldo}");
                return true;
            }
        }

        private void LimpiarViajesAntiguos(DateTime fechaActual)
        {
            _viajesDelDia.RemoveAll(viaje => viaje.Date < fechaActual.Date);
        }

        // Métodos para testing
        public int CantidadViajesGratuitosHoy()
        {
            LimpiarViajesAntiguos(DateTime.Now);
            return _viajesDelDia.Count;
        }

        public void ResetearViajes() // Para testing
        {
            _viajesDelDia.Clear();
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