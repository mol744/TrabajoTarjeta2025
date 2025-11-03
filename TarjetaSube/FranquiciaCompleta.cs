using System;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        private DateTime _tiempoSimulado;

        // Propiedad para testing (similar a MedioBoleto)
        public Func<DateTime> ObtenerFechaActual { get; set; } = () => DateTime.Now;

        public FranquiciaCompleta(int numero) : base(numero)
        {
            _tiempoSimulado = DateTime.Now;
        }

        public bool EstaEnFranjaHorariaPermitida(DateTime fechaHora)
        {
            // Verificar si es de lunes a viernes (DayOfWeek de 1 a 5)
            if (fechaHora.DayOfWeek == DayOfWeek.Saturday ||
                fechaHora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // Verificar si está entre las 6:00 y 22:00
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
                Console.WriteLine("La Franquicia Completa solo es válida de lunes a viernes entre las 6:00 y 22:00.");
                return false;
            }

            // No restamos saldo - el viaje es gratuito
            Console.WriteLine($"Viaje gratuito con Franquicia Completa. Saldo: ${Saldo}");
            return true;
        }

        public new decimal ObtenerTarifa(decimal tarifa)
        {
            return 0m;
        }

        public override string ObtenerTipoTarjeta()
        {
            return "FranquiciaCompleta";
        }

        public void SimularPasoTiempo(TimeSpan tiempo)
        {
            // Avanzar el tiempo simulado
            _tiempoSimulado = _tiempoSimulado.Add(tiempo);
        }
    }
}