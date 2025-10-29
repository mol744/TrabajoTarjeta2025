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
            // Arrange
            tarjetaMedio.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaMedio);

            // Assert - Debería pagar la mitad: 1580 / 2 = 790
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_ComparacionConTarjetaNormal_Test()
        {
            // Arrange
            MedioBoleto tarjetaMedio = new MedioBoleto(77777);
            Tarjeta tarjetaNormal = new Tarjeta(66666);

            tarjetaMedio.CargarSaldo(2000);
            tarjetaNormal.CargarSaldo(2000);

            Colectivo colectivo = new Colectivo("123");

            // Act
            colectivo.PagarCon(tarjetaMedio);
            colectivo.PagarCon(tarjetaNormal);

            // Assert - Medio boleto debería gastar la mitad
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);   // 1210
            Assert.AreEqual(2000 - 1580, tarjetaNormal.Saldo); // 420
        }

        // Tests DIRECTOS al método PagarBoleto (para cobertura 100%)
        [Test]
        public void MedioBoleto_PagarBoletoDirecto_PagaMitad_Test()
        {
            // Arrange
            tarjetaMedio.CargarSaldo(2000);

            // Act - Llamar DIRECTAMENTE al método
            bool resultado = tarjetaMedio.PagarBoleto(1580);

            // Assert - Debería pagar la mitad: 1580 / 2 = 790
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_PagarBoletoDirecto_SaldoInsuficiente_Test()
        {
            // Arrange - Saldo inicial: 0

            // Act - Intentar pagar con saldo 0 (mitad = 790)
            bool resultado = tarjetaMedio.PagarBoleto(1580);
            resultado = tarjetaMedio.PagarBoleto(1580);

            // Assert - Debería fallar (790 > 0)
            Assert.IsFalse(resultado);
            Assert.AreEqual(-790, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_PagarBoletoDirecto_LimiteNegativo_Test()
        {
            // Arrange
            tarjetaMedio.CargarSaldo(2000);

            // Act - Pagar hasta cerca del límite negativo
            bool resultado1 = tarjetaMedio.PagarBoleto(1580); // 2000 - 790 = 1210
            bool resultado2 = tarjetaMedio.PagarBoleto(1580); // 1210 - 790 = 420
            bool resultado3 = tarjetaMedio.PagarBoleto(1580); // 420 - 790 = -370
            bool resultado4 = tarjetaMedio.PagarBoleto(1580); // -370 - 790 = -1160
            bool resultado5 = tarjetaMedio.PagarBoleto(1580); // -1160 - 790 = -1950 (supera -1200)

            // Assert - Último debería fallar
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.IsTrue(resultado4);
            Assert.IsFalse(resultado5);
            Assert.AreEqual(-1160, tarjetaMedio.Saldo);
        }
    }
}