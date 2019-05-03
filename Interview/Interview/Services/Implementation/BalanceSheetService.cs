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
        private readonly IBalanceSheetStore store;

        public BalanceSheetsService(IBalanceSheetStore store){
            if (store == null)
            {
                throw new ArgumentNullException("IBalanceSheetStore");
            }
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
            (bool Exists, BalanceSheet Result) balanceSheetResult = await this.store.Get(balanceSheetYear, balanceSheetMonth);

            LedgerAmount totalLedger = LedgerAmount.Zero;
            if (balanceSheetResult.Exists && balanceSheetResult.Result != null)
            {
                LineItem item = null;

                // Search for line item
                foreach(LineItem lineItem in balanceSheetResult.Result.LineItems)
                { 
                    item = LineItem.FindLineItem(lineItemLabel, lineItem);
                    // If we return non null, we found our item - so search can stop
                    if(item != null)
                    {
                        break;
                    }
                }

                // Calculate lineItem's LedgerAmount
                if (item != null)
                {
                    // We are at a leaf, so use amount as totalLedger
                    if(item.Sublines.Count == 0)
                    {
                        totalLedger = item.Amount;
                    }
                    // The line item contains children, so use Total
                    else
                    {
                        totalLedger = item.Total;
                    }
                }
            }
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

            if(balanceSheetResult.Found && balanceSheetResult.Result != null){
                foreach (LineItem item in balanceSheetResult.Result.LineItems)
                {
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
                Console.WriteLine("Error - Save encountered an exception {0} saving the balance sheet with AsOf Date {1}", e, balanceSheet.AsOf);
            }
        }
    }
}
