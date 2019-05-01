using System;
using System.Threading.Tasks;

namespace Interview.Services.Implementation
{
    /// <summary>
    /// An implementation of <see cref="IBalanceSheetService"/>.
    /// </summary>
    public class BalanceSheetsService : IBalanceSheetService
    {
        // TODO. This service has a dependency on IBalanceSheetStore

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.GetLineItemTotal"/>.
        /// </summary>
        public async Task<(bool Found, LedgerAmount Amount)> GetLineItemTotal(
            int balanceSheetYear,
            int balanceSheetMonth,
            string lineItemLabel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.GetTrialBalance"/>.
        /// </summary>
        public async Task<(bool Found, LedgerAmount Amount)> GetTrialBalance(int balanceSheetYear, int balanceSheetMonth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.Save"/>.
        /// </summary>
        public async Task Save(BalanceSheet balanceSheet)
        {
            throw new NotImplementedException();
        }
    }
}
