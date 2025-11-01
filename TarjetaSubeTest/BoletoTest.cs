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
        public void Boleto_ObtenerFechaHora_Test()
        {
            // Arrange
            Boleto boleto = new Boleto(1580, "12345", "153", 500);

            // Act & Assert
            Assert.That(boleto.ObtenerFechaHora(), Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }

        [Test]
        public void Boleto_ObtenerTotalAbonado_Test()
        {
            // Arrange
            Boleto boleto = new Boleto(1580, "12345", "153", 500);

            // Act & Assert
            Assert.AreEqual(1580, boleto.ObtenerTotalAbonado());
        }

        [Test]
        public void Boleto_InformarMontoConSaldoNegativo_NoLanzaExcepcion_Test()
        {
            // Arrange
            Boleto boleto = new Boleto(2000, "12345", "153", -500);

            // Act & Assert - Verificar que no lance excepción
            Assert.DoesNotThrow(() => boleto.InformarMontoConSaldoNegativo(1580));
        }
    }
}