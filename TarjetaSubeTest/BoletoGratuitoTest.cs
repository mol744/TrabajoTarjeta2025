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
        }
    }
}