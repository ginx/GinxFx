// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CNPJ.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the CNPJ type.
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
    /// Defines the type for the CNPJ (CADASTRO NACIONAL DE PESSOAS JURÍDICAS).
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CNPJ", Justification = "It is an acronym.")]
    public struct CNPJ : IConvertible
    {
        /// <summary>
        /// A empty value for <see cref="CNPJ"/>.
        /// </summary>
        public static readonly CNPJ Empty = new CNPJ("00000000000000");

        /// <summary>
        /// A random number generator for <see cref="Generate"/>.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// The actual of the <see cref="CNPJ"/>.
        /// </summary>
        private readonly ulong value;

        /// <summary>
        /// Initializes a new instance of the <see cref="CNPJ"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public CNPJ(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException("value");
            }

            this.value = ulong.Parse(Regex.Replace(value, "[^0-9]+", string.Empty), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Generates a new randomized valid <see cref="CNPJ"/>.
        /// </summary>
        /// <returns>
        /// A new randomized valid <see cref="CNPJ"/>.
        /// </returns>
        public static CNPJ Generate()
        {
            var cnpj = new StringBuilder(14);

            for (var i = 0; i < 12; i++)
            {
                cnpj.Append(Random.Next(10));
            }

            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var soma1 = 0;
            for (var i = 0; i < 12; i++)
            {
                soma1 += (cnpj[i] - '0') * multiplicador1[i];
            }

            var resto1 = 11 - (soma1 % 11);
            cnpj.Append(resto1 >= 10 ? 0 : resto1);

            var soma2 = 0;
            for (var i = 0; i < 13; i++)
            {
                soma2 += (cnpj[i] - '0') * multiplicador2[i];
            }

            var resto2 = 11 - (soma2 % 11);
            cnpj.Append(resto2 >= 10 ? 0 : resto2);

            return new CNPJ(cnpj.ToString());
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception will be generated if <paramref name="value"/> isn't a valid <see cref="CNPJ"/>.
        /// </exception>
        /// <returns>
        /// A new instance of <see cref="CNPJ"/>.
        /// </returns>
        public static CNPJ Parse(string value)
        {
            var result = new CNPJ(value);

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
        /// <param name="cnpj">
        /// The parsed CNPJ.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is a valid <see cref="CNPJ"/>; <c>false</c> otherwise.
        /// </returns>
        public static bool TryParse(string value, out CNPJ cnpj)
        {
            cnpj = new CNPJ(value);

            return cnpj.IsValid();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="cnpj1">
        /// The first CNPJ.
        /// </param>
        /// <param name="cnpj2">
        /// The second CNPJ.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(CNPJ cnpj1, CNPJ cnpj2)
        {
            return cnpj1.value == cnpj2.value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="CNPJ"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CNPJ(string value)
        {
            return string.IsNullOrEmpty(value) ? Empty : new CNPJ(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CNPJ"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="cnpj">
        /// The CNPJ value.
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CNPJ cnpj)
        {
            return cnpj.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="cnpj1">
        /// The first CNPJ.
        /// </param>
        /// <param name="cnpj2">
        /// The second CNPJ.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(CNPJ cnpj1, CNPJ cnpj2)
        {
            return cnpj1.value == cnpj2.value;
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
            if (!(obj is CNPJ))
            {
                return false;
            }

            return this.value == ((CNPJ)obj).value;
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
            return this.value == 0L;
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            if (this.IsEmpty())
            {
                return false;
            }

            var cnpj = this.ToString(CultureInfo.InvariantCulture);

            if (cnpj.Length != 14)
            {
                return false;
            }

            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var soma1 = 0;
            for (var i = 0; i < 12; i++)
            {
                soma1 += (cnpj[i] - '0') * multiplicador1[i];
            }

            var resto1 = 11 - (soma1 % 11);
            resto1 = resto1 >= 10 ? 0 : resto1;

            var soma2 = 0;
            for (var i = 0; i < 13; i++)
            {
                soma2 += (cnpj[i] - '0') * multiplicador2[i];
            }

            var resto2 = 11 - (soma2 % 11);
            resto2 = resto2 >= 10 ? 0 : resto2;

            var verificador = resto1.ToString(CultureInfo.InvariantCulture)
                              + resto2.ToString(CultureInfo.InvariantCulture);

            return cnpj.EndsWith(verificador, StringComparison.OrdinalIgnoreCase);
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

            return this.ToString(CultureInfo.InvariantCulture);
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

            var v = this.ToString(CultureInfo.InvariantCulture);

            switch (format)
            {
                case "F":
                case "f":
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.{1}.{2}/{3}-{4}", 
                        v.Substring(0, 2), 
                        v.Substring(2, 3), 
                        v.Substring(5, 3), 
                        v.Substring(8, 4), 
                        v.Substring(12, 2));

                case "G":
                case "g":
                    return v;
            }

            throw new FormatException();
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
        public string ToString(IFormatProvider provider)
        {
            return this.value.ToString("00000000000000", CultureInfo.InvariantCulture);
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