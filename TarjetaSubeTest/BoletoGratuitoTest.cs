using TarjetaSube;
using NUnit.Framework;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class BoletoGratuitoTests
    {
        private BoletoGratuito tarjetaGratuita;
        private DateTime _tiempoSimulado;

        [SetUp]
        public void Setup()
        {
            tarjetaGratuita = new BoletoGratuito(55555);

            // Configurar tiempo simulado para testing - Lunes 10:00 (dentro de franja)
            _tiempoSimulado = new DateTime(2025, 1, 6, 10, 0, 0); // Lunes
            tarjetaGratuita.ObtenerFechaActual = () => _tiempoSimulado;
        }

        [Test]
        public void BoletoGratuito_NoDescuentaSaldo_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool resultado = colectivo.PagarCon(tarjetaGratuita);

            // Assert - Saldo debería seguir en 2000 (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo);
        }

        [Test]
        public void BoletoGratuito_ObtenerTipoTarjeta_Test()
        {
            // Act & Assert
            Assert.AreEqual("BoletoGratuito", tarjetaGratuita.ObtenerTipoTarjeta());
        }

        [Test]
        public void BoletoGratuito_ObtenerTarifa_DentroFranja_PrimerosDosViajes_Test()
        {
            // Arrange - Dentro de franja horaria
            _tiempoSimulado = new DateTime(2025, 1, 6, 12, 0, 0); // Lunes 12:00

            // Act & Assert - Primeros 2 viajes gratuitos
            Assert.AreEqual(0, tarjetaGratuita.ObtenerTarifa(1580m));

            // Hacer primer viaje
            tarjetaGratuita.PagarBoleto(1580m);
            Assert.AreEqual(0, tarjetaGratuita.ObtenerTarifa(1580m));

            // Hacer segundo viaje
            _tiempoSimulado = _tiempoSimulado.AddMinutes(10);
            tarjetaGratuita.PagarBoleto(1580m);
            Assert.AreEqual(1580m, tarjetaGratuita.ObtenerTarifa(1580m));
        }

        [Test]
        public void BoletoGratuito_ObtenerTarifa_DentroFranja_TercerViaje_Test()
        {
            // Arrange - Dentro de franja horaria
            _tiempoSimulado = new DateTime(2025, 1, 6, 12, 0, 0); // Lunes 12:00

            // Hacer primeros 2 viajes gratuitos
            tarjetaGratuita.PagarBoleto(1580m);
            _tiempoSimulado = _tiempoSimulado.AddMinutes(10);
            tarjetaGratuita.PagarBoleto(1580m);

            // Act & Assert - Tercer viaje debería ser tarifa completa
            Assert.AreEqual(1580m, tarjetaGratuita.ObtenerTarifa(1580m));
        }

        [Test]
        public void BoletoGratuito_ObtenerTarifa_FueraFranja_Test()
        {
            // Arrange - Fuera de franja horaria (sábado)
            _tiempoSimulado = new DateTime(2025, 1, 4, 12, 0, 0); // Sábado 12:00
            decimal tarifaCompleta = 1580m;

            // Act & Assert - Debería devolver tarifa completa fuera de franja
            Assert.AreEqual(tarifaCompleta, tarjetaGratuita.ObtenerTarifa(tarifaCompleta));
        }

        [Test]
        public void BoletoGratuito_ConsultarSaldoYID_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);

            // Act & Assert
            Assert.AreEqual(2000, tarjetaGratuita.ConsultarSaldo());
            Assert.AreEqual(55555, tarjetaGratuita.ConsultarID());
        }

        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_SiemprePermitePago_Test()
        {
            // Arrange
            tarjetaGratuita.CargarSaldo(2000);

            // Act - Llamar DIRECTAMENTE al método
            bool resultado = tarjetaGratuita.PagarBoleto(1580);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjetaGratuita.Saldo); // No descuenta saldo
        }

        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_MultiplesVeces_Test()
        {
            // Resetear viajes para asegurar estado limpio
            tarjetaGratuita.ResetearViajes();

            // Arrange
            tarjetaGratuita.CargarSaldo(5000); // Más saldo para cubrir viajes adicionales

            // Act - Pagar múltiples veces DIRECTAMENTE
            bool resultado1 = tarjetaGratuita.PagarBoleto(1580);
            bool resultado2 = tarjetaGratuita.PagarBoleto(1580);
            bool resultado3 = tarjetaGratuita.PagarBoleto(1580);

            // Assert - Primeros 2 gratuitos, tercero con tarifa
            Assert.IsTrue(resultado1);
            Assert.IsTrue(resultado2);
            Assert.IsTrue(resultado3);
            Assert.AreEqual(5000 - 1580, tarjetaGratuita.Saldo); // Solo el tercero descuenta
        }

        [Test]
        public void BoletoGratuito_PagarBoletoDirecto_SinSaldo_Test()
        {
            // Arrange - Saldo 0

            // Act - Llamar DIRECTAMENTE al método sin saldo
            bool resultado = tarjetaGratuita.PagarBoleto(1580);

            // Assert - Debería permitir el pago (gratuito)
            Assert.IsTrue(resultado);
            Assert.AreEqual(0, tarjetaGratuita.Saldo);
        }

        [Test]
        public void BoletoGratuito_NoPuedePagar_FueraFranjaHoraria_Test()
        {
            // Arrange - Fuera de franja horaria (domingo)
            _tiempoSimulado = new DateTime(2025, 1, 5, 14, 0, 0); // Domingo 14:00
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaGratuita);

            // Assert - No debería poder pagar fuera de franja
            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void BoletoGratuito_PuedePagar_DentroFranjaHoraria_Test()
        {
            // Arrange - Dentro de franja horaria (viernes 15:00)
            _tiempoSimulado = new DateTime(2025, 1, 10, 15, 0, 0); // Viernes 15:00
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaGratuita);

            // Assert - Debería poder pagar dentro de franja
            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void BoletoGratuito_EstaEnFranjaHorariaPermitida_ValidaCorrectamente_Test()
        {
            // Test directo del método de validación

            // Casos válidos
            Assert.IsTrue(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 6, 10, 0, 0))); // Lunes 10:00
            Assert.IsTrue(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 7, 6, 0, 0)));   // Martes 6:00
            Assert.IsTrue(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 8, 21, 59, 0))); // Miércoles 21:59

            // Casos inválidos
            Assert.IsFalse(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 4, 12, 0, 0))); // Sábado 12:00
            Assert.IsFalse(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 5, 12, 0, 0))); // Domingo 12:00
            Assert.IsFalse(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 6, 5, 59, 0))); // Lunes 5:59
            Assert.IsFalse(tarjetaGratuita.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 7, 22, 0, 0))); // Martes 22:00
        }

        [Test]
        public void BoletoGratuito_TercerViaje_RequiereSaldo_Test()
        {
            // Arrange - Dentro de franja horaria
            _tiempoSimulado = new DateTime(2025, 1, 6, 12, 0, 0); // Lunes 12:00
            tarjetaGratuita.CargarSaldo(2000); // Saldo insuficiente para tercer viaje

            // Hacer primeros 2 viajes gratuitos
            tarjetaGratuita.PagarBoleto(1580m);
            _tiempoSimulado = _tiempoSimulado.AddMinutes(10);
            tarjetaGratuita.PagarBoleto(1580m);

            // Tercer viaje - debería requerir saldo
            _tiempoSimulado = _tiempoSimulado.AddMinutes(10);
            tarjetaGratuita.PagarBoleto(1580m);

            Assert.AreEqual(420m, tarjetaGratuita.Saldo);
            tarjetaGratuita.PagarBoleto(1580m);

            _tiempoSimulado = _tiempoSimulado.AddMinutes(10);

            // Assert - El saldo está al límite
            Assert.AreEqual(-1160, tarjetaGratuita.Saldo);

            // Intentar quinto viaje (tercero con saldo necesario) - debería fallar por saldo insuficiente
            bool resultado = tarjetaGratuita.PagarBoleto(1580m);

            Assert.IsFalse(resultado);
            Assert.AreEqual(-1160, tarjetaGratuita.Saldo); // Saldo no cambia
        }
    }
}