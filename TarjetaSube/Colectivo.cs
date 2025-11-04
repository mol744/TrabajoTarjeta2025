using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        public bool EsInterurbana { get; private set; }
        public static decimal TARIFA_BASICA => 1580m;
        public static decimal TARIFA_INTERURBANA => 3000m;
        private static Dictionary<int, (DateTime, string)> _ultimosViajes = new Dictionary<int, (DateTime, string)>();

        public Colectivo(string linea, bool esInterurbana = false)
        {
            Linea = linea;
            EsInterurbana = esInterurbana;
        }

        public bool PagarCon(Tarjeta tarjeta)
        {
            decimal tarifaAPagar = EsInterurbana ? TARIFA_INTERURBANA : TARIFA_BASICA;

            // Aplicar trasbordo gratuito (solo para líneas urbanas)
            if (!EsInterurbana && _ultimosViajes.ContainsKey(tarjeta.Numero))
            {
                var (fecha, lineaAnterior) = _ultimosViajes[tarjeta.Numero];
                if ((DateTime.Now - fecha).TotalMinutes <= 60 && lineaAnterior != this.Linea)
                {
                    tarifaAPagar = 0;
                    Console.WriteLine("TRASBORDO GRATUITO");
                }
            }

            bool pagoExitoso = tarjeta.PagarBoleto(tarifaAPagar);

            if (pagoExitoso)
            {
                // Solo registrar para trasbordo si es línea urbana
                if (!EsInterurbana)
                {
                    _ultimosViajes[tarjeta.Numero] = (DateTime.Now, this.Linea);
                }
                Boleto boleto = new Boleto(tarjeta.ObtenerTarifa(tarifaAPagar), tarjeta.Numero.ToString(), Linea, tarjeta.Saldo);
                boleto.ImprimirBoleto();
            }

            return pagoExitoso;
        }

        public void MostrarTarifa()
        {
            decimal tarifa = EsInterurbana ? TARIFA_INTERURBANA : TARIFA_BASICA;
            string tipo = EsInterurbana ? "Interurbana" : "Urbana";
            Console.WriteLine($"Línea {Linea} ({tipo}) - Tarifa: ${tarifa}");
        }

        public string ObtenerLinea()
        {
            return Linea;
        }

        public decimal ObtenerTarifaActual()
        {
            return EsInterurbana ? TARIFA_INTERURBANA : TARIFA_BASICA;
        }
    }
}