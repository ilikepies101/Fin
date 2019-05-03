using System;
using System.Threading.Tasks;
using Interview.Services;
using Interview.Tests.Unit.HelperObjects;

namespace Interview.Tests.Unit
{
    public class MockBalanceSheetStore : IBalanceSheetStore
    {

        public MockBalanceSheetStore()
        {

        }

        async Task<(bool Exists, BalanceSheet Result)> IBalanceSheetStore.Get(int Year, int Month)
        {
            // Simulate delay to reach to Store
            await Task.Delay(10);
            return (true, TestBalanceSheet.BalanceSheet);
        }

        Task IBalanceSheetStore.Store(BalanceSheet balanceSheet)
        {
            throw new NotImplementedException();
        }
    }
}
