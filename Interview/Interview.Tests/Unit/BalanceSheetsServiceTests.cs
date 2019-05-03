using FluentAssertions;
using Interview.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using Interview.Tests.Unit.HelperObjects;

namespace Interview.Tests.Unit
{
    [TestClass]
    public class BalanceSheetsServiceTests
    {

        BalanceSheetsService uut;
        MockBalanceSheetStore mockStore;
        /// <summary>
        /// Setup the objects used for asserts
        /// </summary
        [TestInitialize]
        public void Init()
        {
            mockStore = new MockBalanceSheetStore(TestBalanceSheet.BalanceSheet);
            uut = new BalanceSheetsService(mockStore);
        }

        /// <summary>
        /// A method that computes the trial balance
        /// </summary>
        [TestMethod]
        public async Task BalanceSheetsService_CanComputeTrialBalance_IsZero()
        {
            // execute
            var result = await uut.GetTrialBalance(It.IsAny<int>(), It.IsAny<int>());

            // verify
            result.Amount.Should().Be(LedgerAmount.Zero);
            result.Found.Should().BeTrue();
        }

        /// <summary>
        /// A method that computes the trial balance with an unbalanced
        /// balance sheet
        /// </summary>
        [TestMethod]
        public async Task BalanceSheetsService_CanComputeTrialBalance_IsNonZero()
        {
            // setup with unbalanced sheet passed into mock
            mockStore = new MockBalanceSheetStore(TestBalanceSheet_Unbalanced.BalanceSheet);
            uut = new BalanceSheetsService(mockStore);

            // execute
            var result = await uut.GetTrialBalance(It.IsAny<int>(), It.IsAny<int>());

            // verify
            result.Amount.Should().NotBe(LedgerAmount.Zero);
            result.Found.Should().BeTrue();
        }

        /// <summary>
        /// Tests that verify a line item is found with the correct ledger amoutn
        /// </summary>
        /// <param name="lineItemLabel"></param>
        /// <param name="expectedCreditOrDebit"></param>
        /// <param name="expectedAmount"></param>
        /// <returns></returns>
        [TestMethod]
        [DataRow("Assets", CreditOrDebit.Debit, 1_080_000)]
        [DataRow("Current Assets", CreditOrDebit.Debit, 155_000)]
        [DataRow("Accounts Receivable", CreditOrDebit.Debit, 50_000)]
        [DataRow("Liabilities and Equity", CreditOrDebit.Credit, 1_080_000)]
        [DataRow("Accounts Payable", CreditOrDebit.Credit, 20_000)]
        public async Task BalanceSheetsService_CanReadLineItemAmount(
            string lineItemLabel,
            CreditOrDebit expectedCreditOrDebit,
            double expectedAmount)
        {
            // execute
            (bool Found, LedgerAmount Amount) result = await uut.GetLineItemTotal(TestBalanceSheet.BalanceSheet.AsOf.Year, TestBalanceSheet.BalanceSheet.AsOf.Month, lineItemLabel);

            // verify
            result.Found.Should().BeTrue($"because {lineItemLabel} appears in the test balance sheet.");
            result.Amount
                .Should()
                .Be(LedgerAmount.FromFloat(expectedAmount, expectedCreditOrDebit), $"because the expected total amount of {lineItemLabel} is {expectedAmount} {expectedCreditOrDebit}.");
        }
    }
}
