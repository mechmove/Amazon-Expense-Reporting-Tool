# Amazon-Expense-Reporting-tool
Ever wondered how much money you are spending on Amazon Prime? Easy, just go to “Your Account › Your Payments › Transactions”. Unfortunately, Amazon seems to have removed reports that provide sum totals. This is a small program to parse sequential data saved as a text file and produce an organized CSV file for numerical analysis. You must create the text file by "screen scraping" your raw transactions. Please see sample "screen scrape, Amazon Orders.txt", place this file into your \Amazon Expense Reporting Tool\bin\Debug\net6.0 folder to produce output files. There is no GUI. If you find an on-line report, please let me know, many times I end up writing stuff that is redundant and unnecessary, but it keeps my programming skills going.

# Important notes:

11/4/2023 The basic sequential structure is date, amount, order number, and vendor. Transactions earlier than 07/28/2020 omits vendor. In those cases, I just key-in "placeholder vendor" as to not have to accomodate an older format. The available transactions are limited, so download and save your transactions every year!

11/3/2023 Project creation
