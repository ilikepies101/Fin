using System;

namespace Interview
{
    /// <summary>
    /// Indicates whether an amount represents a credit or a debit.
    /// </summary>
    [Flags]
    public enum CreditOrDebit
    {
        /// <summary>
        /// The balance type is unknown.
        /// </summary>
        Unknown = 0x0,

        /// <summary>
        /// The amount represents a credit. Equity and liabilities typically have a credit balance.
        /// </summary>
        Credit = 0x1,

        /// <summary>
        /// The amount represents a debit. Assets typically have a debit balance.
        /// </summary>
        Debit = 0x2,

        /// <summary>
        /// The amout is zero. This can be interpreted as either a credit or debit.
        /// </summary>
        Zero = Credit | Debit,
    }
}
