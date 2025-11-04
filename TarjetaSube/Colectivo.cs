using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        public bool EsInterurbana { get; private set; }
        public static decimal TARIFA_BASICA => 1580m;
        public static decimal TARIFA_INTERURBANA => 3000m;

        public Colectivo(string linea, bool esInterurbana = false)
        {
            Linea = linea;
            EsInterurbana = esInterurbana;
        }

        public bool PagarCon(Tarjeta tarjeta)
        {
            decimal tarifaAPagar = EsInterurbana ? TARIFA_INTERURBANA : TARIFA_BASICA;

            bool pagoExitoso = tarjeta.PagarBoleto(tarifaAPagar);

            if (pagoExitoso)
            {
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