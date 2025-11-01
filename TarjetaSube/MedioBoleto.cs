using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        private List<DateTime> _viajesDelDia;
        private DateTime? _ultimoViaje;

        public Func<DateTime> ObtenerFechaActual { get; set; } = () => DateTime.Now;

        public MedioBoleto(int numero) : base(numero)
        {
            _viajesDelDia = new List<DateTime>();
            _ultimoViaje = null;
        }

        public new bool PagarBoleto(decimal tarifa)
        {
            DateTime ahora = ObtenerFechaActual();
            decimal tarifaAPagar = tarifa;

            LimpiarViajesAntiguos(ahora);

            if (_viajesDelDia.Count >= 2)
            {
                tarifaAPagar = tarifa;
                Console.WriteLine($"Tercer viaje del día - Tarifa completa: ${tarifaAPagar}");
            }
            else
            {
                tarifaAPagar = tarifa / 2;

                if (_ultimoViaje.HasValue)
                {
                    TimeSpan tiempoDesdeUltimoViaje = ahora - _ultimoViaje.Value;
                    if (tiempoDesdeUltimoViaje.TotalMinutes < 5)
                    {
                        Console.WriteLine($"No puede pagar otro viaje. Deben pasar 5 minutos entre viajes. Tiempo transcurrido: {tiempoDesdeUltimoViaje.TotalMinutes:F1} minutos.");
                        return false;
                    }
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

        public void ResetearViajes()
        {
            _viajesDelDia.Clear();
            _ultimoViaje = null;
        }

        public void SimularPasoTiempo(TimeSpan tiempo)
        {
        }
    }
}