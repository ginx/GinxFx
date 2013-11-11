// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CPF.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the CPF type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GinxFx.Types.Brazil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the type for the CPF (CADASTRO DE PESSOAS FÍSICAS).
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CPF", Justification = "It is an acronym.")]
    public struct CPF : IConvertible
    {
        /// <summary>
        /// A empty value for <see cref="CPF"/>.
        /// </summary>
        public static readonly CPF Empty = new CPF("00000000000");

        /// <summary>
        /// A random number generator for <see cref="Generate"/>.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// The actual of the <see cref="CPF"/>.
        /// </summary>
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="CPF"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public CPF(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException("value");
            }

            this.value = Regex.Replace(value, "[^0-9]+", string.Empty);
        }

        /// <summary>
        /// Generates a new randomized valid <see cref="CPF"/>.
        /// </summary>
        /// <returns>
        /// A new randomized valid <see cref="CPF"/>.
        /// </returns>
        public static CPF Generate()
        {
            var cpf = new StringBuilder(11);

            for (var i = 0; i < 9; i++)
            {
                cpf.Append(Random.Next(10));
            }

            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var soma1 = 0;
            for (var i = 0; i < 9; i++)
            {
                soma1 += (cpf[i] - '0') * multiplicador1[i];
            }

            var resto1 = 11 - (soma1 % 11);
            cpf.Append(resto1 >= 10 ? 0 : resto1);

            var soma2 = 0;
            for (var i = 0; i < 10; i++)
            {
                soma2 += (cpf[i] - '0') * multiplicador2[i];
            }

            var resto2 = 11 - (soma2 % 11);
            cpf.Append(resto2 >= 10 ? 0 : resto2);

            return new CPF(cpf.ToString());
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception will be generated if <paramref name="value"/> isn't a valid <see cref="CPF"/>.
        /// </exception>
        /// <returns>
        /// A new instance of <see cref="CPF"/>.
        /// </returns>
        public static CPF Parse(string value)
        {
            var result = new CPF(value);

            if (!result.IsValid())
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return result;
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="cpf">
        /// The parsed CPF.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is a valid <see cref="CPF"/>; <c>false</c> otherwise.
        /// </returns>
        public static bool TryParse(string value, out CPF cpf)
        {
            cpf = new CPF(value);

            return cpf.IsValid();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="cpf1">
        /// The first CPF.
        /// </param>
        /// <param name="cpf2">
        /// The second CPF.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(CPF cpf1, CPF cpf2)
        {
            return cpf1.value == cpf2.value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="CPF"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CPF(string value)
        {
            return string.IsNullOrEmpty(value) ? Empty : new CPF(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CPF"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="cpf">
        /// The CPF value.
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CPF cpf)
        {
            return cpf.value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="cpf1">
        /// The first CPF.
        /// </param>
        /// <param name="cpf2">
        /// The second CPF.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(CPF cpf1, CPF cpf2)
        {
            return cpf1.value != cpf2.value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CPF))
            {
                return false;
            }

            return this.value == ((CPF)obj).value;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return 11 * this.value.GetHashCode();
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(this.value);
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(this.value))
            {
                return false;
            }

            var cpf = Regex.Replace(this.value, "[^0-9]+", string.Empty);

            if (cpf.Length != 11)
            {
                return false;
            }

            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var soma1 = 0;
            for (var i = 0; i < 9; i++)
            {
                soma1 += (cpf[i] - '0') * multiplicador1[i];
            }

            var resto1 = 11 - (soma1 % 11);
            resto1 = resto1 >= 10 ? 0 : resto1;

            var soma2 = 0;
            for (var i = 0; i < 10; i++)
            {
                soma2 += (cpf[i] - '0') * multiplicador2[i];
            }

            var resto2 = 11 - (soma2 % 11);
            resto2 = resto2 >= 10 ? 0 : resto2;

            var verificador = resto1.ToString(CultureInfo.InvariantCulture)
                              + resto2.ToString(CultureInfo.InvariantCulture);

            return cpf.EndsWith(verificador, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return !string.IsNullOrEmpty(this.value) ? this.value : Empty.value;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            var v = this.IsValid() ? this.value : Empty.value;

            switch (format)
            {
                case "F":
                case "f":
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.{1}.{2}-{3}",
                        v.Substring(0, 3),
                        v.Substring(3, 3),
                        v.Substring(6, 3),
                        v.Substring(9, 2));

                case "G":
                case "g":
                    return v;
            }

            throw new FormatException();
        }

        /// <summary>
        /// Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
        /// </returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
        /// </returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.value;
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">
        /// The <see cref="T:System.Type"/> to which the value of this instance is converted.
        /// </param>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        /// </returns>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this.value, conversionType, provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidOperationException("Conversion not allowed.");
        }
    }
}