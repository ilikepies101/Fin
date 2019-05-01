using Interview.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Interview.Api.Controllers
{
    /// <summary>
    /// A simple Web API for working with balance sheets.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class BalanceSheetsController : ControllerBase
    {
        /// <summary>
        /// Constructs this controller from its dependencies (injected during Startup.cs)
        /// </summary>
        /// <param name="balanceSheets">A service implementing the business logic that power's this API.</param>
        public BalanceSheetsController(IBalanceSheetService balanceSheets)
        {
            BalanceSheets = balanceSheets ?? throw new ArgumentNullException(nameof(balanceSheets));
        }

        /// <summary>
        /// A service implementing the business logic that power's this API.
        /// </summary>
        protected IBalanceSheetService BalanceSheets { get; set; }

        // GET /balancesheets/amount?year=,month=,lineItemId=
        [HttpGet("amount"), Produces(typeof(LedgerAmount))]
        public async Task<IActionResult> GetAmount(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromQuery] string lineItemId)
        {
            var amount = await BalanceSheets.GetLineItemTotal(year, month, lineItemId);

            return Ok(amount);
        }

        // GET /balancesheets/trialbalance?year=,month=
        [HttpGet("trialbalance"), Produces(typeof(LedgerAmount))]
        public async Task<IActionResult> GetTrialBalance(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromQuery] string lineItemId)
        {
            var trialBalance = await BalanceSheets.GetTrialBalance(year, month);

            return Ok(trialBalance);
        }

        // PUT /balancesheets
        [HttpPut]
        public async Task<IActionResult> Upload([FromBody] BalanceSheet balanceSheet)
        {
            await BalanceSheets.Save(balanceSheet);

            return NoContent();
        }
    }
}
