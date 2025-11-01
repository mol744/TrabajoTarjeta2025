using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class FranquiciaCompletaTests
    {
        private FranquiciaCompleta tarjetaFranquicia;

        [SetUp]
        public void Setup()
        {
            tarjetaFranquicia = new FranquiciaCompleta(99999);
        }

        [Test]
        public void FranquiciaCompleta_SiemprePuedePagar_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

            // Act - Intentar pagar con saldo 0
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Debería poder pagar siempre
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(0, tarjetaFranquicia.Saldo); // El saldo no cambia
        }

        [Test]
        public void FranquiciaCompleta_ObtenerTipoTarjeta_Test()
        {
            // Act & Assert
            Assert.AreEqual("FranquiciaCompleta", tarjetaFranquicia.ObtenerTipoTarjeta());
        }

        [Test]
        public void FranquiciaCompleta_ConsultarSaldoYID_Test()
        {
            tarjetaFranquicia.CargarSaldo(3000); 

            // Act & Assert
            Assert.AreEqual(3000, tarjetaFranquicia.ConsultarSaldo());
            Assert.AreEqual(99999, tarjetaFranquicia.ConsultarID());
        }
    }
}