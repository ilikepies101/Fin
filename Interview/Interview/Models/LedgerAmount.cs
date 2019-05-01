using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;

namespace Interview
{
    /// <summary>
    /// Models a credit or debit amount.
    /// </summary>
    [DebuggerDisplay("{Type}({Value})")]
    public struct LedgerAmount : IEquatable<LedgerAmount>
    {
        /// <summary>
        /// Zero balance.
        /// </summary>
        public static LedgerAmount Zero = new LedgerAmount(0M, CreditOrDebit.Zero);

        /// <summary>
        /// Constructs a <see cref="LedgerAmount"/> from required parameters.
        /// </summary>
        /// <param name="value">The scalar balance of the account. Cannot be a negative value!</param>
        /// <param name="type">
        /// Used to identify the <paramref name="value"/> as either a credit or debit. If the amount
        /// is 0.00 then <paramref name="type"/> will be converted to <see cref="CreditOrDebit.Zero"/> automatically.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="value"/> is negative.</exception>
        public LedgerAmount(decimal value, CreditOrDebit type)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Only positive values are allowed for {nameof(value)}. Got {value}.");
            }

            Value = value;
            Type = value == 0 ? CreditOrDebit.Zero : type;
        }

        /// <summary>
        /// Specifies whether <see cref="Value"/> represents a credit or debit.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CreditOrDebit Type { get; }

        /// <summary>
        /// The scalar balance. Always positive or 0.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// A simple override factory method for creating <see cref="LedgerAmount"/> from a floating point number.
        /// </summary>
        public static LedgerAmount FromFloat(double value, CreditOrDebit type)
        {
            return new LedgerAmount((decimal)value, type);
        }

        /// <summary>
        /// Subtracts two balances.
        /// </summary>
        /// <param name="a">The first balance.</param>
        /// <param name="b">A second balance.</param>
        /// <returns>A new balance representing the difference of <paramref name="a"/> and <paramref name="b"/> with the resulting <see cref="CreditOrDebit"/>.</returns>
        public static LedgerAmount operator -(LedgerAmount a, LedgerAmount b)
        {
            var amountA = a.Type.HasFlag(CreditOrDebit.Credit) ? a.Value : -a.Value;
            var amountB = b.Type.HasFlag(CreditOrDebit.Credit) ? b.Value : -b.Value;

            var result = amountA - amountB;

            return result >= 0
                ? new LedgerAmount(result, CreditOrDebit.Credit)
                : new LedgerAmount(Math.Abs(result), CreditOrDebit.Debit);
        }

        /// <summary>
        /// Inverts the <see cref="LedgerAmount"/>. IE, credit -> debit and vice versa.
        /// </summary>
        /// <param name="a">The amount to invert.</param>
        /// <returns>A new ledger amount representing the inversion of the first.</returns>
        public static LedgerAmount operator -(LedgerAmount a)
        {
            return new LedgerAmount(a.Value, a.Type == CreditOrDebit.Credit ? CreditOrDebit.Debit : CreditOrDebit.Credit);
        }

        /// <summary>
        /// Returns true if two <see cref="LedgerAmount"/>s are not equal.
        /// </summary>
        /// <param name="a">The first <see cref="LedgerAmount"/>.</param>
        /// <param name="b">A second <see cref="LedgerAmount"/>.</param>
        public static bool operator !=(LedgerAmount a, LedgerAmount b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Adds two balances together.
        /// </summary>
        /// <param name="a">The first balance.</param>
        /// <param name="b">A second balance.</param>
        /// <returns>
        /// A new balance representing the sum of <paramref name="a"/> and <paramref name="b"/> with
        /// the resulting <see cref="CreditOrDebit"/>.
        /// </returns>
        public static LedgerAmount operator +(LedgerAmount a, LedgerAmount b)
        {
            if (a.Type == b.Type)
            {
                return new LedgerAmount(a.Value + b.Value, a.Type);
            }
            else if (a.Value >= b.Value)
            {
                return new LedgerAmount(a.Value - b.Value, a.Type);
            }
            else
            {
                return new LedgerAmount(b.Value - a.Value, b.Type);
            }
        }

        /// <summary>
        /// Returns true if two <see cref="LedgerAmount"/>s are equal.
        /// </summary>
        /// <param name="a">The first <see cref="LedgerAmount"/>.</param>
        /// <param name="b">A second <see cref="LedgerAmount"/>.</param>
        public static bool operator ==(LedgerAmount a, LedgerAmount b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// An implementation of <see cref="IEquatable{FinancialBalance}"/>
        /// </summary>
        /// <param name="other">The balance to compare against.</param>
        /// <returns>True if <see cref="Value"/> and <see cref="Type"/> are equivalent.</returns>
        public bool Equals(LedgerAmount other)
        {
            // This logic depends on the contructor of this class, which ensures that Type will
            // always be Zero when Amount is 0.00.
            return Value == other.Value && Type == other.Type;
        }

        /// <summary>
        /// Recommended override for equality implemenation.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns>True if <paramref name="obj"/> is also a <see cref="LedgerAmount"/> and is equivalent.</returns>
        public override bool Equals(object obj)
        {
            return obj is LedgerAmount a && a.Equals(this);
        }

        /// <summary>
        /// Recommended for implementation of equality.
        /// </summary>
        public override int GetHashCode()
        {
            var hashCode = 1265339359;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Output string representation
        /// </summary>
        public override string ToString()
        {
            switch (Type)
            {
                case CreditOrDebit.Zero:
                case CreditOrDebit.Unknown:
                    return Type.ToString();

                case CreditOrDebit.Credit:
                case CreditOrDebit.Debit:
                    return String.Concat(Type.ToString().Substring(0, 1), Value.ToString());

                default:
                    throw new Exception("Unknown type");
            }
        }
    }
}
