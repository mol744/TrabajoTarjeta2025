using System;
namespace TarjetaSube
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== PRUEBA SALDO NEGATIVO ===");

            // Crear tarjeta y colectivo
            Tarjeta tarjeta = new Tarjeta(12345);
            Colectivo colectivo = new Colectivo("123");

            Console.WriteLine($"Saldo inicial: ${tarjeta.Saldo}");
            Console.WriteLine($"Límite negativo: $-1200");
            Console.WriteLine("---");

            // Prueba 1: Intentar pagar sin saldo (debería permitir hasta -1200)
            Console.WriteLine("1. Intentando pagar primer viaje ($1580) con saldo $0:");
            bool resultado1 = colectivo.PagarCon(tarjeta);
            Console.WriteLine($"¿Pudo pagar? {resultado1}");
            Console.WriteLine($"Saldo después: ${tarjeta.Saldo}");
            Console.WriteLine("---");

            // Prueba 2: Cargar saldo (debería pagar la deuda primero)
            Console.WriteLine("2. Cargando $2000 con saldo negativo:");
            bool resultado2 = tarjeta.CargarSaldo(2000);
            Console.WriteLine($"¿Carga exitosa? {resultado2}");
            Console.WriteLine($"Saldo después: ${tarjeta.Saldo}");
            Console.WriteLine("---");

            // Prueba 3: Intentar superar el límite negativo
            Console.WriteLine("3. Gastando hasta el límite:");
            tarjeta.CargarSaldo(1000); // Cargamos poco
            Console.WriteLine($"Saldo: ${tarjeta.Saldo}");

            colectivo.PagarCon(tarjeta); // Primer viaje
            Console.WriteLine($"Saldo después 1er viaje: ${tarjeta.Saldo}");

            colectivo.PagarCon(tarjeta); // Segundo viaje  
            Console.WriteLine($"Saldo después 2do viaje: ${tarjeta.Saldo}");

            // Este debería fallar
            Console.WriteLine("4. Intentando viaje que superaría límite -1200:");
            bool resultado3 = colectivo.PagarCon(tarjeta);
            Console.WriteLine($"¿Pudo pagar? {resultado3}");
            Console.WriteLine($"Saldo final: ${tarjeta.Saldo}");

            Console.WriteLine("=== FIN PRUEBA ===");
        }
    }
}