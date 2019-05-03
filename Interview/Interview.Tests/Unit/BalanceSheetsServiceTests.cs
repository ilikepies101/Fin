using FluentAssertions;
using Interview.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Interview.Services;
using System.Linq;

namespace Interview.Tests.Unit
{
    [TestClass]
    public class BalanceSheetsServiceTests
    {
        /// <summary>
        /// An example of what a balance sheet will look like for testing purposes.
        /// </summary>
        protected static BalanceSheet TestBalanceSheet = new BalanceSheet
        {
            AsOf = (2019, 4),
            LineItems = new List<LineItem>
            {
                new LineItem
                {
                    Label = "Assets",
                    Sublines = new List<LineItem>
                    {
                        new LineItem
                        {
                            Label = "Current Assets",
                            Sublines = new List<LineItem>
                            {
                                new LineItem
                                {
                                    Label = "Accounts Receivable",
                                    Amount = new LedgerAmount(50_000, CreditOrDebit.Debit),
                                },
                                new LineItem
                                {
                                    Label = "Checking Account",
                                    Amount = new LedgerAmount(100_000, CreditOrDebit.Debit),
                                },
                                new LineItem
                                {
                                    Label = "Undeposited Funds",
                                    Amount = new LedgerAmount(5_000, CreditOrDebit.Debit),
                                },
                            },
                        },
                        new LineItem
                        {
                            Label = "Long-Term Assets",
                            Sublines = new List<LineItem>
                            {
                                new LineItem
                                {
                                    Label = "Trucks",
                                    Amount = new LedgerAmount(45_000, CreditOrDebit.Debit),
                                },
                                new LineItem
                                {
                                    Label = "Accumulated Depreciation Of Trucks",
                                    Amount = new LedgerAmount(20_000, CreditOrDebit.Credit),
                                },
                                new LineItem
                                {
                                    Label = "Office Building",
                                    Amount = new LedgerAmount(1_000_000, CreditOrDebit.Debit),
                                },
                                new LineItem
                                {
                                    Label = "Accumulated Depreciation Of Office Building",
                                    Amount = new LedgerAmount(100_000, CreditOrDebit.Credit),
                                },
                            },
                        },
                    },
                },
                new LineItem
                {
                    Label = "Liabilities and Equity",
                    Sublines = new List<LineItem>
                    {
                        new LineItem
                        {
                            Label = "Liabilities",
                            Sublines = new List<LineItem>
                            {
                                new LineItem
                                {
                                    Label = "Current Liabilities",
                                    Sublines = new List<LineItem>
                                    {
                                        new LineItem
                                        {
                                            Label = "Accounts Payable",
                                            Amount = new LedgerAmount(20_000, CreditOrDebit.Credit),
                                        },
                                    },
                                },
                                new LineItem
                                {
                                    Label = "Long-Term Liabilities",
                                    Sublines = new List<LineItem>
                                    {
                                        new LineItem
                                        {
                                            Label = "Office Building Loan",
                                            Amount = new LedgerAmount(250_000, CreditOrDebit.Credit),
                                        },
                                    },
                                },
                            },
                        },
                        new LineItem
                        {
                            Label = "Equity",
                            Sublines = new List<LineItem>
                            {
                                new LineItem
                                {
                                    Label = "Retained Earnings",
                                    Amount = new LedgerAmount(100_000, CreditOrDebit.Credit),
                                },
                                new LineItem
                                {
                                    Label = "Current Year Earnings",
                                    Amount = new LedgerAmount(500_000, CreditOrDebit.Credit),
                                },
                                new LineItem
                                {
                                    Label = "Common Stock",
                                    Amount = new LedgerAmount(200_000, CreditOrDebit.Credit),
                                },
                                new LineItem
                                {
                                    Label = "Preferred Stock",
                                    Amount = new LedgerAmount(10_000, CreditOrDebit.Credit),
                                },
                            },
                        },
                    },
                },
            },
        };

        /// <summary>
        /// TODO...
        /// </summary>
        [TestMethod]
        public async Task BalanceSheetsService_CanComputeTrialBalance()
        {
            // setup
            MockBalanceSheetStore mockStore = new MockBalanceSheetStore();
            var uut = new BalanceSheetsService(mockStore);

            // execute
            var result = await uut.GetTrialBalance(It.IsAny<int>(), It.IsAny<int>());

            // verify
            result.Amount.Should().Be(LedgerAmount.Zero);
            result.Found.Should().BeTrue();
        }

        /// <summary>
        /// Tests that 
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
            // setup
            MockBalanceSheetStore mockStore = new MockBalanceSheetStore();
            var uut = new BalanceSheetsService(mockStore);

            // execute
            var result = await uut.GetLineItemTotal(TestBalanceSheet.AsOf.Year, TestBalanceSheet.AsOf.Month, lineItemLabel);

            // verify
            result.Found.Should().BeTrue($"because {lineItemLabel} appears in the test balance sheet.");
            result.Amount
                .Should()
                .Be(LedgerAmount.FromFloat(expectedAmount, expectedCreditOrDebit), $"because the expected total amount of {lineItemLabel} is {expectedAmount} {expectedCreditOrDebit}.");
        }

        [TestInitialize]
        public void Init()
        {
        }

        /// <summary>
        /// Verifies the total amount based for the TestBalanceSheet by manually summing up
        /// the expected value from the TestBalanceSheet.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void CanGetTotalFromLineItem()
        {
            // TODO - make the expected total dynamic - based on test balance sheet if possible
            // Verify assets line items
            var lineItem = TestBalanceSheet.LineItems.First(li => li.Label.Equals("Assets"));
            var expectedTotal = 50_000 + 100_000 + 5_000 + 45_000 - 20_000 + 1_000_000 - 100_000;
            LedgerAmount expectedLedger = new LedgerAmount(expectedTotal, CreditOrDebit.Debit);
            expectedLedger.Should().Be(lineItem.Total);
        }
    }
}
