using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Interview
{
    /// <summary>
    /// A label and amount for a financial figure at it appears in a Balance Sheet.
    /// </summary>
    [DebuggerDisplay("LineItem {Label} {Total.Type.ToString()}({Total.Value})")]
    public class LineItem
    {
        /// <summary>
        /// This property represents the monetary amount associated with the <see cref="Label"/> not including <see cref="Sublines"/>.
        /// Example: A Balance Sheet might contain a line item labeled 'Accounts Receivable' with an amount of 500,000 Debit.
        /// When operating mathematically on Credit and Debit amounts you simply represent each with an opposing sign.
        /// So:
        ///    $1 Debit + $1 Debit = $2 Debit
        ///    $1 Credit + $1 Credit = $2 Credit
        ///    $1 Debit + $1 Credit = $0
        ///    <seealso cref="LedgerAmount"/>
        /// </summary>
        public virtual LedgerAmount Amount { get; set; } = LedgerAmount.Zero;

        /// <summary>
        /// A text label for the line item.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// A set of line lines that appear under this categorical heading.
        /// Example: If this line item were 'Current Assets' you might find two sub-lines labeled 'Accounts Receivable' and 'Checking Account'.
        /// In other words, 'Accounts Receivable' and 'Checking Account' are categorized as a 'Current Asset' and the total 'Current Assets' is computed by summing 'Accounts Receivable' and 'Checking Account'.
        /// </summary>
        public List<LineItem> Sublines { get; set; } = new List<LineItem>();

        /// <summary>
        /// This property represents the TOTAL monetary amount associated with <see cref="Label"/> including <see cref="Amount"/> and the total of all <see cref="Sublines"/>.
        /// In other words, this property is used to model subtotals that appear in a Balance Sheet.
        /// </summary>
        public LedgerAmount Total => throw new NotImplementedException();
    }
}
