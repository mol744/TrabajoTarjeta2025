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
        public void PagarViajeConSaldoSuficiente_Test()
        {
            // Arrange
            Tarjeta tarjeta = new TarjetaNormal(11111);
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            // Act
            bool puedePagar = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsTrue(puedePagar);
            Assert.AreEqual(2000 - 1580, tarjeta.Saldo);
        }

        [Test]
        public void PagarViajeSinSaldoSuficiente_Test()
        {
            Tarjeta tarjeta = new TarjetaNormal(22222);
            Colectivo colectivo = new Colectivo("123");

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(0, tarjeta.Saldo);
        }
        [Test]
        public void Colectivo_DiferentesLineas_Test()
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
        public void Colectivo_MostrarTarifa_Test()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("123");

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

        // NUEVOS TESTS PARA TRASBORDOS
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
    }
}