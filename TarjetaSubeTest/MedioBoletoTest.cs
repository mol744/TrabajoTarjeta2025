using NUnit.Framework;
using System;
using System.Collections.Generic;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class MedioBoletoLimitacionTests
    {
        private MedioBoleto tarjetaMedio;
        private DateTime _tiempoSimulado;
        private List<DateTime> _tiemposLlamadas;

        [SetUp]
        public void Setup()
        {
            tarjetaMedio = new MedioBoleto(77777);
            tarjetaMedio.CargarSaldo(10000);
            tarjetaMedio.ResetearViajes();

            // Configurar tiempo simulado para testing
            _tiempoSimulado = new DateTime(2025, 1, 1, 10, 0, 0);
            _tiemposLlamadas = new List<DateTime>();

            tarjetaMedio.ObtenerFechaActual = () => {
                _tiemposLlamadas.Add(_tiempoSimulado);
                return _tiempoSimulado;
            };
        }

        [Test]
        public void MedioBoleto_NoPermiteDosViajesEnMenosDe5Minutos_Test()
        {
            // Arrange
            decimal tarifa = 1580m;

            // Act - Primer viaje a las 10:00
            bool primerViaje = tarjetaMedio.PagarBoleto(tarifa);

            // Intentar segundo viaje inmediatamente (mismo tiempo)
            bool segundoViaje = tarjetaMedio.PagarBoleto(tarifa);

            // Assert
            Assert.IsTrue(primerViaje, "El primer viaje debería ser exitoso");
            Assert.IsFalse(segundoViaje, "El segundo viaje debería fallar por tiempo insuficiente");
            Assert.AreEqual(1, tarjetaMedio.CantidadViajesHoy());
        }

        [Test]
        public void MedioBoleto_PermiteDosViajesDespuesDe5Minutos_Test()
        {
            // Arrange
            decimal tarifa = 1580m;
            decimal saldoInicial = tarjetaMedio.Saldo;

            // Act - Primer viaje a las 10:00
            bool primerViaje = tarjetaMedio.PagarBoleto(tarifa);

            // Avanzar 6 minutos
            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);

            // Segundo viaje a las 10:06 (después de 5 minutos)
            bool segundoViaje = tarjetaMedio.PagarBoleto(tarifa);

            // Assert
            Assert.IsTrue(primerViaje, "El primer viaje debería ser exitoso");
            Assert.IsTrue(segundoViaje, "El segundo viaje debería ser exitoso después de 5 minutos");
            Assert.AreEqual(2, tarjetaMedio.CantidadViajesHoy());

            // Verificar que se cobró medio boleto en ambos (790 * 2 = 1580)
            decimal tarifaMedio = tarifa / 2;
            Assert.AreEqual(saldoInicial - (tarifaMedio * 2), tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_TercerViajeDelDia_TarifaCompleta_Test()
        {
            // Arrange
            decimal tarifa = 1580m;
            decimal saldoInicial = tarjetaMedio.Saldo;

            // Act - Realizar 3 viajes con intervalos de 6 minutos
            bool primerViaje = tarjetaMedio.PagarBoleto(tarifa); // 10:00

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            bool segundoViaje = tarjetaMedio.PagarBoleto(tarifa); // 10:06

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            bool tercerViaje = tarjetaMedio.PagarBoleto(tarifa); // 10:12

            // Assert
            Assert.IsTrue(primerViaje, "Primer viaje debería ser exitoso");
            Assert.IsTrue(segundoViaje, "Segundo viaje debería ser exitoso");
            Assert.IsTrue(tercerViaje, "Tercer viaje debería ser exitoso pero con tarifa completa");
            Assert.AreEqual(3, tarjetaMedio.CantidadViajesHoy());

            // Verificar que los primeros 2 son medio boleto y el tercero es completo
            decimal tarifaMedio = tarifa / 2;
            decimal totalEsperado = (tarifaMedio * 2) + tarifa; // 790*2 + 1580 = 3160
            Assert.AreEqual(saldoInicial - totalEsperado, tarjetaMedio.Saldo);
        }

        [Test]
        public void MedioBoleto_NoPermiteMasDeDosViajesConTarifaReducida_Test()
        {
            // Arrange
            decimal tarifa = 1580m;
            decimal tarifaMedio = tarifa / 2;

            // Act - Realizar 2 viajes con tarifa reducida + 1 con tarifa completa
            tarjetaMedio.PagarBoleto(tarifa); // Viaje 1 - medio boleto (10:00)

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            tarjetaMedio.PagarBoleto(tarifa); // Viaje 2 - medio boleto (10:06)

            decimal saldoDespuesDeDosViajes = tarjetaMedio.Saldo;

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            tarjetaMedio.PagarBoleto(tarifa); // Viaje 3 - tarifa completa (10:12)

            // Assert
            decimal diferenciaTercerViaje = saldoDespuesDeDosViajes - tarjetaMedio.Saldo;
            Assert.AreEqual(tarifa, diferenciaTercerViaje, "El tercer viaje debería cobrar tarifa completa");
        }

        [Test]
        public void MedioBoleto_ReiniciaContadorAlDiaSiguiente_Test()
        {
            // Arrange
            decimal tarifa = 1580m;

            // Act - Realizar 2 viajes "hoy"
            tarjetaMedio.PagarBoleto(tarifa); // 10:00

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            tarjetaMedio.PagarBoleto(tarifa); // 10:06

            int viajesHoy = tarjetaMedio.CantidadViajesHoy();

            // Simular nuevo día (avanzar 24 horas)
            _tiempoSimulado = _tiempoSimulado.AddDays(1);

            // Hacer otro viaje - debería contar como primer viaje del nuevo día
            tarjetaMedio.PagarBoleto(tarifa);

            int viajesNuevoDia = tarjetaMedio.CantidadViajesHoy();

            // Assert
            Assert.AreEqual(1, viajesNuevoDia, "Debería tener 1 viaje en el nuevo día");
        }

        [Test]
        public void MedioBoleto_ObtenerTarifa_CalculaCorrectamente()
        {   
            tarjetaMedio.ResetearViajes();
            // Primeros 2 viajes - tarifa media
            Assert.AreEqual(790m, tarjetaMedio.ObtenerTarifa(1580m));

            tarjetaMedio.PagarBoleto(1580m); // Primer viaje
            Assert.AreEqual(790m, tarjetaMedio.ObtenerTarifa(1580m));

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            tarjetaMedio.PagarBoleto(1580m); // Segundo viaje
            Assert.AreEqual(790m, tarjetaMedio.ObtenerTarifa(1580m));

            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            tarjetaMedio.PagarBoleto(1580m); // Tercer viaje (tarifa completa)
            Assert.AreEqual(1580m, tarjetaMedio.ObtenerTarifa(1580m));
        }

        [Test]
        public void MedioBoleto_RespetaLimiteNegativo_Test()
        {
            tarjetaMedio.ResetearViajes();
            // Arrange
            tarjetaMedio.CargarSaldo(2000); // Saldo bajo

            // Act - Intentar pagar medio boleto (790) con saldo 2000
            bool resultado = tarjetaMedio.PagarBoleto(1580m);
            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            resultado = tarjetaMedio.PagarBoleto(1580m);
            _tiempoSimulado = _tiempoSimulado.AddMinutes(6);
            resultado = tarjetaMedio.PagarBoleto(1580m);

            // Assert
            Assert.IsTrue(resultado, "");
            //Assert.AreEqual(500 - 790, tarjetaMedio.Saldo); // -290
        }

        [Test]
        public void MedioBoleto_TiempoDesdeUltimoViaje_CalculaCorrectamente()
        {
            // Act & Assert - Sin viajes
            Assert.IsNull(tarjetaMedio.TiempoDesdeUltimoViaje());

            // Primer viaje
            tarjetaMedio.PagarBoleto(1580m);

            // Avanzar 3 minutos
            _tiempoSimulado = _tiempoSimulado.AddMinutes(3);

            // Verificar tiempo transcurrido
            TimeSpan? tiempo = tarjetaMedio.TiempoDesdeUltimoViaje();
            Assert.IsNotNull(tiempo);
            Assert.AreEqual(3, tiempo.Value.TotalMinutes, 0.1);
        }
    }
}