CREATE proc [dbo].[Transaction_SelectByAccountId]
			@AccountId int
AS
BEGIN
SELECT
		Id,
		Amount,
		Date ,
		Type, 
		AccountId,
		Currency
		
	from dbo.[Transaction]	
	where AccountId = @AccountId 
	union all
SELECT
		r.Id,
		r.Amount,
		r.Date ,
		r.Type, 
		r.AccountId,
		r.Currency
		
	from dbo.[Transaction] t inner JOIN [Transaction] r on r.Date = t.Date
	where (t.AccountId = @AccountId and t.Type = 3) and
	(r.AccountId != @AccountId and r.Type = 3);
	
END
