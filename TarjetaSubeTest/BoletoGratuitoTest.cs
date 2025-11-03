using TarjetaSube;
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

        [Test]
        public void BoletoGratuito_ObtenerTipoTarjeta_Test()
        {
            // Act & Assert
            Assert.AreEqual("BoletoGratuito", tarjetaGratuita.ObtenerTipoTarjeta());
        }

        [Test]
        public void BoletoGratuito_ObtenerTarifa_Test()
        {
            // Act & Assert
            Assert.AreEqual(0, tarjetaGratuita.ObtenerTarifa(1580m));
        }

        [Test]
        public void BoletoGratuito_ConsultarSaldoYID_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);


            // Act & Assert
            Assert.AreEqual(2000, tarjetaGratuita.ConsultarSaldo());
            Assert.AreEqual(55555, tarjetaGratuita.ConsultarID());
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
            tarjetaGratuita.CargarSaldo(5000); // Más saldo para cubrir viajes adicionales

            // Act - Pagar múltiples veces DIRECTAMENTE
            bool resultado1 = tarjetaGratuita.PagarBoleto(1580);
            bool resultado2 = tarjetaGratuita.PagarBoleto(1580);
            bool resultado3 = tarjetaGratuita.PagarBoleto(1580);

            // Assert - Primeros 2 gratuitos, tercero con tarifa
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(5000 - 1580, tarjetaGratuita.Saldo); // Solo el tercero descuenta
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