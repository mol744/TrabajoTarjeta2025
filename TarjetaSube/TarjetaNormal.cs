using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaSube
{
    public class TarjetaNormal : Tarjeta
    {
        private List<DateTime> _viajesDelMes;
        private DateTime _mesActual;

        public Func<DateTime> ObtenerFechaActual { get; set; } = () => DateTime.Now;

        public TarjetaNormal(int numero) : base(numero)
        {
            _viajesDelMes = new List<DateTime>();
            _mesActual = DateTime.Now;
        }

        public override bool PagarBoleto(decimal tarifa)
        {
            DateTime ahora = ObtenerFechaActual();
            decimal tarifaReal = ObtenerTarifa(tarifa);

            // Actualizar contador de viajes si cambió el mes
            if (ahora.Month != _mesActual.Month || ahora.Year != _mesActual.Year)
            {
                _viajesDelMes.Clear();
                _mesActual = ahora;
            }

            bool pagoExitoso = base.PagarBoleto(tarifaReal);

            if (pagoExitoso)
            {
                _viajesDelMes.Add(ahora);
            }

            return pagoExitoso;
        }

        public override decimal ObtenerTarifa(decimal tarifa)
        {
            DateTime ahora = ObtenerFechaActual();

            // Si cambió el mes, reiniciar contador
            if (ahora.Month != _mesActual.Month || ahora.Year != _mesActual.Year)
            {
                _viajesDelMes.Clear();
                _mesActual = ahora;
            }

            int viajesRealizados = _viajesDelMes.Count;
            int numeroViajeActual = viajesRealizados + 1; // El próximo viaje será este número

            // Aplicar descuentos según el NÚMERO del viaje actual
            if (numeroViajeActual >= 30 && numeroViajeActual <= 59)
            {
                return tarifa * 0.8m; // 20% de descuento
            }
            else if (numeroViajeActual >= 60 && numeroViajeActual <= 80)
            {
                return tarifa * 0.75m; // 25% de descuento
            }
            else
            {
                return tarifa; // Viajes 1-29 y 81+ - tarifa normal
            }
        }

        public override string ObtenerTipoTarjeta()
        {
            return "TarjetaNormal";
        }

        // Métodos para testing
        public int CantidadViajesEsteMes()
        {
            DateTime ahora = ObtenerFechaActual();
            if (ahora.Month != _mesActual.Month || ahora.Year != _mesActual.Year)
            {
                return 0;
            }
            return _viajesDelMes.Count;
        }

        public void ResetearContadorViajes()
        {
            _viajesDelMes.Clear();
            _mesActual = ObtenerFechaActual();
        }

        public void SimularCambioDeMes()
        {
            _mesActual = _mesActual.AddMonths(-1);
        }
    }
}