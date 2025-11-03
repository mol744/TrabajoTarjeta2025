using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class BoletoGratuitoLimitacionTests
    {
        private BoletoGratuito tarjetaGratuita;

        [SetUp]
        public void Setup()
        {
            tarjetaGratuita = new BoletoGratuito(55555);
            tarjetaGratuita.ResetearViajes(); // Asegurar estado limpio para cada test
        }

        [Test]
        public void BoletoGratuito_PrimerosDosViajes_Gratuitos_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");
            int saldoInicial = 2000; // Cambiar decimal por int
            tarjetaGratuita.CargarSaldo(saldoInicial);

            // Act - Realizar 2 viajes gratuitos
            bool primerViaje = colectivo.PagarCon(tarjetaGratuita);
            bool segundoViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.IsTrue(primerViaje, "El primer viaje debería ser exitoso y gratuito");
            Assert.IsTrue(segundoViaje, "El segundo viaje debería ser exitoso y gratuito");
            Assert.AreEqual(saldoInicial, tarjetaGratuita.Saldo, "El saldo no debería cambiar en viajes gratuitos");
            Assert.AreEqual(2, tarjetaGratuita.CantidadViajesGratuitosHoy());
        }

        [Test]
        public void BoletoGratuito_TercerViajeDelDia_TarifaCompleta_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");
            int saldoInicial = 5000; // Cambiar decimal por int
            tarjetaGratuita.CargarSaldo(saldoInicial);

            // Act - Realizar 3 viajes
            bool primerViaje = colectivo.PagarCon(tarjetaGratuita);  // Gratuito
            bool segundoViaje = colectivo.PagarCon(tarjetaGratuita); // Gratuito
            bool tercerViaje = colectivo.PagarCon(tarjetaGratuita);  // Tarifa completa

            // Assert
            Assert.IsTrue(primerViaje, "Primer viaje debería ser exitoso y gratuito");
            Assert.IsTrue(segundoViaje, "Segundo viaje debería ser exitoso y gratuito");
            Assert.IsTrue(tercerViaje, "Tercer viaje debería ser exitoso pero con tarifa completa");
            Assert.AreEqual(saldoInicial - 1580, tarjetaGratuita.Saldo, "El tercer viaje debería cobrar tarifa completa");
            Assert.AreEqual(3, tarjetaGratuita.CantidadViajesGratuitosHoy());
        }

        [Test]
        public void BoletoGratuito_CuartoViajeDelDia_TarifaCompleta_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");
            int saldoInicial = 10000; // Cambiar decimal por int
            tarjetaGratuita.CargarSaldo(saldoInicial);

            // Act - Realizar 4 viajes
            bool primerViaje = colectivo.PagarCon(tarjetaGratuita);  // Gratuito
            bool segundoViaje = colectivo.PagarCon(tarjetaGratuita); // Gratuito
            bool tercerViaje = colectivo.PagarCon(tarjetaGratuita);  // Tarifa completa
            bool cuartoViaje = colectivo.PagarCon(tarjetaGratuita);  // Tarifa completa

            // Assert
            Assert.IsTrue(primerViaje, "Primer viaje debería ser exitoso");
            Assert.IsTrue(segundoViaje, "Segundo viaje debería ser exitoso");
            Assert.IsTrue(tercerViaje, "Tercer viaje debería ser exitoso");
            Assert.IsTrue(cuartoViaje, "Cuarto viaje debería ser exitoso");

            // Verificar que se cobró tarifa completa en el 3er y 4to viaje
            decimal totalCobrado = 1580 * 2; // 3er y 4to viaje
            Assert.AreEqual(saldoInicial - totalCobrado, tarjetaGratuita.Saldo);
            Assert.AreEqual(4, tarjetaGratuita.CantidadViajesGratuitosHoy());
        }

        [Test]
        public void BoletoGratuito_TercerViaje_SaldoInsuficiente_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");
            // Saldo inicial: 0 (no cargar saldo)

            // Act - Realizar 2 viajes gratuitos
            bool primerViaje = colectivo.PagarCon(tarjetaGratuita);  // Gratuito
            bool segundoViaje = colectivo.PagarCon(tarjetaGratuita); // Gratuito

            // Intentar tercer viaje sin saldo
            bool tercerViaje = colectivo.PagarCon(tarjetaGratuita);  // Tarifa completa (debería fallar)

            // Assert
            Assert.IsTrue(primerViaje, "Primer viaje gratuito debería funcionar");
            Assert.IsTrue(segundoViaje, "Segundo viaje gratuito debería funcionar");
            Assert.IsFalse(tercerViaje, "Tercer viaje debería fallar por saldo insuficiente");
            Assert.AreEqual(0, tarjetaGratuita.Saldo);
            Assert.AreEqual(2, tarjetaGratuita.CantidadViajesGratuitosHoy());
        }

        [Test]
        public void BoletoGratuito_ReiniciaContadorAlDiaSiguiente_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");
            int saldoInicial = 5000; // Cambiar decimal por int
            tarjetaGratuita.CargarSaldo(saldoInicial);

            // Act - Realizar 2 viajes "hoy" (gratuitos)
            tarjetaGratuita.PagarBoleto(1580); // Viaje 1 gratuito
            tarjetaGratuita.PagarBoleto(1580); // Viaje 2 gratuito

            int viajesHoy = tarjetaGratuita.CantidadViajesGratuitosHoy();
            decimal saldoDespuesDosViajes = tarjetaGratuita.Saldo;

            // Simular nuevo día (resetear)
            tarjetaGratuita.ResetearViajes();

            // Viaje en "nuevo día" - debería ser gratuito
            tarjetaGratuita.PagarBoleto(1580);

            // Assert
            Assert.AreEqual(2, viajesHoy, "Debería tener 2 viajes antes del reset");
            Assert.AreEqual(saldoInicial, saldoDespuesDosViajes, "El saldo no debería cambiar en viajes gratuitos");
            Assert.AreEqual(1, tarjetaGratuita.CantidadViajesGratuitosHoy(), "Debería tener 1 viaje en el nuevo día");
        }

        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_RespetaLimites_Test()
        {
            // Arrange
            int saldoInicial = 5000; // Cambiar decimal por int
            tarjetaGratuita.CargarSaldo(saldoInicial);

            // Act - Llamar DIRECTAMENTE al método 3 veces
            bool resultado1 = tarjetaGratuita.PagarBoleto(1580); // Gratuito
            bool resultado2 = tarjetaGratuita.PagarBoleto(1580); // Gratuito
            bool resultado3 = tarjetaGratuita.PagarBoleto(1580); // Tarifa completa

            // Assert
            Assert.IsTrue(resultado1, "Primer viaje directo debería ser gratuito");
            Assert.IsTrue(resultado2, "Segundo viaje directo debería ser gratuito");
            Assert.IsTrue(resultado3, "Tercer viaje directo debería cobrar tarifa completa");
            Assert.AreEqual(saldoInicial - 1580, tarjetaGratuita.Saldo, "Solo el tercer viaje debería cobrarse");
        }
    }
}