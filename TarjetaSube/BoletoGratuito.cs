using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaSube
{
    public class BoletoGratuito : Tarjeta
    {
        private List<DateTime> _viajesDelDia;
        private DateTime _tiempoSimulado;

        public Func<DateTime> ObtenerFechaActual { get; set; } = () => DateTime.Now;

        public BoletoGratuito(int numero) : base(numero)
        {
            _viajesDelDia = new List<DateTime>();
        }

        public bool EstaEnFranjaHorariaPermitida(DateTime fechaHora)
        {
            if (fechaHora.DayOfWeek == DayOfWeek.Saturday ||
                fechaHora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            if (fechaHora.Hour < 6 || fechaHora.Hour >= 22)
            {
                return false;
            }

            return true;
        }

        public override bool PagarBoleto(decimal tarifa)
        {
            DateTime ahora = ObtenerFechaActual();

            if (!EstaEnFranjaHorariaPermitida(ahora))
            {
                Console.WriteLine("El Boleto Gratuito solo es válido de lunes a viernes entre las 6:00 y 22:00.");
                return false;
            }

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

        public int CantidadViajesGratuitosHoy()
        {
            LimpiarViajesAntiguos(ObtenerFechaActual());
            return _viajesDelDia.Count;
        }

        public void ResetearViajes() // Para testing
        {
            _viajesDelDia.Clear();
        }

        public override decimal ObtenerTarifa(decimal tarifa)
        {
            DateTime ahora = ObtenerFechaActual();

            // Si está fuera de franja horaria, tarifa completa
            if (!EstaEnFranjaHorariaPermitida(ahora))
            {
                return tarifa;
            }

            // Si está dentro de franja horaria, aplicar lógica de boleto gratuito
            LimpiarViajesAntiguos(ahora);

            if (_viajesDelDia.Count >= 2)
            {
                return tarifa; // A partir del tercer viaje, tarifa completa
            }
            else
            {
                return 0m; // Primeros 2 viajes gratuitos
            }
        }

        public override string ObtenerTipoTarjeta()
        {
            return "BoletoGratuito";
        }
    }
}