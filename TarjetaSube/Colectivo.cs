using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        private const decimal TARIFA_BASICA = 1580m;

        public Colectivo(string linea)
        {
            Linea = linea;
        }

        public bool PagarCon(Tarjeta tarjeta)
        {
            
            decimal tarifaAPagar = TARIFA_BASICA;

            if (tarjeta is FranquiciaCompleta )
            {
                tarifaAPagar = 0;
            }
            else if (tarjeta is MedioBoleto)
            {
                tarifaAPagar = TARIFA_BASICA / 2; // Mitad de tarifa
            }

            bool pagoExitoso = tarjeta.PagarBoleto(tarifaAPagar);

            if (pagoExitoso)
            {
                Boleto boleto = new Boleto(tarifaAPagar, tarjeta.Numero.ToString(), Linea, tarjeta.Saldo);
                boleto.ImprimirBoleto();
            }

            return pagoExitoso;
        }

        public void MostrarTarifa()
        {
            Console.WriteLine($"Línea {Linea} - Tarifa: ${TARIFA_BASICA}");
        }
    }
}