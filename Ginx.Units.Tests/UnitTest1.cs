using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ginx.Units.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UnitManager.Instance.Register(typeof(UnitManager).Assembly);

            var speed = new Amount(10, SpeedUnits.MeterPerSecond);
            var km = speed.ConvertTo(SpeedUnits.KilometerPerHour);

            var m = string.Format("{0:000}", 9999);
            //var temperature = new Amount(37.5, TemperatureUnits.DegreeCelcius) * 2;
            //var temperatureInDegreeFahrenheit = temperature.ConvertTo(TemperatureUnits.DegreeFahrenheit);
            //var total = temperature + temperatureInDegreeFahrenheit;
            //var consume = new Amount(1, EnergyUnits.KiloWattHour);

            // 6.67×10−11 N·(m/kg)2
            //var gravitational = new Amount(6.67384 * Math.Pow(10, -11),
            //    ForceUnits.Newton * (LengthUnits.Meter / MassUnits.KiloGram).Pow(2));

            //Assert.AreEqual(gravitational, PhysicsConstant.NewtonianGravitation);

            //var gravity = new Amount(9.8, AccelerationUnits.MeterPerSecondSquared);

            //var nucleous = new Amount(300, AreaUnits.Barn);

            //for (int i = 0; i < 100; i++)
            //{
            //    var time = new Amount(i, TimeUnits.Second);
            //    var speed = gravity * time;
            //    var distance = 0.5 * gravity * time * time;
            //}
        }
    }
}
