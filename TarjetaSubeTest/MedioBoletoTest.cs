using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class MedioBoletoTests
    {
        private MedioBoleto tarjetaMedio;

        [SetUp]
        public void Setup()
        {
            tarjetaMedio = new MedioBoleto(77777);
        }

        [Test]
        public void MedioBoleto_PagaMitad_Test()
        {
            tarjetaMedio.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            bool puedePagar = colectivo.PagarCon(tarjetaMedio);

            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);
            Assert.AreEqual(1580/2, tarjetaMedio.ObtenerTarifa(1580m));
        }

        [Test]
        public void MedioBoleto_PagoParcial_Test()
        {
            tarjetaMedio.CargarSaldo(2000); 

            bool resultado = tarjetaMedio.PagarBoleto(1580);

            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_ObtenerTipoTarjeta_Test()
        {
            Assert.AreEqual("MedioBoleto", tarjetaMedio.ObtenerTipoTarjeta());
        }

        [Test]
        public void MedioBoleto_ConsultarSaldoYID_Test()
        {
            tarjetaMedio.CargarSaldo(2000);

            Assert.AreEqual(2000, tarjetaMedio.ConsultarSaldo());
            Assert.AreEqual(77777, tarjetaMedio.ConsultarID());
        }
    }
}