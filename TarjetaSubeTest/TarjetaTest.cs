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

        // Tests de saldo negativo
        [Test]
        public void PagarConSaldoNegativo_DentroDelLimite_Test()
        {
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            colectivo.PagarCon(tarjeta); // 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta); // 420 - 1580 = -1160

            bool puedePagar = colectivo.PagarCon(tarjeta); // -1160 - 1580 = -2740 (supera -1200)

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void CargarSaldo_ConSaldoNegativo_Test()
        {
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");
            colectivo.PagarCon(tarjeta); // 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta); // 420 - 1580 = -1160

            bool cargaExitosa = tarjeta.CargarSaldo(2000);

            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(840, tarjeta.Saldo); // -1160 + 2000 = 840
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

        [Test]
        public void PagarBoleto_QueSupereLimiteNegativo_Test()
        {
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            colectivo.PagarCon(tarjeta); // 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta); // 420 - 1580 = -1160

            bool resultado = colectivo.PagarCon(tarjeta); // -1160 - 1580 = -2740 (supera -1200)

            Assert.IsFalse(resultado);
        }
    }
}