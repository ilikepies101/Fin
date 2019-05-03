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
            (bool Exists, BalanceSheet Result) balanceSheetResult = await this.store.Get(balanceSheetYear, balanceSheetMonth);

            LedgerAmount totalLedger = LedgerAmount.Zero;
            if (balanceSheetResult.Exists && balanceSheetResult.Result != null)
            {
                LineItem item = null;
                foreach(LineItem lineItem in balanceSheetResult.Result.LineItems)
                { 
                    item = LineItem.FindLineItem(lineItemLabel, lineItem);
                    if(item != null)
                    {
                        break;
                    }
                }
                if(item != null)
                {
                    // We are at a leaf, so use amount as totalLedger
                    if(item.Sublines.Count == 0)
                    {
                        totalLedger = item.Amount;
                    }
                    // The line item contains children, so use total
                    else
                    {
                        totalLedger = item.Total;
                    }
                }
            }
            // TODO - create log class 
            else if (!balanceSheetResult.Exists)
            {
                Console.WriteLine(
                    "Warn - GetLineItemTotal did not find a balance sheet for year {0} and month {1}",
                    balanceSheetYear,
                    balanceSheetMonth);
            }
            else
            {
                Console.WriteLine("Error - GetLineItemTotal returned found for year {0} and month {1} but Result was null",
                    balanceSheetYear,
                    balanceSheetMonth);
            }

            return (balanceSheetResult.Exists, totalLedger);
        }

        /// <summary>
        /// An implementation of <see cref="IBalanceSheetService.GetTrialBalance"/>.
        /// </summary>
        public async Task<(bool Found, LedgerAmount Amount)> GetTrialBalance(int balanceSheetYear, int balanceSheetMonth)
        {
            LedgerAmount totalLedger = LedgerAmount.Zero;
            (bool Found, BalanceSheet Result) balanceSheetResult = await this.store.Get(balanceSheetYear, balanceSheetMonth);          
            
            //TODO - wrap in try catch
            if(balanceSheetResult.Found && balanceSheetResult.Result != null){
                foreach (LineItem item in balanceSheetResult.Result.LineItems)
                {
                    // TODO - implement += in ledger amount class
                    totalLedger = totalLedger + item.Total;
                }
            }
            else if (!balanceSheetResult.Found)
            {
                Console.WriteLine(
                    "Warn - GetTrialBalance did not find a balance sheet for year {0} and month {1}",
                    balanceSheetYear,
                    balanceSheetMonth);
            }
            else
            {
                Console.WriteLine("Error - GetTrialBalance returned found for year {0} and month {1} but Result was null",
                    balanceSheetYear,
                    balanceSheetMonth);
            }

            return (balanceSheetResult.Found, totalLedger);
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
