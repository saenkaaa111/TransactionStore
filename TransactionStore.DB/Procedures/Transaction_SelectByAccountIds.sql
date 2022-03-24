CREATE PROCEDURE [dbo].[Transaction_SelectByAccountIds]
	@tvp [dbo].[AccountTVP] readonly
AS 
BEGIN
SELECT 	
		Id,
		Amount,
		Date,
		Type, 
		AccountId,
		Currency
		FROM [dbo].[Transaction] t
    INNER JOIN @tvp i ON t.AccountId = i.AccountIds
END