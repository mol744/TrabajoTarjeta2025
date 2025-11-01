using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TarjetaTests
    {
        private Tarjeta tarjeta;

        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(12345);
        }

        // Tests de carga de saldo
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
            Assert.IsFalse(resultado);
        }

        // Tests de pago parcial
        [Test]
        public void PagarBoleto_PagoParcial_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(2000); // Tiene 600
            decimal tarifa = 700;     // Boleto vale 700

            // Act
            bool resultado = tarjeta.PagarBoleto(tarifa);

            // Assert - Debería pagar parcialmente 600
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 700, tarjeta.Saldo);
        }

        [Test]
        public void PagarBoleto_PagoParcialHastaLimiteNegativo_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(200); // Tiene 200
            decimal tarifa = 1500;    // Boleto vale 1500
            // Puede pagar hasta: 200 - (-1200) = 1400

            // Act
            bool resultado = tarjeta.PagarBoleto(tarifa);

            // Assert - Debería pagar 1400 y quedar en -1200
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void PagarBoleto_NoPuedePagarNada_Test()
        {
            // Arrange
            tarjeta.Saldo = -1200; // Ya está en el límite
            decimal tarifa = 100;

            // Act
            bool resultado = tarjeta.PagarBoleto(tarifa);

            // Assert - No puede pagar nada
            Assert.IsFalse(resultado);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void ObtenerTipoTarjeta_Normal_Test()
        {
            // Act & Assert
            Assert.AreEqual("Tarjeta Común", tarjeta.ObtenerTipoTarjeta());
        }

        [Test]
        public void ConsultarSaldo_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(3000);

            // Act & Assert
            Assert.AreEqual(3000, tarjeta.ConsultarSaldo());
        }

        [Test]
        public void ConsultarID_Test()
        {
            // Act & Assert
            Assert.AreEqual(12345, tarjeta.ConsultarID());
        }

        // Tests de límites
        [Test]
        public void CargarSaldo_SuperaLimiteMaximo_Test()
        {
            tarjeta.CargarSaldo(30000); // 30000
            bool resultado = tarjeta.CargarSaldo(15000); // Superaría 40000

            Assert.IsFalse(resultado);
            Assert.AreEqual(30000, tarjeta.Saldo);
        }
    }
}