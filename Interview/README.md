# Required Dependencies

[.NET Core Runtime 2.2.4](https://dotnet.microsoft.com/download)

# Descripton of work

Complete the implementation of a simple API that:

1. Supports uploading Balance Sheets.
2. Supports reading individual amounts from stored Balance Sheets.
3. Supports returning the Trial Balance amount for a Balance Sheet.
4. Can be integrated with a separately coded implemenation of `IBalanceSheetStore`. Don't bother writing a storage back-end yourself.

# High-level steps

**Implement `BalanceSheetService.Save`**
Assuming `IBalanceSheetStore` is being implemented by someone else, this should be very straightforward.
    
**Implement and test `BalanceSheetService.GetLineItemTotal`**
    1. Again, assume that `IBalanceSheetStore` will be implemented by someone else and that it will be available after an integration. Use that dependency to retreive the appropriate `BalanceSheet` by year and month.
    2. Implement a simple search routine that can locate a specific line item appearing within a Balance Sheet.
    3. Implement a getter for `LineItem.Total`, which should dynamically compute the total amount for a given line item appearing in a Balance Sheet. See [Balance Sheet](#BalanceSheet) for more context.
    4. Finish up the method by `return`ing the `Total` of the `LineItem` with a `Label` matching the method's parameter `lineItemLabel`.  
    5. The `BalanceSheetsService_CanReadLineItemAmount` unit test can now be used to verify your work.
    
**Implement `BalanceSheetService.GetTrialBalance`.**
    1. Still assuming that `IBalanceSheetStore` is being implemented by someone else, retreive a BalanceSheet by year and month.
    2. Write an algorithm to compute the Trial Balance for that Balance Sheet. Try to figure out how to do this on your own, but let me know if you need more information. See [Trial Balance](#TrialBalance) below for a bit more context. Hints: I'm looking for is a solution that doesn't depend on text `Label`s. There are at **least** two ways to do it.
    3. Write at least one unit test for this method. Stubbed out by `BalanceSheetsService_CanComputeTrialBalance`.
    
# Domain Models

I've designed this code problem to align with the type of data that we typically work with at Finagraph, namely Accounting data. Hopefully, it's not too exotic and I've provided enough clarity below where you don't have to be an Accountant to code up a solution. Again, let me know if you have questions.

Refer to *BalanceSheet.cs*, *LineItem.cs*, and *LedgerAmount.cs* to reference the concrete models used to represent a Balance Sheet.

## BalanceSheet

The `BalanceSheet` class is the primary domain model you'll be working with. It has just two propeties:

`AsOf` - This property tells you the year and month (Date) for which the Balance Sheet was prepared. The monetary amounts appearing in the Balance Sheet represent the balance of a each account appearing in the business's [Chart Of Accounts](#ChartOfAccounts) as of this date.
`LineItems` - This property contains a set of string labels and the monetary amounts associated with those labels. Essentially, it is a set of key value pairs like, for example, "Current Assets" -> $500. However, the set of `LineItems` are actually represented as a hierarchy/tree for two reasons:

1. The amounts in a Balance Sheet are categorized based on standard accounting practices. So, for example, the amount in the business's bank account is categorized under `Assets` and more specifically as a `Current Asset`. A tree structure allows this categorization relationship to be modeled.
2. Each categorical `LineItem` is also a subtotal. It has an amount that is equal to the sum of all `LineItems` appearing underneath it. Going back to the example just mentioned, this means that the `Total` of `Current Assets` will include the amount in the business's bank account as well as any other `Current Assets`. In other words, the hierarchical representation can also be interpreted as an addition tree. The intuition behind this is that a Balance Sheet is just a visual way of representing the *accounting equation*:
`Assets = Liabilities + Equity` or equivalently `Assets - Liabilities - Equity = 0`
That equation can be re-written using more granular categories:
`Current Assets + Long-Term Assets` = `Current Liabilities + Long-Term Liabilities + Equity`
And finally as individual accounts from the business's [Chart Of Accounts](#ChartOfAccounts).
`(Bank Checking + Bank Savings + Inventory + ...) + (Vehicles + Buildings + ...)` = ...

## LineItem

The `LineItem` class represents the set of label/amount pairs that appear in the Balance Sheet. It has the following properties:

`Label` - A text string that identifies the financial figure that appears in the Balance Sheet. To keep things simple, we're also going to treat this as a unique identifier for the `LineItem` in this exercise.

`Amount` - A monetary amount associated with the `Label` not including `Sublines`.

`Total` - The total monetary amount associated with the `Label`. IE the subtotal computed from the sum total of `Sublines` (recursive).

`Sublines` - A set of `LineItems` that have been categorized under this label and should be included in the `Total` (recursive).

## LedgerAmount

This `LedgerAmount` struct is used to represent monetary amounts as represented by an Accounting System. It has the following properties:

`Type` - Enum with values: Credit, Debit, or Zero. You can think of Credits and Debits as being positive or negative where the choice of sign doesn't impact calculations.

`Value` - A positive number representing the amount.

`+` and `-` operators are already defined for `LedgerAmount` so they should be fairly straight forward to calculate with them. The gist of how you operate with Credits and Debits is:

```
1 Credit + 1 Credit = 2 Credit
1 Debit + 1 Debit = 2 Debit
1 Credit + 1 Debit = 0
```

---

# Domain Concepts

## ChartOfAccounts

A Chart Of Accounts is simply a listing of text labels (called accounts) defined by the business's accountant that are used to categorize the financial financial figures that will appear in the Balance Sheet [and other financial statements].
Example Chart Of Accounts listing:
* Bank Checking - Money in the bank used for normal business activity.
* Undeposited Funds - Money received, but not yet deposited in the bank.
* Accounts Receivable - Invoices tracking sales made for which a payment has not yet been received.
* Inventory - Value of inventory owned.
* Vehicles - Value of vehicles owned.
* Buildings - Value of buildings owned.
* Accounts Payable - Invoices tracking expenses owed that have yet to be paid.
* Long-Term Loans - Amount remaining to pay off loans.
* Common Stock - Value of company's common share stock.
* Preferred Stock - Value of company's preferred stock.
* Current Year Earnings - Year-to-date earnings
...

Each account in the Chart Of Accounts can be thought of as a sub-category of one of the top-level sections that appear in a Balance Sheet [or in other financial statements]. Those top-level sections (Assets, Current Assets, Long-Term Assets, Liabilities, Current Liabilities, Long-Term Liabilities, Equity, ...) are applicable to just about any business and because of adherence to accounting standards, you'll typically spot them in just about any Balance Sheet you find. A Balance Sheet using the Chart Of Accounts listed above might look like:

```
+-------------------------+------------+--------+
| Assets                  | 1,080,000  | Debit  |
+-------------------------+------------+--------+
|  Current Assets         |    155,000 | Debit  |
+-------------------------+------------+--------+
|    Bank Checking        |    100,000 | Debit  |
+-------------------------+------------+--------+
|    Undeposited Funds    |      5,000 | Debit  |
+-------------------------+------------+--------+
|    Accounts Receivable  |     50,000 | Debit  |
+-------------------------+------------+--------+
|  Long-Term Assets       |    925,000 | Debit  |
+-------------------------+------------+--------+
|    Vehicles             |     45,000 | Debit  |
+-------------------------+------------+--------+
|    Buildings            |    880,000 | Debit  |
+-------------------------+------------+--------+
| Liabilities & Equity    |  1,080,000 | Credit |
+-------------------------+------------+--------+
|  Liabilities            |    270,000 | Credit |
+-------------------------+------------+--------+
|   Current Liabilities   |     20,000 | Credit |
+-------------------------+------------+--------+
|    Accounts Payable     |     20,000 | Credit |
+-------------------------+------------+--------+
|   Long-Term Liabilities |    250,000 | Credit |
+-------------------------+------------+--------+
|    Long-Term Loans      |    250,000 | Credit |
+-------------------------+------------+--------+
| Equity                  |    810,000 | Credit |
+-------------------------+------------+--------+
|  Current Year Earnings  |    600,000 | Credit |
+-------------------------+------------+--------+
|  Common Stock           |    200,000 | Credit |
+-------------------------+------------+--------+
|  Preferred Stock        |     10,000 | Credit |
+-------------------------+------------+--------+
```

>>>The above Balance Sheet example should be pretty close to what you'll find in the test class `BalanceSheetsServiceTests`. Look for a static property called `TestBalanceSheet`.

## Trial Balance

In the Balance Sheet example above, I added the Credit or Debit column to make it a bit more clear about how to compute a Trial Balance. As mentioned before, the Balance Sheet is simply a nice way to present the *accounting equation* to people.

`Assets = Liabilities + Equity`

A Trial Balance is essentially the assertion that if we add up all credits or debits in the Balance Sheet we will arrive at zero. The formulas that are often associated with a Trial Balance are:

`Assets + (-Liabilities) + (-Equity) = 0`?
i.e
`Assets + (-Liabilities) + (-Equity) = Trial Balance`

However, those equations are a tiny bit misleading though, because of the appearance of negative numbers (or equivalently subtraction). Normally, Assets have Debit amounts and Liabilities & Equity have Credit amounts so you can might be tempted to assign a [+/-] sign to those labels in the equation. In reality, there are often Assets with Credit amounts (think overdraft on bank account) and Liabilities and Equities with Debit amounts. So, a more practical way to think about how to compute a trial balance is simply:
```All Credits + All Debits = Trial Balance``` or ``Credits + Debits = 0``?
If you leverage `LedgerAmount` you can even think of the Trial Balance as:
```
Bank Checking + Undeposited Funds + ... + Preferred Stock + ... all accounts in the business's Chart Of Accounts listing
```






    




 
