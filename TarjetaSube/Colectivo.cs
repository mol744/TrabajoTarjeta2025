﻿using System;

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
            bool pagoExitoso = tarjeta.PagarBoleto(TARIFA_BASICA);

            if (pagoExitoso)
            {
                Boleto boleto = new Boleto(TARIFA_BASICA, tarjeta.Numero.ToString(), Linea, tarjeta.Saldo);
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