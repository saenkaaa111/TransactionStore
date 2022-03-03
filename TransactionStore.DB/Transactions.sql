CREATE TABLE [dbo].[Transactions]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Date] DATETIME NOT NULL, 
    [Amount] DECIMAL NOT NULL, 
    [AcountId] INT NOT NULL
)
