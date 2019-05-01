using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Interview
{
    /// <summary>
    /// A model of a Balance Sheet, otherwise known as a Statement of Financial Position.
    /// See https://en.wikipedia.org/wiki/Balance_sheet
    /// </summary>
    [DebuggerDisplay("Balance Sheet for {new System.DateTime(AsOf.Year, AsOf.Month, 1).ToString(\"MMMM\")} {AsOf.Year}")]
    public class BalanceSheet
    {
        /// <summary>
        /// The year and month of the balance sheet.
        /// </summary>
        public (int Year, int Month) AsOf { get; set; }

        /// <summary>
        /// The set of line items that make up the balance sheet represented as a hierarchy.
        /// That hierarchy can be represented/interpreted as an addition tree where the <see cref="LineItem.TotalAmount"/> of each node in the tree
        /// is simply the sum total of it's children.
        /// </summary>
        /// <example>
        ///  * Assets : 155,000 Debit + 925,000 Debit = 1,080,000 Debit
        ///    * Current Assets : 50,000 Debit + 100,000 Debit + 5,000 Debit = 155,000 Debit
        ///        * Accounts Receivable : 50,000 Debit
        ///        * Checking Account : 100,000 Debit
        ///        * Undeposited Funds : 5,000 Debit
        ///    * Long-Term Assets = 45,000 Debit + 20,000 Credit + 1,000,000 Debit + 100,000 Credit = 925,000 Debit
        ///        * Trucks : 45,000 Debit
        ///        * Accumulated Depreciation of Trucks : 20,000 Credit
        ///        * Office Building : 1,000,000 Debit
        ///        * Accumulated Depreciation of Office Building : 100,000 Credit
        ///  * Liabilities & Equity = 270,000 Credit + 810,000 Credit = 1,080,000 Credit
        ///     * Liabilities : 20,000 Credit + 250,000 Credit = 270,000 Credit
        ///         * Current Liabilities: 20,000 Credit = 20,000 Credit
        ///             * Accounts Payable : 20,000 Credit
        ///         * Long-term Liabilities: 250,000 Credit
        ///             * Office Building Loan : 250,000 Credit
        ///     * Equity : 100,000 Credit + 500,000 Credit + 200,000 Credit + 10,000 Credit = 810,000 Credit
        ///         * Retained Earnings : 100,000 Credit
        ///         * Current Year Earnings : 500,000 Credit
        ///         * Common Stock : 200,000 Credit
        ///         * Preferred Stock : 10,000 Credit
        /// </example>
        public List<LineItem> LineItems { get; set; } = new List<LineItem>();
    }
}
