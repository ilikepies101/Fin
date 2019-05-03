using System;
using System.Collections.Generic;

namespace Interview.Tests.Unit.HelperObjects
{
    /// <summary>
    /// A helper class to retrieve te test balance sheet
    /// </summary>
    public static class TestBalanceSheet
    {
        /// <summary>
        /// An example of what a balance sheet will look like for testing purposes.
        /// </summary>
        public static BalanceSheet BalanceSheet = new BalanceSheet
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
    }
}
