using TarjetaSube;
using NUnit.Framework;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class FranquiciaCompletaTests
    {
        private FranquiciaCompleta tarjetaFranquicia;
        private DateTime _tiempoSimulado;

        [SetUp]
        public void Setup()
        {
            tarjetaFranquicia = new FranquiciaCompleta(99999);
            _tiempoSimulado = new DateTime(2025, 1, 6, 12, 0, 0); // Lunes 12:00 (dentro de franja )

            tarjetaFranquicia.ObtenerFechaActual = () => _tiempoSimulado;
        }

        [Test]
        public void FranquiciaCompleta_PuedePagar_DentroFranjaHoraria_Test()
        {
            // Arrange - Lunes 15:00 (dentro de franja)
            _tiempoSimulado = new DateTime(2025, 1, 6, 15, 0, 0);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Debería poder pagar
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(0, tarjetaFranquicia.Saldo); // El saldo no cambia
        }

        [Test]
        public void FranquiciaCompleta_NoPuedePagar_FueraFranjaHoraria_FinDeSemana_Test()
        {
            // Arrange - Sábado 12:00 (fuera de franja)
            _tiempoSimulado = new DateTime(2025, 1, 4, 12, 0, 0); // Sábado
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - No debería poder pagar
            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_NoPuedePagar_FueraFranjaHoraria_Noche_Test()
        {
            // Arrange - Lunes 23:00 (fuera de franja)
            _tiempoSimulado = new DateTime(2025, 1, 6, 23, 0, 0);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - No debería poder pagar
            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_NoPuedePagar_FueraFranjaHoraria_Madrugada_Test()
        {
            // Arrange - Martes 5:00 (fuera de franja)
            _tiempoSimulado = new DateTime(2025, 1, 7, 5, 0, 0);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - No debería poder pagar
            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_PuedePagar_BordeInferiorFranja_Test()
        {
            // Arrange - Miércoles 6:00 (borde inferior - dentro de franja)
            _tiempoSimulado = new DateTime(2025, 1, 8, 6, 0, 0);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Debería poder pagar
            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_NoPuedePagar_BordeSuperiorFranja_Test()
        {
            // Arrange - Jueves 22:00 (borde superior - fuera de franja)
            _tiempoSimulado = new DateTime(2025, 1, 9, 22, 0, 0);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - No debería poder pagar
            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_PuedePagar_BordeSuperiorMenosUnMinuto_Test()
        {
            // Arrange - Viernes 21:59 (dentro de franja)
            _tiempoSimulado = new DateTime(2025, 1, 10, 21, 59, 0);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjetaFranquicia);

            // Assert - Debería poder pagar
            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_ObtenerTipoTarjeta_Test()
        {
            // Act & Assert
            Assert.AreEqual("FranquiciaCompleta", tarjetaFranquicia.ObtenerTipoTarjeta());
        }

        [Test]
        public void ObtenerTarifa_FranquiciaCompleta_Test()
        {
            // Act & Assert
            Assert.AreEqual(0, tarjetaFranquicia.ObtenerTarifa(1580m));
        }

        [Test]
        public void FranquiciaCompleta_ConsultarSaldoYID_Test()
        {
            tarjetaFranquicia.CargarSaldo(3000);

            // Act & Assert
            Assert.AreEqual(3000, tarjetaFranquicia.ConsultarSaldo());
            Assert.AreEqual(99999, tarjetaFranquicia.ConsultarID());
        }

        [Test]
        public void FranquiciaCompleta_EstaEnFranjaHorariaPermitida_ValidaCorrectamente_Test()
        {
            // Test directo del método de validación

            // Casos válidos
            Assert.IsTrue(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 6, 10, 0, 0))); // Lunes 10:00
            Assert.IsTrue(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 7, 6, 0, 0)));   // Martes 6:00
            Assert.IsTrue(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 8, 21, 59, 0))); // Miércoles 21:59

            // Casos inválidos
            Assert.IsFalse(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 4, 12, 0, 0))); // Sábado 12:00
            Assert.IsFalse(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 5, 12, 0, 0))); // Domingo 12:00
            Assert.IsFalse(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 6, 5, 59, 0))); // Lunes 5:59
            Assert.IsFalse(tarjetaFranquicia.EstaEnFranjaHorariaPermitida(new DateTime(2025, 1, 7, 22, 0, 0))); // Martes 22:00
        }
    }
}