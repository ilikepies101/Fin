using System;
using System.Threading.Tasks;

namespace Interview.Services
{
    /// <summary>
    /// An interface for a service that is responsible for persistent storage of balance sheets.
    /// </summary>
    /// <remarks>The implementation is currently under development. May be backed by SQL, but still up in the air.</remarks>
    public interface IBalanceSheetStore
    {
        /// <summary>
        /// Get a balance sheet by year and month.
        /// </summary>
        /// <param name="Year">The balance sheet's year.</param>
        /// <param name="Month">The balance sheet's month.</param>
        /// <returns>A tuple describing if the balance sheet exists in storage and the balance sheet result if true.</returns>
        Task<(bool Exists, BalanceSheet Result)> Get(int Year, int Month);

        /// <summary>
        /// Store a balance sheet.
        /// </summary>
        /// <param name="balanceSheet">The balance sheet to persist to storage.</param>
        Task Store(BalanceSheet balanceSheet);
    }
}
