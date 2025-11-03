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
            tarjeta = new TarjetaNormal(12345);
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
            Assert.IsFalse(resultado);
        }

<<<<<<< HEAD
        // Tests de pago parcial
=======
>>>>>>> origin/acreditacion_saldo
        [Test]
        public void PagarBoleto_PagoParcial_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(2000); // Tiene 600
            decimal tarifa = 700;     // Boleto vale 700

<<<<<<< HEAD
            // Act
            bool resultado = tarjeta.PagarBoleto(tarifa);

            // Assert - Debería pagar parcialmente 600
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 700, tarjeta.Saldo);
=======
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(-1160, tarjeta.Saldo);
>>>>>>> origin/acreditacion_saldo
        }

        [Test]
        public void PagarBoleto_PagoParcialHastaLimiteNegativo_Test()
        {
<<<<<<< HEAD
            // Arrange
            tarjeta.CargarSaldo(200); // Tiene 200
            decimal tarifa = 1500;    // Boleto vale 1500
            // Puede pagar hasta: 200 - (-1200) = 1400
=======
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
>>>>>>> origin/acreditacion_saldo

            // Act
            bool resultado = tarjeta.PagarBoleto(tarifa);

<<<<<<< HEAD
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
            Assert.AreEqual("TarjetaNormal", tarjeta.ObtenerTipoTarjeta());
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
=======
            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(840, tarjeta.Saldo);
>>>>>>> origin/acreditacion_saldo
        }

        [Test]
        public void CargarSaldo_SuperaLimiteMaximo_Test()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            bool resultado = tarjeta.CargarSaldo(10000);

            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(14000, tarjeta.Acargar);
        }
<<<<<<< HEAD
=======

        [Test]
        public void PagarBoleto_QueSupereLimiteNegativo_Test()
        {
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool resultado = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(resultado);
        }

        [Test]
        public void AcreditarCarga_Completamente_CuandoHayEspacioSuficiente_Test()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.PagarBoleto(10000);

            bool resultado = tarjeta.AcreditarCarga();

            Assert.IsTrue(resultado);
            Assert.AreEqual(50000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.Acargar);
        }

        [Test]
        public void AcreditarCarga_Parcialmente_CuandoNoHayEspacioSuficiente_Test()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.PagarBoleto(2000);

            bool resultado = tarjeta.AcreditarCarga();

            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2000, tarjeta.Acargar);
        }

        [Test]
        public void CargarSaldo_ConSaldoPendienteYAcreditar_Test()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            bool pagoExitoso = tarjeta.PagarBoleto(5000);

            Assert.IsTrue(pagoExitoso);
            Assert.AreEqual(55000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.Acargar);
        }

        [Test]
        public void CargarSaldo_MontoNoPermitido_ConSaldoPendiente_Test()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            bool resultado = tarjeta.CargarSaldo(100);

            Assert.IsFalse(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.Acargar);
        }
>>>>>>> origin/acreditacion_saldo
    }
}