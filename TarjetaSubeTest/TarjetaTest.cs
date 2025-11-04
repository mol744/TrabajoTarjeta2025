using TarjetaSube;
using NUnit.Framework;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TarjetaTests
    {
        private TarjetaNormal tarjeta;
        private DateTime _tiempoSimulado;

        [SetUp]
        public void Setup()
        {
            tarjeta = new TarjetaNormal(12345);
            _tiempoSimulado = new DateTime(2025, 11, 1, 0, 0, 0);

            // Configurar tiempo simulado para testing
            tarjeta.ObtenerFechaActual = () => _tiempoSimulado;
            tarjeta.ResetearContadorViajes();
        }

        // Tests existentes (se mantienen igual)
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
            Assert.IsFalse(resultado);
        }

        [Test]
        public void PagarConSaldoNegativo_DentroDelLimite_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool puedePagar = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(puedePagar);
            Assert.AreEqual(-1160, tarjeta.Saldo);
        }

        [Test]
        public void CargarSaldo_ConSaldoNegativo_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool cargaExitosa = tarjeta.CargarSaldo(2000);

            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(840, tarjeta.Saldo);
        }

        [Test]
        public void CargarSaldo_SuperaLimiteMaximo_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            bool resultado = tarjeta.CargarSaldo(10000);

            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(14000, tarjeta.Acargar);
        }

        [Test]
        public void PagarBoleto_QueSupereLimiteNegativo_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(2000);
            Colectivo colectivo = new Colectivo("123");

            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);

            bool resultado = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(resultado);
        }

        [Test]
        public void AcreditarCarga_Completamente_CuandoHayEspacioSuficiente_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.PagarBoleto(10000);

            bool resultado = tarjeta.AcreditarCarga();

            Assert.IsTrue(resultado);
            Assert.AreEqual(50000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.Acargar);
        }

        [Test]
        public void AcreditarCarga_Parcialmente_CuandoNoHayEspacioSuficiente_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.PagarBoleto(2000);

            bool resultado = tarjeta.AcreditarCarga();

            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2000, tarjeta.Acargar);
        }

        [Test]
        public void CargarSaldo_ConSaldoPendienteYAcreditar_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            bool pagoExitoso = tarjeta.PagarBoleto(5000);

            Assert.IsTrue(pagoExitoso);
            Assert.AreEqual(55000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.Acargar);
        }

        [Test]
        public void CargarSaldo_MontoNoPermitido_ConSaldoPendiente_Test()
        {
            tarjeta.ResetearSaldo();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            bool resultado = tarjeta.CargarSaldo(100);

            Assert.IsFalse(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.Acargar);
        }

        [Test]
        public void ObtenerTipoTarjeta_Test()
        {
            Assert.AreEqual("TarjetaNormal", tarjeta.ObtenerTipoTarjeta());
        }

        // NUEVOS TESTS PARA BOLETO DE USO FRECUENTE

        [Test]
        public void TarjetaNormal_Viajes1a29_TarifaNormal_Test()
        {
            tarjeta.ResetearSaldo();
            // Arrange
            tarjeta.CargarSaldo(30000);
            decimal tarifaBase = 1580m;

            // Act & Assert - Primeros 29 viajes deberían ser tarifa normal
            for (int i = 1; i <= 29; i++)
            {
                decimal tarifaCalculada = tarjeta.ObtenerTarifa(tarifaBase);
                Assert.AreEqual(tarifaBase, tarifaCalculada, $"Viaje {i} debería tener tarifa normal");

                // Simular pago exitoso
                tarjeta.PagarBoleto(tarifaCalculada);
            }
        }

        [Test]
        public void TarjetaNormal_Viajes30a59_20PorcientoDescuento_Test()
        {
            tarjeta.ResetearSaldo();
            // Arrange
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(20000);
            decimal tarifaBase = 1580m;
            decimal tarifaConDescuento = 1580m * 0.8m; // 1264

            // Realizar 29 viajes primero
            for (int i = 1; i <= 29; i++)
            {
                tarjeta.PagarBoleto(tarifaBase);
            }
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(10000);
            // Act & Assert - Viajes 30-59 deberían tener 20% de descuento
            for (int i = 30; i <= 59; i++)
            {
                decimal tarifaCalculada = tarjeta.ObtenerTarifa(tarifaBase);
                Assert.AreEqual(tarifaConDescuento, tarifaCalculada, $"Viaje {i} debería tener 20% de descuento");

                tarjeta.PagarBoleto(tarifaCalculada);
            }
        }

        [Test]
        public void TarjetaNormal_Viajes60a80_25PorcientoDescuento_Test()
        {
            tarjeta.ResetearSaldo();
            // Arrange
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000); 
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            decimal tarifaBase = 1580m;
            decimal tarifaConDescuento = 1580m * 0.75m; // 1185

            // Realizar 59 viajes primero
            for (int i = 1; i <= 59; i++)
            {
                tarjeta.PagarBoleto(tarifaBase);
            }

            // Act & Assert - Viajes 60-80 deberían tener 25% de descuento
            for (int i = 60; i <= 80; i++)
            {
                decimal tarifaCalculada = tarjeta.ObtenerTarifa(tarifaBase);
                Assert.AreEqual(tarifaConDescuento, tarifaCalculada, $"Viaje {i} debería tener 25% de descuento");

                tarjeta.PagarBoleto(tarifaCalculada);
            }
        }

        [Test]
        public void TarjetaNormal_Viaje81EnAdelante_TarifaNormal_Test()
        {
            tarjeta.ResetearSaldo();
            // Arrange
            tarjeta.CargarSaldo(50000);
            decimal tarifaBase = 1580m;

            // Realizar 80 viajes primero
            for (int i = 1; i <= 80; i++)
            {
                tarjeta.PagarBoleto(tarifaBase);
            }

            // Act & Assert - Viaje 81+ debería volver a tarifa normal
            decimal tarifaCalculada = tarjeta.ObtenerTarifa(tarifaBase);
            Assert.AreEqual(tarifaBase, tarifaCalculada, "Viaje 81+ debería tener tarifa normal");
        }

        [Test]
        public void TarjetaNormal_ReiniciaContadorAlCambiarDeMes_Test()
        {
            tarjeta.ResetearSaldo();
            // Arrange
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(20000);
            decimal tarifaBase = 1580m;

            // Realizar 40 viajes en el mes actual (con descuento del 20%)
            for (int i = 1; i <= 40; i++)
            {
                tarjeta.PagarBoleto(tarifaBase);
            }

            // Verificar que está en descuento
            decimal tarifaConDescuento = tarjeta.ObtenerTarifa(tarifaBase);
            Assert.AreEqual(1580m * 0.8m, tarifaConDescuento);

            // Act - Simular cambio de mes
            _tiempoSimulado = _tiempoSimulado.AddMonths(1);
            tarjeta.ResetearContadorViajes();

            // Assert - Primer viaje del nuevo mes debería ser tarifa normal
            decimal tarifaNuevoMes = tarjeta.ObtenerTarifa(tarifaBase);
            Assert.AreEqual(tarifaBase, tarifaNuevoMes, "Primer viaje del nuevo mes debería ser tarifa normal");
        }

        [Test]
        public void TarjetaNormal_CantidadViajesEsteMes_RetornaCorrectamente_Test()
        {
            tarjeta.ResetearSaldo();
            // Arrange
            tarjeta.CargarSaldo(20000);
            tarjeta.CargarSaldo(30000);

            // Act - Realizar 5 viajes
            for (int i = 1; i <= 5; i++)
            {
                tarjeta.PagarBoleto(1580m);
            }

            // Assert
            Assert.AreEqual(5, tarjeta.CantidadViajesEsteMes());
        }

        [Test]
        public void TarjetaNormal_ReiniciaContadorAlCambiarDeMes_EnPagarBoleto_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(10000);

            // Simular que estamos en un mes diferente al inicial
            _tiempoSimulado = new DateTime(2025, 12, 1, 10, 0, 0); // Diciembre 2025

            // Act - Hacer un viaje en diciembre (debería reiniciar contador)
            bool resultado = tarjeta.PagarBoleto(1580m);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(1, tarjeta.CantidadViajesEsteMes(), "Debería tener 1 viaje en diciembre");
        }

        [Test]
        public void TarjetaNormal_ReiniciaContadorAlCambiarDeMes_EnObtenerTarifa_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(10000);

            // Hacer algunos viajes en noviembre
            for (int i = 1; i <= 5; i++)
            {
                tarjeta.PagarBoleto(1580m);
            }

            Assert.AreEqual(5, tarjeta.CantidadViajesEsteMes(), "Debería tener 5 viajes en noviembre");

            // Cambiar a diciembre
            _tiempoSimulado = new DateTime(2025, 12, 1, 10, 0, 0);

            // Act - Obtener tarifa en diciembre (debería reiniciar contador)
            decimal tarifa = tarjeta.ObtenerTarifa(1580m);

            // Assert
            Assert.AreEqual(1580m, tarifa, "Primer viaje del mes debería ser tarifa normal");
            Assert.AreEqual(0, tarjeta.CantidadViajesEsteMes(), "Debería reiniciar contador a 0 en diciembre");
        }

        [Test]
        public void TarjetaNormal_CantidadViajesEsteMes_RetornaCeroSiCambioMes_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(10000);

            // Hacer viajes en noviembre
            for (int i = 1; i <= 3; i++)
            {
                tarjeta.PagarBoleto(1580m);
            }

            Assert.AreEqual(3, tarjeta.CantidadViajesEsteMes(), "Debería tener 3 viajes en noviembre");

            // Cambiar a diciembre
            _tiempoSimulado = new DateTime(2025, 12, 1, 10, 0, 0);

            // Act & Assert - CantidadViajesEsteMes debería retornar 0
            Assert.AreEqual(0, tarjeta.CantidadViajesEsteMes(), "Debería retornar 0 si cambió el mes");
        }

        [Test]
        public void TarjetaNormal_SimularCambioDeMes_FuncionaCorrectamente_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(10000);

            // Hacer algunos viajes
            for (int i = 1; i <= 5; i++)
            {
                tarjeta.PagarBoleto(1580m);
            }

            // Guardar mes actual
            DateTime mesOriginal = _tiempoSimulado;

            // Act - Simular cambio de mes
            tarjeta.SimularCambioDeMes();

            // Assert - Verificar que el método se ejecuta sin errores
            // Este test principalmente verifica que no hay excepciones
            Assert.DoesNotThrow(() => tarjeta.SimularCambioDeMes());
        }

        [Test]
        public void TarjetaNormal_CambioDeAnio_ReiniciaContador_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(10000);

            // Hacer viajes en noviembre 2025
            for (int i = 1; i <= 5; i++)
            {
                tarjeta.PagarBoleto(1580m);
            }

            // Cambiar a enero 2026 (cambio de año)
            _tiempoSimulado = new DateTime(2026, 1, 1, 10, 0, 0);

            // Act - Hacer un viaje en enero 2026
            bool resultado = tarjeta.PagarBoleto(1580m);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(1, tarjeta.CantidadViajesEsteMes(), "Debería reiniciar contador al cambiar de año");
        }

        [Test]
        public void TarjetaNormal_MultiplesCambiosDeMes_Test()
        {
            // Arrange
            tarjeta.CargarSaldo(20000);

            // Viaje en noviembre 2025
            tarjeta.PagarBoleto(1580m);
            Assert.AreEqual(1, tarjeta.CantidadViajesEsteMes());

            // Cambiar a diciembre 2025
            _tiempoSimulado = new DateTime(2025, 12, 15, 10, 0, 0);
            tarjeta.PagarBoleto(1580m);
            Assert.AreEqual(1, tarjeta.CantidadViajesEsteMes(), "Debería reiniciar en diciembre");

            // Cambiar a enero 2026
            _tiempoSimulado = new DateTime(2026, 1, 10, 10, 0, 0);
            tarjeta.PagarBoleto(1580m);
            Assert.AreEqual(1, tarjeta.CantidadViajesEsteMes(), "Debería reiniciar en enero");

            // Volver a noviembre 2025 (debería reiniciar nuevamente)
            _tiempoSimulado = new DateTime(2025, 11, 20, 10, 0, 0);
            tarjeta.PagarBoleto(1580m);
            Assert.AreEqual(1, tarjeta.CantidadViajesEsteMes(), "Debería reiniciar al volver a noviembre");
        }
    }
}