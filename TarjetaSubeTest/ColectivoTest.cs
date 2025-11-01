using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class ColectivoTests
    {
        [Test]
        public void PagarViajeConSaldoSuficiente_Test()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(11111);
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 1580, tarjeta.Saldo);
        }

        [Test]
        public void PagarViajeSinSaldoSuficiente_Test()
        {
            Tarjeta tarjeta = new Tarjeta(22222);
            Colectivo colectivo = new Colectivo("123");

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(puedePagar);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void Colectivo_DiferentesLineas_Test()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(33333);
            tarjeta.CargarSaldo(4000);
            Colectivo colectivo1 = new Colectivo("123");
            Colectivo colectivo2 = new Colectivo("153");

            // Act
            colectivo1.PagarCon(tarjeta);
            colectivo2.PagarCon(tarjeta);

            // Assert
            Assert.AreEqual(4000 - 1580 - 1580, tarjeta.Saldo);
        }

        [Test]
        public void Colectivo_MostrarTarifa_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

            // Act & Assert - Solo verificar que no lance excepción
            Assert.DoesNotThrow(() => colectivo.MostrarTarifa());
        }
        [Test]
        public void Colectivo_ObtenerLinea_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

            // Act & Assert
            Assert.AreEqual("123", colectivo.ObtenerLinea());
        }
    }
}