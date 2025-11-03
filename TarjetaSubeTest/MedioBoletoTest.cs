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
        public void MedioBoleto_PagarBoletoDirecto_ConTarifaDividida_Test()
        {
            // Arrange
            tarjetaMedio.CargarSaldo(2000);
            decimal tarifaMedio = 1580 / 2; // 790

            // Act - Llamar DIRECTAMENTE al método con tarifa YA DIVIDIDA
            bool resultado = tarjetaMedio.PagarBoleto(tarifaMedio);

            // Assert - Debería pagar 790
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_ObtenerTipoTarjeta_Test()
        {
            // Arrange - Saldo inicial: 0
            decimal tarifaMedio = 1580 / 2; // 790

            // Act - Intentar pagar con saldo 0
            bool resultado = tarjetaMedio.PagarBoleto(tarifaMedio);

            // Assert - Debería PERMITIR el pago porque llega hasta -790 (dentro del límite -1200)
            Assert.IsTrue(resultado); // Cambiar de False a True
            Assert.AreEqual(-790, tarjetaMedio.Saldo); // Queda en -790
        }

        [Test]
        public void MedioBoleto_ConsultarSaldoYID_Test()
        {
            tarjetaMedio.CargarSaldo(2000);
            decimal tarifaMedio = 1580 / 2; // 790

            // Act - Pagar hasta cerca del límite negativo
            bool resultado1 = tarjetaMedio.PagarBoleto(tarifaMedio); // 2000 - 790 = 1210
            bool resultado2 = tarjetaMedio.PagarBoleto(tarifaMedio); // 1210 - 790 = 420
            bool resultado3 = tarjetaMedio.PagarBoleto(tarifaMedio); // 420 - 790 = -370
            bool resultado4 = tarjetaMedio.PagarBoleto(tarifaMedio); // -370 - 790 = -1160
            bool resultado5 = tarjetaMedio.PagarBoleto(tarifaMedio); // -1160 - 790 = -1950 (supera -1200)

            // Assert - Último debería fallar
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.IsTrue(resultado4);
            Assert.IsFalse(resultado5);
            Assert.AreEqual(-1160, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_PagarBoletoDirecto_ConTarifaCompleta_Test()
        {
            // Arrange
            tarjetaMedio.CargarSaldo(2000);
            decimal tarifaCompleta = 1580;

            // Act - Llamar DIRECTAMENTE al método con tarifa COMPLETA
            bool resultado = tarjetaMedio.PagarBoleto(tarifaCompleta);

            // Assert - Debería pagar 1580 (no divide la tarifa)
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 1580, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_PagarBoletoDirecto_MultiplesTarifas_Test()
        {
            // Arrange
            tarjetaMedio.CargarSaldo(5000);

            // Act - Pagar con diferentes tarifas
            bool resultado1 = tarjetaMedio.PagarBoleto(500);  // Tarifa baja
            bool resultado2 = tarjetaMedio.PagarBoleto(1000); // Tarifa media  
            bool resultado3 = tarjetaMedio.PagarBoleto(2000); // Tarifa alta

            // Assert - Todas deberían funcionar
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(5000 - 500 - 1000 - 2000, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_PagarBoletoDirecto_ExactamenteEnLimite_Test()
        {
            // Arrange - Usar monto permitido
            tarjetaMedio.CargarSaldo(2000); // Cambiar 1200 por 2000 (monto permitido)
            decimal tarifaMedio = 1580 / 2; // 790

            // Act - Pagar hasta quedar exactamente en el límite negativo
            bool resultado1 = tarjetaMedio.PagarBoleto(tarifaMedio); // 2000 - 790 = 1210
            bool resultado2 = tarjetaMedio.PagarBoleto(tarifaMedio); // 1210 - 790 = 420
            bool resultado3 = tarjetaMedio.PagarBoleto(tarifaMedio); // 420 - 790 = -370
            bool resultado4 = tarjetaMedio.PagarBoleto(tarifaMedio); // -370 - 790 = -1160
            bool resultado5 = tarjetaMedio.PagarBoleto(tarifaMedio); // -1160 - 790 = -1950 (supera -1200)

            // Assert
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.IsTrue(resultado4);
            Assert.IsFalse(resultado5); // Este debería fallar
            Assert.AreEqual(-1160, tarjetaMedio.Saldo);
        }
    }
}