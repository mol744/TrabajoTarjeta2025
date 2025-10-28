using System;

namespace TarjetaSube
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== DEMO SISTEMA SUBE ===\n");

            DemoSaldoNegativo();
            DemoFranquicias();

            Console.WriteLine("\n=== FIN DEMO ===");
        }

        static void DemoSaldoNegativo()
        {
            Console.WriteLine("1. DEMO SALDO NEGATIVO:");
            Tarjeta tarjetaNormal = new Tarjeta(1001);
            Colectivo colectivo = new Colectivo("123");

            Console.WriteLine($"Saldo inicial: ${tarjetaNormal.Saldo}");
            colectivo.PagarCon(tarjetaNormal); // -1580
            Console.WriteLine($"Saldo después del viaje: ${tarjetaNormal.Saldo}");
            Console.WriteLine("---\n");
        }

        static void DemoFranquicias()
        {
            Console.WriteLine("2. DEMO FRANQUICIAS:");
            Colectivo colectivo = new Colectivo("153");

            // Medio Boleto
            MedioBoleto medio = new MedioBoleto(2001);
            medio.CargarSaldo(2000);
            Console.WriteLine($"Medio Boleto - Saldo: ${medio.Saldo}");
            colectivo.PagarCon(medio);
            Console.WriteLine($"Medio Boleto - Saldo después: ${medio.Saldo}");

            // Boleto Gratuito
            BoletoGratuito gratuito = new BoletoGratuito(3001);
            Console.WriteLine($"Boleto Gratuito - Saldo: ${gratuito.Saldo}");
            colectivo.PagarCon(gratuito);
            Console.WriteLine($"Boleto Gratuito - Saldo después: ${gratuito.Saldo}");

            // Franquicia Completa
            FranquiciaCompleta franquicia = new FranquiciaCompleta(4001);
            Console.WriteLine($"Franquicia Completa - Saldo: ${franquicia.Saldo}");
            colectivo.PagarCon(franquicia);
            Console.WriteLine($"Franquicia Completa - Saldo después: ${franquicia.Saldo}");
        }
    }
}