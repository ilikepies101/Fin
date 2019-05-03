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
        public string Label { get; set; } = "";

        /// <summary>
        /// A set of line items that appear under this categorical heading.
        /// Example: If this line item were 'Current Assets' you might find two sub-lines labeled 'Accounts Receivable' and 'Checking Account'.
        /// In other words, 'Accounts Receivable' and 'Checking Account' are categorized as a 'Current Asset' and the total 'Current Assets' is computed by summing 'Accounts Receivable' and 'Checking Account'.
        /// </summary>
        public List<LineItem> Sublines { get; set; } = new List<LineItem>();

        /// <summary>
        /// This property represents the TOTAL monetary amount associated with <see cref="Label"/> including <see cref="Amount"/> and the total of all <see cref="Sublines"/>.
        /// In other words, this property is used to model subtotals that appear in a Balance Sheet.
        /// </summary>
        public LedgerAmount Total {
            get {
                LedgerAmount ledger = LedgerAmount.Zero;
                GetSublinesAmount(ref ledger, this.Sublines);
                return ledger;
            }
        }

        /// <summary>
        /// Helper function to retrieve the total <see cref="Label"/> including <see cref="Amount"/> and the total of all <see cref="Sublines"/>.
        /// In other words, this property is used to model subtotals that appear in a Balance Sheet.
        /// </summary>
        private void GetSublinesAmount(ref LedgerAmount ledger, List<LineItem> sublines){
            // Not checking for null base case because sublines will always be instantiated to an
            // empty list
            foreach (LineItem item in sublines){
                // Not checking for a null pointer since Amount is always initialized to zero LedgerAmount
                ledger = ledger + item.Amount;
                GetSublinesAmount(ref ledger, item.Sublines);
            }
        }

        /// <summary>
        /// This function searches for a LineItem with a specific label.
        /// </summary>
        /// <returns> The found line item </returns>
        public static LineItem FindLineItem(String label, LineItem item) {
            if(item.Label == null)
            {
                return null;
            }

            Queue<LineItem> q = new Queue<LineItem>();

            q.Enqueue(item);

            while (q.Count != 0)
            {
                LineItem queueItem = q.Dequeue();
                // Match - return that LineItem
                if (queueItem.Label == label)
                {
                    return queueItem;
                }

                queueItem.Sublines.ForEach(li => q.Enqueue(li));
            }

            // Didn't find a match for any of the child LineItems
            return null;    
        }
    }
}
