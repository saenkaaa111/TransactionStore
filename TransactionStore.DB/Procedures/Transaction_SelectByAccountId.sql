CREATE proc [dbo].[Transaction_SelectByAccountId]
			@AccountId int
AS
BEGIN
SELECT
		Id,
		Amount,
		Date,
		Type, 
		AccountId,
		Currency
		
	from dbo.[Transaction]
	where AccountId = @AccountId						
END
