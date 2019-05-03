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
        /// Setup - objects used in the tests
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            mockStore = new MockBalanceSheetStore(TestBalanceSheet.BalanceSheet, true);
            uut = new BalanceSheetsService(mockStore);
        }

        // <summary>
        /// A test that verifies a zero ledger amount is returned if result is flagged as not found
        /// </summary>
        [TestMethod]
        public async Task BalanceSheetsService_CanComputeTrialBalance_NotFound()
        {
            // setup store so balance sheet is not found
            mockStore = new MockBalanceSheetStore(TestBalanceSheet_Unbalanced.BalanceSheet, false);
            uut = new BalanceSheetsService(mockStore);

            // execute
            var result = await uut.GetTrialBalance(It.IsAny<int>(), It.IsAny<int>());

            // verify
            result.Amount.Should().Be(LedgerAmount.Zero, "because we default to a zero LedgerAmount.");
            result.Found.Should().BeFalse("because our mocked store did not find the resul.");
        }

        /// <summary>
        /// A method that verifies the trial balance is zero for a balanced balance sheet
        /// </summary>
        [TestMethod]
        public async Task BalanceSheetsService_CanComputeTrialBalance_IsZero()
        {
            // execute
            var result = await uut.GetTrialBalance(It.IsAny<int>(), It.IsAny<int>());

            // verify
            result.Amount.Should().Be(LedgerAmount.Zero, "because our balance sheet is balanced.");
            result.Found.Should().BeTrue("because our mocked store found the result");
        }

        /// <summary>
        /// Test to verify trial balance with an unbalanced balanced sheet
        /// </summary>
        [TestMethod]
        public async Task BalanceSheetsService_CanComputeTrialBalance_IsNonZero()
        {
            // setup with unbalanced sheet passed into mock
            mockStore = new MockBalanceSheetStore(TestBalanceSheet_Unbalanced.BalanceSheet, true);
            uut = new BalanceSheetsService(mockStore);

            // execute
            var result = await uut.GetTrialBalance(It.IsAny<int>(), It.IsAny<int>());

            // verify
            result.Amount.Should().NotBe(LedgerAmount.Zero, "because our balance sheet is unbalanced.");
            result.Found.Should().BeTrue("because our mocked store found the result.");
        }

        /// <summary>
        /// Tests that verify a line item is found with the correct ledger amount
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

        /// <summary>
        /// Tests that verify a line item is not found with a row that doesn't exist in the
        /// balance sheet. 
        /// </summary>
        /// <param name="lineItemLabel"></param>
        /// <returns></returns>
        [TestMethod]
        [DataRow("Made up thing")]
        public async Task BalanceSheetsService_CanReadLineItemAmount_DoesNotExist(
            string lineItemLabel)
        {
            // execute
            (bool Found, LedgerAmount Amount) result = await uut.GetLineItemTotal(TestBalanceSheet.BalanceSheet.AsOf.Year, TestBalanceSheet.BalanceSheet.AsOf.Month, lineItemLabel);

            // verify
            result.Found.Should().BeFalse($"because {lineItemLabel} does not appear in the test balance sheet.");
            result.Amount
                .Should()
                .Be(LedgerAmount.Zero, $"because there is no ledger amount associated with the non existing LineItem.");
        }
    }
}
