using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        public static decimal TARIFA_BASICA => 1580m;
        private static Dictionary<int, (DateTime, string)> _ultimosViajes = new Dictionary<int, DateTime, string>();

        public Colectivo(string linea)
        {
            Linea = linea;
        }

        public bool PagarCon(Tarjeta tarjeta)
        {
            decimal tarifaAPagar = TARIFA_BASICA;

            if (_ultimosViajes.ContainsKey(tarjeta.Numero))
            {
                var (fecha, lineaAnterior) = _ultimosViajes[tarjeta.Numero];
                if ((DateTime.Now - fecha).TotalMinutes <= 60 && lineaAnterior != this.Linea)
                {
                    tarifaAPagar = 0; // 🎉 Trasbordo gratis
                    Console.WriteLine("🎉 TRASBORDO GRATUITO");
                }
            }

            bool pagoExitoso = tarjeta.PagarBoleto(tarifaAPagar);

            if (pagoExitoso)
            {
                _ultimosViajes[tarjeta.Numero] = (DateTime.Now, this.Linea);
                Boleto boleto = new Boleto(tarjeta.ObtenerTarifa(tarifaAPagar), tarjeta.Numero.ToString(), Linea, tarjeta.Saldo);
                boleto.ImprimirBoleto();
            }

            return pagoExitoso;
        }

        public void MostrarTarifa()
        {
            Console.WriteLine($"Línea {Linea} - Tarifa: ${TARIFA_BASICA}");
        }

        public string ObtenerLinea()
        {
           return Linea;

        }
    }
}