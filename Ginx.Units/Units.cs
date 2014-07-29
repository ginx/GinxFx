using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Ginx.Units
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UnitDefinitionAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class UnitConversionAttribute : Attribute
    {
    }

    [Serializable]
    public class UnitConversionException : InvalidOperationException
    {
        public UnitConversionException() : base() { }

        public UnitConversionException(string message) : base(message) { }

        public UnitConversionException(Unit fromUnit, Unit toUnit)
            : this(String.Format("Failed to convert from unit '{0}' to unit '{1}'. Units are not compatible and no conversions are defined.", fromUnit.Name, toUnit.Name))
        {
        }

        protected UnitConversionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public sealed class UnitType
    {
        public static readonly UnitType None = new UnitType(string.Empty);

        private readonly string name;
        private readonly int[] dimensions;

        public UnitType(string name)
        {
            this.name = name;
        }

        public UnitType(string name, int[] dimensions)
            : this(name)
        {
            if (dimensions.Length != 7)
            {
                throw new ArgumentOutOfRangeException("An UnitType must have 7 dimensions.");
            }

            this.dimensions = new int[7];
            dimensions.CopyTo(this.dimensions, 0);
        }

        public UnitType Pow(int power)
        {
            var unitType = new UnitType(string.Format("{0}^{1}", this.name, power), this.dimensions);

            for (int i = 0; i < unitType.dimensions.Length; i++)
            {
                unitType.dimensions[i] *= power;
            }

            return unitType;
        }

        public static bool operator ==(UnitType left, UnitType right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            if (object.ReferenceEquals(left, null))
            {
                return false;
            }

            if (object.ReferenceEquals(right, null))
            {
                return false;
            }

            if (left.dimensions == null)
            {
                return false;
            }

            if (right.dimensions == null)
            {
                return false;
            }

            for (int i = 0; i < left.dimensions.Length; i++)
            {
                if (left.dimensions[i] != right.dimensions[i]) return false;
            }

            return true;
        }

        public static bool operator !=(UnitType left, UnitType right)
        {
            return !(left == right);
        }

        public static UnitType operator *(UnitType left, UnitType right)
        {
            var unitType = new UnitType(string.Format("{0}·{1}", left.name, right.name), left.dimensions);

            for (int i = 0; i < unitType.dimensions.Length; i++)
            {
                unitType.dimensions[i] += right.dimensions[i];
            }

            return unitType;
        }

        public static UnitType operator /(UnitType left, UnitType right)
        {
            var unitType = new UnitType(string.Format("{0}/{1}", left.name, right.name), left.dimensions);

            for (int i = 0; i < unitType.dimensions.Length; i++)
            {
                unitType.dimensions[i] -= right.dimensions[i];
            }

            return unitType;
        }

        public override string ToString()
        {
            return this.name;
        }
    }

    public sealed class Unit
    {
        public static readonly Unit None = new Unit(string.Empty, string.Empty, UnitType.None);

        private readonly string name;
        private readonly string symbol;
        private readonly UnitType unitType;
        private readonly double factor;

        public Unit(string name, string symbol, Unit unit)
            : this(name, symbol, unit.factor, unit.unitType)
        {
        }

        public Unit(string name, string symbol, UnitType unitType)
            : this(name, symbol, 1, unitType)
        {
        }

        public Unit(string name, string symbol, double factor, UnitType unitType)
        {
            this.name = name;
            this.symbol = symbol;
            this.factor = factor;
            this.unitType = unitType;
        }

        public string Name { get { return this.name; } }
        public string Symbol { get { return this.symbol; } }
        public double Factor { get { return this.factor; } }
        public UnitType UnitType { get { return this.unitType; } }

        public bool IsCompatibleTo(Unit otherUnit)
        {
            return (this.unitType == (otherUnit ?? Unit.None).unitType);
        }

        public Unit Pow(int power)
        {
            return new Unit(
                string.Format("({0}^{1})", this.name, power),
                string.Format("({0}^{1})", this.symbol, power),
                Math.Pow(this.factor, power),
                this.unitType.Pow(power));
        }

        public static Unit operator *(Unit left, double right)
        {
            return (right * left);
        }

        public static Unit operator *(double left, Unit right)
        {
            right = right ?? Unit.None;
            return new Unit(
                string.Format("({0}·{1}", left, right.name),
                string.Format("({0}·{1}", left, right.symbol),
                left * right.factor,
                right.unitType);
        }

        public static Unit operator *(Unit left, Unit right)
        {
            left = left ?? Unit.None;
            right = right ?? Unit.None;

            return new Unit(
                string.Format("({0}·{1})", left.name, right.name),
                string.Format("({0}·{1})", left.symbol, right.symbol),
                left.factor * right.factor,
                left.unitType * right.unitType);
        }

        public static Unit operator /(Unit left, double right)
        {
            left = left ?? Unit.None;
            return new Unit(
                string.Format("({0}/{1}", left.name, right),
                string.Format("({0}/{1}", left.symbol, right),
                left.factor * right,
                left.unitType);
        }

        public static Unit operator /(double left, Unit right)
        {
            right = right ?? Unit.None;
            return new Unit(
                string.Format("({0}/{1}", left, right.name),
                string.Format("({0}/{1}", left, right.symbol),
                left * right.factor,
                right.unitType);
        }

        public static Unit operator /(Unit left, Unit right)
        {
            left = left ?? Unit.None;
            right = right ?? Unit.None;

            return new Unit(
                string.Format("({0}/{1})", left.name, right.name),
                string.Format("({0}/{1})", left.symbol, right.symbol),
                left.factor / right.factor,
                left.unitType / right.unitType);
        }

        public override string ToString()
        {
            return this.symbol;
        }
    }

    public sealed class Amount
    {
        private const int equalityPrecision = 8;

        private double value;
        private Unit unit;

        public Amount(double value, Unit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public Unit Unit
        {
            get
            {
                return this.unit;
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
        }

        public Amount ConvertTo(Unit unit)
        {
            return UnitManager.ConvertTo(this, unit);
        }

        public static bool operator ==(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, right))
            {
                return true;
            }

            if (Object.ReferenceEquals(left, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(right, null))
            {
                return false;
            }

            try
            {
                return Math.Round(left.value, Amount.equalityPrecision) == Math.Round(right.ConvertTo(left.Unit).value, Amount.equalityPrecision);
            }
            catch (UnitConversionException)
            {
                return false;
            }
        }

        public static bool operator !=(Amount left, Amount right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return (this == (obj as Amount));
        }

        public bool Equals(Amount amount)
        {
            return (this == amount);
        }

        public static Amount operator +(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, null) && Object.ReferenceEquals(right, null))
            {
                return null;
            }

            left = left ?? new Amount(0, right.unit);
            right = right ?? new Amount(0, left.unit);

            return new Amount(left.value + right.ConvertTo(left.unit).value, left.unit);
        }

        public static Amount operator +(double left, Amount right)
        {
            return right + left;
        }

        public static Amount operator +(Amount left, double right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return null;
            }

            return new Amount(left.value + right, left.unit);
        }

        public static Amount operator -(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, null) && Object.ReferenceEquals(right, null))
            {
                return null;
            }

            left = left ?? new Amount(0, right.unit);
            right = right ?? new Amount(0, left.unit);

            return new Amount(left.value - right.ConvertTo(left.unit).value, left.unit);
        }

        public static Amount operator -(double left, Amount right)
        {
            return right - left;
        }

        public static Amount operator -(Amount left, double right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return null;
            }

            return new Amount(left.value - right, left.unit);
        }

        public static Amount operator *(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return null;
            }

            if (Object.ReferenceEquals(right, null))
            {
                return null;
            }

            return new Amount(left.value * right.value, left.unit * right.unit);
        }

        public static Amount operator *(double left, Amount right)
        {
            return right * left;
        }

        public static Amount operator *(Amount left, double right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return null;
            }

            return new Amount(left.value * right, left.unit);
        }

        public static Amount operator /(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return null;
            }

            if (Object.ReferenceEquals(right, null))
            {
                return null;
            }

            return new Amount(left.value / right.value, left.unit / right.unit);
        }

        public static Amount operator /(double left, Amount right)
        {
            if (Object.ReferenceEquals(right, null))
            {
                return null;
            }

            return new Amount(left / right.value, right.unit);
        }

        public static Amount operator /(Amount left, double right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return null;
            }

            return new Amount(left.value / right, left.unit);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.value, this.unit.Symbol);
        }
    }

    public sealed class SIBaseUnits
    {
        public static readonly UnitType Metre = new UnitType("metre", new[] { 1, 0, 0, 0, 0, 0, 0 });
        public static readonly UnitType Mass = new UnitType("kilogram", new[] { 0, 1, 0, 0, 0, 0, 0 });
        public static readonly UnitType Time = new UnitType("second", new[] { 0, 0, 1, 0, 0, 0, 0 });
        public static readonly UnitType ElectricCurrent = new UnitType("ampere", new[] { 0, 0, 0, 1, 0, 0, 0 });
        public static readonly UnitType ThermodynamicTemperature = new UnitType("kelvin", new[] { 0, 0, 0, 0, 1, 0, 0 });
        public static readonly UnitType AmountOfSubstance = new UnitType("mole", new[] { 0, 0, 0, 0, 0, 1, 0 });
        public static readonly UnitType LuminousIntensity = new UnitType("candela", new[] { 0, 0, 0, 0, 0, 0, 1 });
    }

    public sealed class UnitManager
    {
        private static readonly Lazy<UnitManager> lazyInstance = new Lazy<UnitManager>(() => new UnitManager());

        private HashSet<Unit> units = new HashSet<Unit>();
        private Dictionary<UnitType, HashSet<Unit>> unitsByType = new Dictionary<UnitType, HashSet<Unit>>();
        private Dictionary<string, Unit> unitsByName = new Dictionary<string, Unit>();
        private Dictionary<string, Unit> unitsBySymbol = new Dictionary<string, Unit>();

        private Dictionary<Tuple<Unit, Unit>, Func<Amount, Amount>> conversions = new Dictionary<System.Tuple<Unit, Unit>, System.Func<Amount, Amount>>();

        public static UnitManager Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        private UnitManager()
        {
        }

        public static Amount ConvertTo(Amount amount, Unit toUnit)
        {
            try
            {
                if (Object.ReferenceEquals(amount.Unit, toUnit))
                {
                    return amount;
                }

                if (amount.Unit.IsCompatibleTo(toUnit))
                {
                    return new Amount(amount.Value * amount.Unit.Factor / toUnit.Factor, toUnit);
                }
                else
                {
                    return Instance.conversions[new Tuple<Unit, Unit>(amount.Unit, toUnit)](amount);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new UnitConversionException(amount.Unit, toUnit);
            }
        }

        public void RegisterConversion(Unit fromUnit, Unit toUnit, Func<Amount, Amount> conversion)
        {
            this.conversions[new Tuple<Unit, Unit>(fromUnit, toUnit)] = conversion;
        }

        public void Register(Unit unit)
        {
            if (unit == null) throw new ArgumentNullException("unit");

            if (this.units.Contains(unit))
            {
                return;
            }

            this.units.Add(unit);

            try
            {
                this.unitsByType[unit.UnitType].Add(unit);
            }
            catch (KeyNotFoundException)
            {
                this.unitsByType[unit.UnitType] = new HashSet<Unit>();
                this.unitsByType[unit.UnitType].Add(unit);
            }

            this.unitsByName[unit.Name] = unit;
            this.unitsBySymbol[unit.Symbol] = unit;
        }

        public void Register(Type klass)
        {
            foreach (FieldInfo field in klass.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(Unit))
                {
                    this.Register((Unit)field.GetValue(null));
                }
            }

            foreach (MethodInfo method in klass.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static))
            {
                if (method.GetCustomAttributes(typeof(UnitConversionAttribute), false).Length == 0)
                {
                    continue;
                }

                if ((method.ReturnType == typeof(void)) && (method.GetParameters().Length == 0))
                {
                    method.Invoke(null, new object[0]);
                }
            }
        }

        public void Register(Assembly assembly)
        {
            foreach (Type t in assembly.GetExportedTypes())
            {
                if (t.GetCustomAttributes(typeof(UnitDefinitionAttribute), false).Length > 0)
                {
                    this.Register(t);
                }
            }
        }
    }

    [UnitDefinition]
    public sealed class LengthUnits
    {
        public static readonly Unit Meter = new Unit("meter", "m", SIBaseUnits.Metre);
        public static readonly Unit MilliMeter = new Unit("millimeter", "mm", 0.001 * Meter);
        public static readonly Unit CentiMeter = new Unit("centimeter", "cm", 0.01 * Meter);
        public static readonly Unit DeciMeter = new Unit("decimeter", "dm", 0.1 * Meter);
        public static readonly Unit DecaMeter = new Unit("decameter", "Dm", 10.0 * Meter);
        public static readonly Unit HectoMeter = new Unit("hectometer", "Hm", 100.0 * Meter);
        public static readonly Unit KiloMeter = new Unit("kilometer", "km", 1000.0 * Meter);

        public static readonly Unit Inch = new Unit("inch", "in", 0.0254 * Meter);
        public static readonly Unit Foot = new Unit("foot", "ft", 12.0 * Inch);
        public static readonly Unit Yard = new Unit("yard", "yd", 36.0 * Inch);
        public static readonly Unit Mile = new Unit("mile", "mi", 5280.0 * Foot);
        public static readonly Unit NauticalMile = new Unit("nautical mile", "nmi", 1852.0 * Meter);

        public static readonly Unit LightYear = new Unit("light-year", "ly", 9460730472580800.0 * Meter);
    }

    [UnitDefinition]
    public sealed class AreaUnits
    {
        public static readonly Unit SquareMeter = new Unit("meter²", "m²", LengthUnits.Meter.Pow(2));
        public static readonly Unit SquareMilliMeter = new Unit("millimeter²", "mm²", LengthUnits.MilliMeter.Pow(2));
        public static readonly Unit SquareCentiMeter = new Unit("centimeter²", "cm²", LengthUnits.CentiMeter.Pow(2));
        public static readonly Unit SquareKiloMeter = new Unit("kilometer²", "km²", LengthUnits.KiloMeter.Pow(2));

        public static readonly Unit Are = new Unit("are", "are", 100.0 * SquareMeter);
        public static readonly Unit HectAre = new Unit("hectare", "ha", 10000.0 * SquareMeter);
    }

    [UnitDefinition]
    public static class VolumeUnits
    {
        public static readonly Unit CubicMeter = new Unit("meter³", "m³", LengthUnits.Meter.Pow(3));
        public static readonly Unit CubicMilliMeter = new Unit("millimeter²", "mm²", LengthUnits.MilliMeter.Pow(3));
        public static readonly Unit CubicCentiMeter = new Unit("centimeter²", "cm²", LengthUnits.CentiMeter.Pow(3));

        public static readonly Unit Liter = new Unit("liter", "L", LengthUnits.DeciMeter.Pow(3));
        public static readonly Unit MilliLiter = new Unit("milliliter", "mL", 0.001 * Liter);
        public static readonly Unit CentiLiter = new Unit("centiliter", "cL", 0.01 * Liter);
        public static readonly Unit DeciLiter = new Unit("deciliter", "dL", 0.1 * Liter);
    }

    [UnitDefinition]
    public static class TimeUnits
    {
        public static readonly Unit Second = new Unit("second", "s", SIBaseUnits.Time);
        public static readonly Unit MicroSecond = new Unit("microsecond", "μs", 0.000001 * Second);
        public static readonly Unit MilliSecond = new Unit("millisecond", "ms", 0.001 * Second);
        public static readonly Unit Minute = new Unit("minute", "min", 60.0 * Second);
        public static readonly Unit Hour = new Unit("hour", "h", 3600.0 * Second);
        public static readonly Unit Day = new Unit("day", "d", 24.0 * Hour);
    }

    [UnitDefinition]
    public static class SpeedUnits
    {
        public static readonly Unit MeterPerSecond = new Unit("meter/second", "m/s", LengthUnits.Meter / TimeUnits.Second);
        public static readonly Unit KilometerPerHour = new Unit("kilometer/hour", "km/h", LengthUnits.KiloMeter / TimeUnits.Hour);
        public static readonly Unit MilePerHour = new Unit("mile/hour", "mi/h", LengthUnits.Mile / TimeUnits.Hour);
        public static readonly Unit Knot = new Unit("knot", "kn", 1.852 * SpeedUnits.KilometerPerHour);
    }

    [UnitDefinition]
    public static class MassUnits
    {
        public static readonly Unit KiloGram = new Unit("kilogram", "Kg", SIBaseUnits.Mass);
        public static readonly Unit Gram = new Unit("gram", "g", 0.001 * KiloGram);
        public static readonly Unit MilliGram = new Unit("milligram", "mg", 0.001 * Gram);
        public static readonly Unit Ton = new Unit("ton", "ton", 1000.0 * KiloGram);
    }

    [UnitDefinition]
    public static class ForceUnits
    {
        public static readonly Unit Newton = new Unit("newton", "N", LengthUnits.Meter * MassUnits.KiloGram * TimeUnits.Second.Pow(-2));
    }

    [UnitDefinition]
    public static class EnergyUnits
    {
        public static readonly Unit Joule = new Unit("joule", "J", LengthUnits.Meter.Pow(2) * MassUnits.KiloGram * TimeUnits.Second.Pow(-2));
        public static readonly Unit KiloJoule = new Unit("kilojoule", "kJ", 1000.0 * Joule);
        public static readonly Unit MegaJoule = new Unit("megajoule", "MJ", 1000000.0 * Joule);
        public static readonly Unit GigaJoule = new Unit("gigajoule", "GJ", 1000000000.0 * Joule);

        public static readonly Unit Watt = new Unit("watt", "W", Joule / TimeUnits.Second);
        public static readonly Unit KiloWatt = new Unit("kilowatt", "kW", 1000.0 * Watt);
        public static readonly Unit MegaWatt = new Unit("megawatt", "MW", 1000000.0 * Watt);

        public static readonly Unit WattSecond = new Unit("watt-second", "Wsec", Watt * TimeUnits.Second);
        public static readonly Unit WattHour = new Unit("watt-hour", "Wh", Watt * TimeUnits.Hour);
        public static readonly Unit KiloWattHour = new Unit("kilowatt-hour", "kWh", 1000.0 * WattHour);

        public static readonly Unit Calorie = new Unit("calorie", "cal", 4.1868 * Joule);
        public static readonly Unit KiloCalorie = new Unit("kilocalorie", "kcal", 1000.0 * Calorie);

        public static readonly Unit HorsePower = new Unit("horsepower", "hp", 0.73549875 * KiloWatt);
    }

    [UnitDefinition]
    public static class ElectricUnits
    {
        public static readonly Unit Ampere = new Unit("ampere", "amp", SIBaseUnits.ElectricCurrent);
        public static readonly Unit Coulomb = new Unit("coulomb", "C", TimeUnits.Second * Ampere);
        public static readonly Unit Volt = new Unit("volt", "V", EnergyUnits.Watt / Ampere);
        public static readonly Unit Ohm = new Unit("ohm", "Ω", Volt / Ampere);
        public static readonly Unit Farad = new Unit("farad", "F", Coulomb / Volt);
    }

    [UnitDefinition]
    public static class PressureUnits
    {
        public static readonly Unit Pascal = new Unit("pascal", "Pa", ForceUnits.Newton * LengthUnits.Meter.Pow(-2));
        public static readonly Unit HectoPascal = new Unit("hectopascal", "hPa", 100.0 * Pascal);
        public static readonly Unit KiloPascal = new Unit("kilopascal", "KPa", 1000.0 * Pascal);
        public static readonly Unit Bar = new Unit("bar", "bar", 100000.0 * Pascal);
        public static readonly Unit MilliBar = new Unit("millibar", "mbar", 0.001 * Bar);
        public static readonly Unit Atmosphere = new Unit("atmosphere", "atm", 101325.0 * Pascal);
    }

    [UnitDefinition]
    public static class FrequencyUnits
    {
        public static readonly Unit Hertz = new Unit("Hertz", "hz", TimeUnits.Second.Pow(-1));
        public static readonly Unit KiloHertz = new Unit("KiloHertz", "hz", 1000.0 * Hertz);
        public static readonly Unit MegaHerts = new Unit("MegaHertz", "Mhz", 1000000.0 * Hertz);
        public static readonly Unit RPM = new Unit("Rounds per minute", "rpm", TimeUnits.Minute.Pow(-1));
    }

    [UnitDefinition]
    public static class AmountOfSubstanceUnits
    {
        public static readonly Unit Mole = new Unit("mole", "mol", SIBaseUnits.AmountOfSubstance);
    }

    [UnitDefinition]
    public static class LuminousIntensityUnits
    {
        public static readonly Unit Candela = new Unit("candela", "cd", SIBaseUnits.LuminousIntensity);
    }

    [UnitDefinition]
    public static class TemperatureUnits
    {
        public static readonly Unit Kelvin = new Unit("Kelvin", "K", SIBaseUnits.ThermodynamicTemperature);
        public static readonly Unit DegreeCelcius = new Unit("degree celcius", "°C", new UnitType("celcius temperature"));
        public static readonly Unit DegreeFahrenheit = new Unit("degree fahrenheit", "°F", new UnitType("fahrenheit temperature"));

        #region Conversion functions

        [UnitConversion]
        public static void Register()
        {
            // Register conversion functions:

            // Convert Celcius to Fahrenheit:
            UnitManager.Instance.RegisterConversion(
                DegreeCelcius,
                DegreeFahrenheit,
                amount => new Amount(amount.Value * 9.0 / 5.0 + 32.0, DegreeFahrenheit));

            // Convert Fahrenheit to Celcius:
            UnitManager.Instance.RegisterConversion(
                DegreeFahrenheit,
                DegreeCelcius,
                amount => new Amount((amount.Value - 32.0) / 9.0 * 5.0, DegreeCelcius));

            // Convert Celcius to Kelvin:
            UnitManager.Instance.RegisterConversion(
                DegreeCelcius,
                Kelvin,
                amount => new Amount(amount.Value + 273.15, Kelvin));

            // Convert Kelvin to Celcius:
            UnitManager.Instance.RegisterConversion(
                Kelvin,
                DegreeCelcius,
                amount => new Amount(amount.Value - 273.15, DegreeCelcius));

            // Convert Fahrenheit to Kelvin:
            UnitManager.Instance.RegisterConversion(
                DegreeFahrenheit,
                Kelvin,
                amount => amount.ConvertTo(DegreeCelcius).ConvertTo(Kelvin));

            // Convert Kelvin to Fahrenheit:
            UnitManager.Instance.RegisterConversion(
                Kelvin,
                DegreeFahrenheit,
                amount => amount.ConvertTo(DegreeCelcius).ConvertTo(DegreeFahrenheit));
        }

        #endregion Conversion functions
    }
}