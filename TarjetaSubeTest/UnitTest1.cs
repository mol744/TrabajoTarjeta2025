using NUnit.Framework;
using System;
using TarjetaSube;

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

        // ####################################
        // ########### TESTEO MAIN ############
        // ####################################

        [Test]
        public void Program_Main_ExecuteWithoutErrors_Test()
        {
            // Este test asegura que el Program.cs se puede ejecutar sin errores
            Program.Main(Array.Empty<string>());
            Assert.IsTrue(true); // Si llega aquí, no hubo excepciones
        }

        // ###############################################
        // ########### TESTEO TARJETA GENERAL ############
        // ###############################################

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

        [Test]
        public void PagarConSaldoNegativo_DentroDelLimite_Test()
        {
            // Arrange - Cargamos $2000 (monto permitido) y gastamos hasta acercarnos al límite
            tarjeta.CargarSaldo(2000); // Saldo: 2000
            Colectivo colectivo = new Colectivo("123");

            // Primer viaje: 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta);
            // Segundo viaje: 420 - 1580 = -1160 (dentro del límite -1200)
            colectivo.PagarCon(tarjeta);

            // Act - Intentamos tercer viaje: -1160 - 1580 = -2740 (FUERA del límite -1200)
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert - No debería permitir este tercer pago
            Assert.IsFalse(puedePagar);
            Assert.AreEqual(-1160, tarjeta.Saldo); // Se quedó en -1160, no en -2740
        }


        [Test]
        public void PagarConSaldoNegativo_FueraDelLimite_Test()
        {
            // Arrange - tarjeta con saldo -1000 (cerca del límite)
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Primero pagamos dos viajes para llegar cerca del límite
            colectivo.PagarCon(tarjeta); // Saldo: 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta); // Saldo: 420 - 1580 = -1160

            // Act - intentamos un tercer viaje que superaría el límite -1200
            bool puedePagar = colectivo.PagarCon(tarjeta); // -1160 - 1580 = -2740 (supera -1200)

            // Assert - no debería permitir el pago
            Assert.IsFalse(puedePagar);
            Assert.AreEqual(-1160, tarjeta.Saldo); // El saldo no cambió
        }

        [Test]
        public void CargarSaldo_ConSaldoNegativo_Test()
        {
            // Arrange - tarjeta con saldo negativo DENTRO del límite
            tarjeta.CargarSaldo(2000); // Saldo: 2000
            Colectivo colectivo = new Colectivo("123");
            colectivo.PagarCon(tarjeta); // Saldo: 2000 - 1580 = 420
            colectivo.PagarCon(tarjeta); // Saldo: 420 - 1580 = -1160

            // Act - cargamos saldo
            bool cargaExitosa = tarjeta.CargarSaldo(2000);

            // Assert - la carga paga la deuda: -1160 + 2000 = 840
            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(840, tarjeta.Saldo);
        }

        // ###############################################
        // ########### TESTEO FRANQ. COMPLETA ############
        // ###############################################

        [Test]
        public void FranquiciaCompleta_SiemprePuedePagar_Test()
        {
            // Arrange
            FranquiciaCompleta tarjetaFranquicia = new FranquiciaCompleta(99999);
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
            FranquiciaCompleta tarjetaFranquicia = new FranquiciaCompleta(99999);
            Colectivo colectivo = new Colectivo("123");

            // Act - Intentar pagar 5 viajes seguidos
            bool resultado1 = colectivo.PagarCon(tarjetaFranquicia);
            bool resultado2 = colectivo.PagarCon(tarjetaFranquicia);
            bool resultado3 = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Todos deberían funcionar
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(0, tarjetaFranquicia.Saldo); // Saldo sigue en 0
        }

        [Test]
        public void FranquiciaCompleta_NoDescuestaSaldo_Test()
        {
            // Arrange
            FranquiciaCompleta tarjetaGratuita = new FranquiciaCompleta(55555);
            tarjetaGratuita.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool resultado = colectivo.PagarCon(tarjetaGratuita);

            // Assert - Saldo debería seguir en 2000 (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo);
        }

        [Test]
        // ############################################
        // ############ TESTEO MEDIO BOLETO ###########
        // ############################################

        public void MedioBoleto_PagaMitad_Test()
        {
            // Arrange
            MedioBoleto tarjetaMedio = new MedioBoleto(88888);
            tarjetaMedio.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaMedio);

            // Assert - Debería pagar la mitad: 1580 / 2 = 790
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo); // 2000 - 790 = 1210
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

        [Test]
        public void MedioBoleto_PagaExactamenteLaMitad_Test()
        {
            // Arrange
            MedioBoleto tarjetaMedio = new MedioBoleto(88888);
            tarjetaMedio.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool resultado = colectivo.PagarCon(tarjetaMedio);

            // Assert - Debería pagar 1580 / 2 = 790
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000 - 790, tarjetaMedio.Saldo);
        }

        [Test]

        // ############################################
        // ########### BOLETO EST. GRATUITO ###########
        // ############################################
        public void BoletoGratuito_SiemprePuedePagar_Test()
        {
            // Arrange
            BoletoGratuito tarjetaGratuita = new BoletoGratuito(55555);
            Colectivo colectivo = new Colectivo("123");

            // Act - Intentar pagar con saldo 0
            bool puedePagar = colectivo.PagarCon(tarjetaGratuita);

            // Assert - Debería poder pagar siempre (gratuito)
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(0, tarjetaGratuita.Saldo); // El saldo no cambia
        }

        [Test]
        public void BoletoGratuito_NoDescuestaSaldo_Test()
        {
            // Arrange
            BoletoGratuito tarjetaGratuita = new BoletoGratuito(55555);
            tarjetaGratuita.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool resultado = colectivo.PagarCon(tarjetaGratuita);

            // Assert - Saldo debería seguir en 2000 (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo);
        }
    }
}