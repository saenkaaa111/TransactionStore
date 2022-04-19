CREATE PROCEDURE [dbo].[Transaction_GetLastDate]
	AS
Begin
	
	SELECT Date 
	FROM [dbo].[Transaction]
	where Id=(select max(Id) from [dbo].[Transaction])

END