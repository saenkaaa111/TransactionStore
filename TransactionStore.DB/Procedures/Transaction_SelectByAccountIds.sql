CREATE PROCEDURE [dbo].[Transaction_SelectByAccountIds]
	@tvp [dbo].[AccountTVP] readonly
AS
BEGIN
SELECT * FROM (
SELECT
		Id,
		Amount,
		Date ,
		Type, 
		AccountId,
		Currency
		
	from dbo.[Transaction]	t
	INNER JOIN @tvp i ON t.AccountId = i.AccountIds
	
	union all
SELECT
		r.Id,
		r.Amount,
		r.Date ,
		r.Type, 
		r.AccountId,
		r.Currency
		
	from dbo.[Transaction] t left JOIN [Transaction] r on r.Date = t.Date
	INNER JOIN @tvp i ON t.AccountId = i.AccountIds
	where (t.AccountId = i.AccountIds and t.Type = 3) and
	(r.AccountId != i.AccountIds and r.Type = 3)
	) q ORDER BY q.Date desc
END