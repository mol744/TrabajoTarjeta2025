using TarjetaSube;
using NUnit.Framework;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class BoletoTests
    {
        [Test]
        public void Boleto_CreacionCorrecta_Test()
        {
            // Arrange & Act
            Boleto boleto = new Boleto(1580, "12345", "153", 500);

            // Assert
            Assert.AreEqual(1580, boleto.Tarifa);
            Assert.AreEqual("12345", boleto.NumeroTarjeta);
            Assert.AreEqual("153", boleto.LineaColectivo);
            Assert.AreEqual(500, boleto.SaldoRestante);
            Assert.That(boleto.FechaHora, Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }

        [Test]
        public void Boleto_ImprimirBoleto_NoLanzaExcepcion_Test()
        {
            // Arrange
            Boleto boleto = new Boleto(1580, "12345", "153", 500);

            // Act & Assert - Verificar que no lance excepción
            Assert.DoesNotThrow(() => boleto.ImprimirBoleto());
        }

        [Test]
        public void Boleto_ConTarifaCero_Test()
        {
            // Arrange & Act
            Boleto boleto = new Boleto(0, "99999", "123", 1000);

            // Assert
            Assert.AreEqual(0, boleto.Tarifa);
            Assert.AreEqual("99999", boleto.NumeroTarjeta);
            Assert.AreEqual(1000, boleto.SaldoRestante);
        }

        [Test]
        public void Boleto_ConSaldoNegativo_Test()
        {
            // Arrange & Act
            Boleto boleto = new Boleto(1580, "12345", "153", -500);

            // Assert
            Assert.AreEqual(-500, boleto.SaldoRestante);
            Assert.AreEqual(1580, boleto.Tarifa);
        }
    }
}