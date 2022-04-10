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
	) q ORDER BY q.Date desc
END