CREATE proc [dbo].[Transaction_SelectByAccountId]
			@AccountId int
AS
BEGIN
SELECT
		t.Id,
		t.Amount,
		t.Date ,
		t.Type, 
		t.AccountId,
		t.Currency,
		r.Id,
		r.Date,
		r.AccountId	
		
	from dbo.[Transaction] t left JOIN [Transaction] r on r.Date = t.Date
	where t.AccountId = @AccountId or
	(t.AccountId = @AccountId and t.Type = 3) and
	(r.AccountId != @AccountId and r.Type = 3);
	
END
