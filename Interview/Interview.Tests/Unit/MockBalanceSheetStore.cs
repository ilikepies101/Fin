using System;
using System.Threading.Tasks;
using Interview.Services;

namespace Interview.Tests.Unit
{
    public class MockBalanceSheetStore : BalanceSheetsServiceTests, IBalanceSheetStore
    {

        public MockBalanceSheetStore()
        {

        }

        async Task<(bool Exists, BalanceSheet Result)> IBalanceSheetStore.Get(int Year, int Month)
        {
            // Simulate delay to reach to Store
            await Task.Delay(10);
            return (true, TestBalanceSheet);
        }

        Task IBalanceSheetStore.Store(BalanceSheet balanceSheet)
        {
            throw new NotImplementedException();
        }
    }
}
