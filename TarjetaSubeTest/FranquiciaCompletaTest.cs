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
        public void FranquiciaCompleta_MultiplesViajes_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

            // Act - Intentar pagar 5 viajes seguidos
            bool resultado1 = colectivo.PagarCon(tarjetaFranquicia);
            bool resultado2 = colectivo.PagarCon(tarjetaFranquicia);
            bool resultado3 = colectivo.PagarCon(tarjetaFranquicia);
            bool resultado4 = colectivo.PagarCon(tarjetaFranquicia);
            bool resultado5 = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Todos deberían funcionar
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.IsTrue(resultado4);
            Assert.IsTrue(resultado5);
            Assert.AreEqual(0, tarjetaFranquicia.Saldo); // Saldo sigue en 0
        }

        [Test]
        public void FranquiciaCompleta_NoDescuestaSaldo_Test()
        {
            // Arrange
            tarjetaFranquicia.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool resultado = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Saldo debería seguir en 2000 (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaFranquicia.Saldo);
        }

        // Tests DIRECTOS al método PagarBoleto (para cobertura 100%)
        [Test]
        public void FranquiciaCompleta_PagarBoletoDirecto_SiemprePermitePago_Test()
        {
            // Arrange - Saldo 0

            // Act - Llamar DIRECTAMENTE al método
            bool resultado = tarjetaFranquicia.PagarBoleto(1580);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(0, tarjetaFranquicia.Saldo); // No descuenta saldo
        }

        [Test]
        public void FranquiciaCompleta_PagarBoletoDirecto_MontosDiferentes_Test()
        {
            // Arrange
            tarjetaFranquicia.CargarSaldo(2000);

            // Act - Pagar con diferentes montos DIRECTAMENTE
            bool resultado1 = tarjetaFranquicia.PagarBoleto(1000);
            bool resultado2 = tarjetaFranquicia.PagarBoleto(2000);
            bool resultado3 = tarjetaFranquicia.PagarBoleto(500);

            // Assert - Todos deberían funcionar independientemente del monto
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(2000, tarjetaFranquicia.Saldo); // Saldo no cambia
        }
    }
}