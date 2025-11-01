using System;

namespace TarjetaSube
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(int numero) : base(numero)
        {
        }

        public new bool PagarBoleto(decimal tarifa)
        {
            decimal tarifaMedio = tarifa / 2;
            return base.PagarBoleto(tarifaMedio);
        }
        public virtual string ObtenerTipoTarjeta()
        {
            return "MedioBoleto";
        }

    }
}