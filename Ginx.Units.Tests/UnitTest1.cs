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
            var temperature = new Amount(37.5, TemperatureUnits.DegreeCelcius) * 2;
            var temperatureInDegreeFahrenheit = temperature.ConvertTo(TemperatureUnits.DegreeFahrenheit);
            var total = temperature + temperatureInDegreeFahrenheit;

            var consume = new Amount(1, EnergyUnits.KiloWattHour);
        }
    }
}
