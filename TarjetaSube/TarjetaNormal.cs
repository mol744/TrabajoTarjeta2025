namespace TarjetaSube
{
    public class TarjetaNormal : Tarjeta
    {
        public TarjetaNormal(int numero) : base(numero) { }

        // Usa el comportamiento POR DEFECTO de la clase base (paga tarifa completa)
        public override string ObtenerTipoTarjeta()
        {
            return "TarjetaNormal";
        }
    }
}