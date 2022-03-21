CREATE proc [dbo].[Transaction_SelectByAccountIdMinimal]
			@AccountId int
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
		
	from dbo.[Transaction]	
	where AccountId = @AccountId 
	) q ORDER BY q.Date desc
END
