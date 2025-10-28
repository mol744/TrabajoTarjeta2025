using System;

namespace TarjetaSube
{
    public class Boleto
    {
        public DateTime FechaHora { get; private set; }
        public decimal Tarifa { get; private set; }
        public string NumeroTarjeta { get; private set; }
        public string LineaColectivo { get; private set; }
        public decimal SaldoRestante { get; private set; }

        public Boleto(decimal tarifa, string numeroTarjeta, string lineaColectivo, decimal saldoRestante)
        {
            FechaHora = DateTime.Now;
            Tarifa = tarifa;
            NumeroTarjeta = numeroTarjeta;
            LineaColectivo = lineaColectivo;
            SaldoRestante = saldoRestante;
        }

        public void ImprimirBoleto()
        {
            Console.WriteLine($"| Fecha: {FechaHora} | Tarifa: {Tarifa} | Nro: {NumeroTarjeta} | Línea: {LineaColectivo} | Saldo: {SaldoRestante} |");
        }
    }
}