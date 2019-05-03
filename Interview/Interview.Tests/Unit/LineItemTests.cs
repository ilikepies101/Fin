using FluentAssertions;
using Interview.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Interview.Services;
using System.Linq;
using Interview.Tests.Unit.HelperObjects;

namespace Interview.Tests.Unit
{
    [TestClass]
    public class LineItemTests
    {
        /// <summary>
        /// Verifies the total amount based for the TestBalanceSheet by manually summing up
        /// the expected value from the TestBalanceSheet.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void LineItem_CanGetTotalForLineItem()
        {
            // Verify assets line items ledger amount
            var lineItem = TestBalanceSheet.BalanceSheet.LineItems.First(li => li.Label.Equals("Assets"));
            var expectedTotal = 50_000 + 100_000 + 5_000 + 45_000 - 20_000 + 1_000_000 - 100_000;
            LedgerAmount expectedLedger = new LedgerAmount(expectedTotal, CreditOrDebit.Debit);
            expectedLedger.Should().Be(lineItem.Total);
        }
    }
}
