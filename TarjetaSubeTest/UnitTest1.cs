using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    public class Tests
    {
        private Tarjeta tarjeta;

        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(12345); // Necesita número ahora
        }

        [Test]
        public void CargaMontoPermitido_2000_Test()
        {
            bool resultado = tarjeta.CargarSaldo(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void CargaMontoNoPermitido_100_Test()
        {
            bool resultado = tarjeta.CargarSaldo(100);
            Assert.IsFalse(resultado); // Debe fallar porque 100 no está en montos permitidos
        }

        [Test]
        public void PagarViajeConSaldoSuficiente_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(2000);
            var colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 1580, tarjeta.Saldo); // 1580 es la tarifa
        }

        [Test]
        public void PagarViajeSinSaldoSuficiente_Test()
        {
            // Arrange - tarjeta con saldo 0
            var colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsFalse(puedePagar);
            Assert.AreEqual(0, tarjeta.Saldo);
        }
    }
}