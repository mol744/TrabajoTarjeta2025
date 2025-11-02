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

        [Test]
        public void PagarConSaldoNegativo_DentroDelLimite_Test()
        {
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void CargarSaldo_ConSaldoNegativo_Test()
        {
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool cargaExitosa = tarjeta.CargarSaldo(2000);

            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(840, tarjeta.Saldo);
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
    }
}