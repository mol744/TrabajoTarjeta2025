﻿using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class BoletoGratuitoTests
    {
        private BoletoGratuito tarjetaGratuita;

        [SetUp]
        public void Setup()
        {
            tarjetaGratuita = new BoletoGratuito(55555);
        }

        [Test]
        public void BoletoGratuito_NoDescuestaSaldo_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool resultado = colectivo.PagarCon(tarjetaGratuita);

            // Assert - Saldo debería seguir en 2000 (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo);
        }

        // Tests DIRECTOS al método PagarBoleto (para cobertura 100%)
        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_SiemprePermitePago_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);

            // Act - Llamar DIRECTAMENTE al método
            bool resultado = tarjetaGratuita.PagarBoleto(1580);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo); // No descuenta saldo
        }

        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_MultiplesVeces_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);

            // Act - Pagar múltiples veces DIRECTAMENTE
            bool resultado1 = tarjetaGratuita.PagarBoleto(1580);
            bool resultado2 = tarjetaGratuita.PagarBoleto(1580);
            bool resultado3 = tarjetaGratuita.PagarBoleto(1580);

            // Assert - Todos deberían funcionar (gratuitos)
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo); // Saldo no cambia
        }

        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_SinSaldo_Test()
        {
            // Arrange - Saldo 0

            // Act - Llamar DIRECTAMENTE al método sin saldo
            bool resultado = tarjetaGratuita.PagarBoleto(1580);

            // Assert - Debería permitir el pago (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(0, tarjetaGratuita.Saldo);
        }
    }
}