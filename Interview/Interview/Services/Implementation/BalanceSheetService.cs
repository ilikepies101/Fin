using System;
using System.Threading.Tasks;
using System.Linq;

namespace Interview.Services.Implementation
{
    /// <summary>
    /// An implementation of <see cref="IBalanceSheetService"/>.
    /// </summary>
    public class BalanceSheetsService : IBalanceSheetService
    {
        // TODO. This service has a dependency on IBalanceSheetStore
        private readonly IBalanceSheetStore store;

        public BalanceSheetsService(IBalanceSheetStore store){
            this.store = store;
        }

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.GetLineItemTotal"/>.
        /// </summary>
        public async Task<(bool Found, LedgerAmount Amount)> GetLineItemTotal(
            int balanceSheetYear,
            int balanceSheetMonth,
            string lineItemLabel)
        {
            // //TODO - make the exists and balance sheet result its own class
            // (bool Exists, BalanceSheet Result) tuple = await this.store.Get(balanceSheetYear, balanceSheetMonth);
            // if(tuple.Exists){
            //     //LedgerAmount ledger = new LedgerAmount(0, CreditOrDebit.Zero);
            //     //this.CalculateLedgerSum(Result.LineItems, ledger);

            //     //LedgerAmount ledger = new LedgerAmount(sum, );
            //}
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.GetTrialBalance"/>.
        /// </summary>
        public async Task<(bool Found, LedgerAmount Amount)> GetTrialBalance(int balanceSheetYear, int balanceSheetMonth)
        {
            LedgerAmount ledger = LedgerAmount.Zero;
            (bool Found, BalanceSheet Result) trialBalanceTask = await this.store.Get(balanceSheetYear, balanceSheetMonth);          
            
            //TODO - wrap in try catch
            if(trialBalanceTask.Found && trialBalanceTask.Result != null){
                foreach (LineItem item in trialBalanceTask.Result.LineItems)
                {
                    // TODO - implement += in ledger amount class
                    ledger = ledger + item.Amount;
                }
            }

            return (trialBalanceTask.Found, ledger);
        }

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.Save"/>.
        /// </summary>
        public async Task Save(BalanceSheet balanceSheet)
        {
            try {
                await this.store.Store(balanceSheet);
            } catch (Exception e){
                // Log exception
                Console.WriteLine("Encountered an exception {0} saving the balance sheet with AsOf Date {1}", e, balanceSheet.AsOf);
            }
        }
    }
}
