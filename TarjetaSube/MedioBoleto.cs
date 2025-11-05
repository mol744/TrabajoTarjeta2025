using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        private List<DateTime> _viajesDelDia;
        private DateTime? _ultimoViaje;
        public DateTime _tiempoSimulado;

        public Func<DateTime> ObtenerFechaActual { get; set; } = () => DateTime.Now;

        public MedioBoleto(int numero) : base(numero)
        {
            _viajesDelDia = new List<DateTime>();
            _ultimoViaje = null;
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
                Console.WriteLine("El Medio Boleto solo es válido de lunes a viernes entre las 6:00 y 22:00.");
                return false;
            }

            LimpiarViajesAntiguos(ahora);
            decimal tarifaAPagar = ObtenerTarifa(tarifa);

            if (_ultimoViaje.HasValue)
            {
                TimeSpan tiempoDesdeUltimoViaje = ahora - _ultimoViaje.Value;
                if (tiempoDesdeUltimoViaje.TotalMinutes < 5)
                {
                    Console.WriteLine($"No puede pagar otro viaje. Deben pasar 5 minutos entre viajes. Tiempo transcurrido: {tiempoDesdeUltimoViaje.TotalMinutes:F1} minutos.");
                    return false;
                }
            }

            if (Saldo - tarifaAPagar < SALDO_MINIMO)
            {
                Console.WriteLine("Saldo insuficiente. Límite negativo alcanzado.");
                return false;
            }

            Saldo -= tarifaAPagar;
            _viajesDelDia.Add(ahora);
            _ultimoViaje = ahora;

            string tipoViaje = _viajesDelDia.Count <= 2 ? "Medio Boleto" : "Tarifa Completa";
            Console.WriteLine($"Pago realizado con {tipoViaje}. Tarifa: ${tarifaAPagar}. Nuevo saldo: ${Saldo}. Viajes hoy: {_viajesDelDia.Count}");

            return true;
        }

        private void LimpiarViajesAntiguos(DateTime fechaActual)
        {
            _viajesDelDia.RemoveAll(viaje => viaje.Date < fechaActual.Date);
        }

        public int CantidadViajesHoy()
        {
            LimpiarViajesAntiguos(ObtenerFechaActual());
            return _viajesDelDia.Count;
        }

        public TimeSpan? TiempoDesdeUltimoViaje()
        {
            if (!_ultimoViaje.HasValue) return null;
            return ObtenerFechaActual() - _ultimoViaje.Value;
        }

        public void ResetearViajes() // Para testing
        {
            _viajesDelDia.Clear();
            _ultimoViaje = null;
        }

        public override decimal ObtenerTarifa(decimal tarifa)
        {
            DateTime ahora = ObtenerFechaActual();

            // Si está fuera de franja horaria, tarifa completa
            if (!EstaEnFranjaHorariaPermitida(ahora))
            {
                return tarifa;
            }

            LimpiarViajesAntiguos(ahora);

            if (_viajesDelDia.Count >= 2)
            {
                return tarifa;
            }
            else
            {
                return tarifa / 2;
            }
        }

        public override string ObtenerTipoTarjeta()
        {
            return "MedioBoleto";
        }
    }
}