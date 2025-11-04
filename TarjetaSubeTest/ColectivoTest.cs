using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class ColectivoTests
    {
        [Test]
        public void ObtenerTarifaBasicaDeLaLinea_Test()
        {
            Colectivo colectivo = new Colectivo("123");

            Assert.AreEqual(1580m, Colectivo.TARIFA_BASICA);
        }

        [Test]
        public void ObtenerTarifaInterurbana_Test()
        {
            Assert.AreEqual(3000m, Colectivo.TARIFA_INTERURBANA);
        }

        [Test]
        public void PagarViajeConSaldoSuficiente_Urbana_Test()
        {
            // Arrange
            Tarjeta tarjeta = new TarjetaNormal(11111);
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123"); // Línea urbana por defecto

            // Act
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 1580, tarjeta.Saldo);
        }

        [Test]
        public void PagarViajeConSaldoSuficiente_Interurbana_Test()
        {
            // Arrange
            Tarjeta tarjeta = new TarjetaNormal(11111);
            tarjeta.CargarSaldo(4000);
            Colectivo colectivo = new Colectivo("200", true); // Línea interurbana

            // Act
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(4000 - 3000, tarjeta.Saldo);
        }

        [Test]
        public void PagarViajeSinSaldoSuficiente_Urbana_Test()
        {
            Tarjeta tarjeta = new TarjetaNormal(22222);
            Colectivo colectivo = new Colectivo("123"); // Línea urbana

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void PagarViajeSinSaldoSuficiente_Interurbana_Test()
        {
            Tarjeta tarjeta = new TarjetaNormal(22222);
            Colectivo colectivo = new Colectivo("200", true); // Línea interurbana

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void Colectivo_DiferentesLineas_UrbanaEInterurbana_Test()
        {
            // Arrange
            Tarjeta tarjeta = new TarjetaNormal(33333);
            tarjeta.CargarSaldo(10000);
            Colectivo colectivoUrbano = new Colectivo("123");        // Urbana: 1580
            Colectivo colectivoInterurbano = new Colectivo("200", true); // Interurbana: 3000

            // Act
            colectivoUrbano.PagarCon(tarjeta);
            colectivoInterurbano.PagarCon(tarjeta);

            // Assert
            Assert.AreEqual(10000 - 1580 - 3000, tarjeta.Saldo);
        }

        [Test]
        public void Colectivo_MostrarTarifa_Urbana_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

            // Act & Assert - Solo verificar que no lance excepción
            Assert.DoesNotThrow(() => colectivo.MostrarTarifa());
        }

        [Test]
        public void Colectivo_MostrarTarifa_Interurbana_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("200", true);

            // Act & Assert - Solo verificar que no lance excepción
            Assert.DoesNotThrow(() => colectivo.MostrarTarifa());
        }

        [Test]
        public void Colectivo_ObtenerLinea_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

            // Act & Assert
            Assert.AreEqual("123", colectivo.ObtenerLinea());
        }

        [Test]
        public void Colectivo_TARIFA_BASICA_DeberiaSer1580()
        {
            // Act & Assert
            Assert.AreEqual(1580m, Colectivo.TARIFA_BASICA);
        }

        [Test]
        public void Colectivo_TARIFA_INTERURBANA_DeberiaSer3000()
        {
            // Act & Assert
            Assert.AreEqual(3000m, Colectivo.TARIFA_INTERURBANA);
        }

        [Test]
        public void Colectivo_EsInterurbana_LineaUrbana_Test()
        {
            // Arrange & Act
            var colectivo = new Colectivo("123");

            // Assert
            Assert.IsFalse(colectivo.EsInterurbana);
        }

        [Test]
        public void Colectivo_EsInterurbana_LineaInterurbana_Test()
        {
            // Arrange & Act
            var colectivo = new Colectivo("200", true);

            // Assert
            Assert.IsTrue(colectivo.EsInterurbana);
        }

        [Test]
        public void Colectivo_ObtenerTarifaActual_Urbana_Test()
        {
            // Arrange
            var colectivo = new Colectivo("123");

            // Act & Assert
            Assert.AreEqual(1580m, colectivo.ObtenerTarifaActual());
        }

        [Test]
        public void Colectivo_ObtenerTarifaActual_Interurbana_Test()
        {
            // Arrange
            var colectivo = new Colectivo("200", true);

            // Act & Assert
            Assert.AreEqual(3000m, colectivo.ObtenerTarifaActual());
        }

        [Test]
        public void Colectivo_Interurbana_ConMedioBoleto_Test()
        {
            // Arrange
            var tarjeta = new MedioBoleto(44444);
            tarjeta.CargarSaldo(4000);
            var colectivo = new Colectivo("200", true); // Interurbana: 3000

            // Act
            bool resultado = colectivo.PagarCon(tarjeta);

            // Assert - Medio boleto aplica a interurbana también (3000 / 2 = 1500)
            Assert.IsTrue(resultado);
            Assert.AreEqual(4000 - 1500, tarjeta.Saldo);
        }

        [Test]
        public void Colectivo_Interurbana_ConBoletoGratuito_Test()
        {
            // Arrange
            var tarjeta = new BoletoGratuito(55555);
            tarjeta.CargarSaldo(2000);
            var colectivo = new Colectivo("200", true); // Interurbana: 3000

            // Act
            bool resultado = colectivo.PagarCon(tarjeta);

            // Assert - Boleto gratuito no descuenta saldo incluso en interurbana
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void Colectivo_Constructor_DefaultEsUrbana_Test()
        {
            // Arrange & Act
            var colectivo = new Colectivo("123"); // Sin especificar esInterurbana

            // Assert
            Assert.IsFalse(colectivo.EsInterurbana);
            Assert.AreEqual(1580m, colectivo.ObtenerTarifaActual());
        }

        [Test]
        public void Colectivo_Constructor_ExplicitoUrbana_Test()
        {
            // Arrange & Act
            var colectivo = new Colectivo("123", false); // Explícitamente urbana

            // Assert
            Assert.IsFalse(colectivo.EsInterurbana);
            Assert.AreEqual(1580m, colectivo.ObtenerTarifaActual());
        }
    }
}