CREATE PROCEDURE [dbo].[Transaction_GetAccountBalance]
	@Id int
	
AS
Begin
 	
	select Amount, Date from(SELECT 	
	[Id] = 1,
	[Amount] = Sum(t.Amount)
	FROM [dbo].[Transaction] t
	where @Id = t.AccountId ) g left join  
	(
	
	SELECT 	
	[Id] = 1,
	[Date] = Max(r.Date)
	FROM [dbo].[Transaction] r) m on g.Id = m.Id

End