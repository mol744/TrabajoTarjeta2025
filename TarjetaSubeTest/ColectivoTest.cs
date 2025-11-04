using TarjetaSube;
using NUnit.Framework;
using System;
using System.Threading;

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

        // =============================================
        // TESTS DE TRASBORDO GRATUITO (de la versión 1)
        // =============================================

        [Test]
        public void Colectivo_DiferentesLineas_UrbanaConTrasbordo_Test()
        {
            // Arrange
            Tarjeta tarjeta = new TarjetaNormal(33333);
            tarjeta.CargarSaldo(4000);
            Colectivo colectivo1 = new Colectivo("123");
            Colectivo colectivo2 = new Colectivo("153");

            // Act
            colectivo1.PagarCon(tarjeta);
            colectivo2.PagarCon(tarjeta);

            // Assert - SOLO SE COBRA EL PRIMER VIAJE (segundo es trasbordo gratuito)
            Assert.AreEqual(4000 - 1580, tarjeta.Saldo); // 2420 en lugar de 840
        }

        [Test]
        public void Trasbordo_Gratuito_EntreLineasDiferentes_DentroDe60Minutos_Test()
        {
            // Arrange
            TarjetaNormal tarjeta = new TarjetaNormal(77777);
            tarjeta.CargarSaldo(5000);

            Colectivo colectivo123 = new Colectivo("123");
            Colectivo colectivo153 = new Colectivo("153");

            // Act - Primer viaje
            bool primerViaje = colectivo123.PagarCon(tarjeta);
            decimal saldoDespuesPrimerViaje = tarjeta.Saldo;

            // Segundo viaje en línea diferente dentro de 60 minutos (debería ser trasbordo gratuito)
            bool segundoViaje = colectivo153.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(primerViaje, "Primer viaje debería ser exitoso");
            Assert.IsTrue(segundoViaje, "Segundo viaje debería ser exitoso con trasbordo");
            Assert.AreEqual(saldoDespuesPrimerViaje, tarjeta.Saldo, "No debería cobrar en trasbordo gratuito");
        }

        [Test]
        public void No_Trasbordo_MismaLinea_Test()
        {
            // Arrange
            TarjetaNormal tarjeta = new TarjetaNormal(88888);
            tarjeta.CargarSaldo(5000);

            Colectivo colectivo123 = new Colectivo("123");

            // Act - Dos viajes misma línea
            bool primerViaje = colectivo123.PagarCon(tarjeta);
            decimal saldoDespuesPrimerViaje = tarjeta.Saldo;

            bool segundoViaje = colectivo123.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(primerViaje, "Primer viaje debería ser exitoso");
            Assert.IsTrue(segundoViaje, "Segundo viaje debería ser exitoso");
            Assert.AreEqual(saldoDespuesPrimerViaje - 1580, tarjeta.Saldo, "Debería cobrar tarifa normal en misma línea");
        }

        [Test]
        public void No_Trasbordo_SinViajePrevio_Test()
        {
            // Arrange
            TarjetaNormal tarjeta = new TarjetaNormal(99999);
            tarjeta.CargarSaldo(2000);

            Colectivo colectivo = new Colectivo("123");

            // Act - Primer viaje (no debería ser trasbordo)
            bool resultado = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(resultado, "Debería poder pagar primer viaje");
            Assert.AreEqual(2000 - 1580, tarjeta.Saldo, "Debería cobrar tarifa normal sin viaje previo");
        }

        [Test]
        public void Trasbordo_Funciona_ConDiferentesTiposTarjeta_Test()
        {
            // Arrange - Probar solo con tarjetas que no tienen restricción de tiempo
            TarjetaNormal tarjetaNormal = new TarjetaNormal(10001);
            BoletoGratuito tarjetaGratuita = new BoletoGratuito(10003);

            tarjetaNormal.CargarSaldo(5000);
            tarjetaGratuita.CargarSaldo(5000);

            Colectivo colectivo123 = new Colectivo("123");
            Colectivo colectivo153 = new Colectivo("153");

            // Act & Assert - TarjetaNormal y BoletoGratuito permiten trasbordo
            colectivo123.PagarCon(tarjetaNormal);
            decimal saldoNormal = tarjetaNormal.Saldo;
            bool trasbordoNormal = colectivo153.PagarCon(tarjetaNormal);
            Assert.IsTrue(trasbordoNormal, "TarjetaNormal debería permitir trasbordo");
            Assert.AreEqual(saldoNormal, tarjetaNormal.Saldo, "TarjetaNormal no debería cobrar en trasbordo");

            colectivo123.PagarCon(tarjetaGratuita);
            decimal saldoGratuito = tarjetaGratuita.Saldo;
            bool trasbordoGratuito = colectivo153.PagarCon(tarjetaGratuita);
            Assert.IsTrue(trasbordoGratuito, "BoletoGratuito debería permitir trasbordo");
            Assert.AreEqual(saldoGratuito, tarjetaGratuita.Saldo, "BoletoGratuito no debería cobrar en trasbordo");
        }

        [Test]
        public void Multiples_Trasbordos_Consecutivos_Test()
        {
            // Arrange
            TarjetaNormal tarjeta = new TarjetaNormal(11112);
            tarjeta.CargarSaldo(10000);

            Colectivo linea1 = new Colectivo("123");
            Colectivo linea2 = new Colectivo("153");
            Colectivo linea3 = new Colectivo("76");

            // Act - Tres viajes en líneas diferentes consecutivas
            bool viaje1 = linea1.PagarCon(tarjeta); // Paga normal
            decimal saldoDespuesViaje1 = tarjeta.Saldo;

            bool viaje2 = linea2.PagarCon(tarjeta); // Trasbordo gratuito
            bool viaje3 = linea3.PagarCon(tarjeta); // Trasbordo gratuito

            // Assert
            Assert.IsTrue(viaje1, "Primer viaje debería ser exitoso");
            Assert.IsTrue(viaje2, "Segundo viaje debería ser trasbordo gratuito");
            Assert.IsTrue(viaje3, "Tercer viaje debería ser trasbordo gratuito");
            Assert.AreEqual(saldoDespuesViaje1, tarjeta.Saldo, "Solo debería cobrarse el primer viaje");
        }

        [Test]
        public void No_Trasbordo_Interurbana_Test()
        {
            // Arrange
            TarjetaNormal tarjeta = new TarjetaNormal(12121);
            tarjeta.CargarSaldo(10000);

            Colectivo urbano = new Colectivo("123");
            Colectivo interurbano = new Colectivo("200", true);

            // Act - Viaje urbano seguido de interurbano
            urbano.PagarCon(tarjeta);
            decimal saldoDespuesUrbano = tarjeta.Saldo;
            interurbano.PagarCon(tarjeta);

            // Assert - No debería haber trasbordo entre urbano e interurbano
            Assert.AreEqual(saldoDespuesUrbano - 3000, tarjeta.Saldo, "Debería cobrar tarifa interurbana completa");
        }
    }
}