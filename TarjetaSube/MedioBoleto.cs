using System;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(int numero) : base(numero)
        {
        }

        public override bool PagarBoleto(decimal tarifa)
        {
            decimal tarifaMedio = tarifa / 2;
            return base.PagarBoleto(tarifaMedio);
        }
        public new decimal ObtenerTarifa(decimal tarifa)
        {
            return tarifa / 2;
        }
        public override string ObtenerTipoTarjeta()
        {
            return "MedioBoleto";
        }

    }
}