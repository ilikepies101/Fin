using System;
using System.Threading.Tasks;

namespace Interview.Services
{
    /// <summary>
    /// An interface for a service that implements business logic related to balance sheets.
    /// </summary>
    public interface IBalanceSheetService
    {
        /// <summary>
        /// Looks up the amount of a specific line item that appears in a balance sheet.
        /// </summary>
        /// <param name="balanceSheetYear">The year of the balance sheet for which we want an amount.</param>
        /// <param name="balanceSheetMonth">The month of the balance sheet for which we want an amount.</param>
        /// <param name="lineItemId">The id of the line item to locate in the balance sheet.</param>
        /// <returns>The amount of the line item in the balance sheet with the requested year and month.</returns>
        Task<(bool Found, LedgerAmount Amount)> GetLineItemTotal(
            int balanceSheetYear,
            int balanceSheetMonth,
            string lineItemId);

        /// <summary>
        /// Returns the trial balance amount for the balance sheet having <paramref name="balanceSheetYear"/> and <paramref name="balanceSheetMonth"/>.
        /// </summary>
        /// <param name="balanceSheetYear">The year of the balance sheet for which we want a trial balance.</param>
        /// <param name="balanceSheetMonth">The month of the balance sheet for which we want a trial balance.</param>
        Task<(bool Found, LedgerAmount Amount)> GetTrialBalance(
            int balanceSheetYear,
            int balanceSheetMonth);

        /// <summary>
        /// Persist a balance sheet to storage. Use the Year and Month of the Balance Sheet to read
        /// it back from storage (<seealso cref="IBalanceSheetStore"/>.
        /// </summary>
        /// <param name="balanceSheet">The balance sheet to save to persistent storage.</param>
        Task Save(BalanceSheet balanceSheet);
    }
}
