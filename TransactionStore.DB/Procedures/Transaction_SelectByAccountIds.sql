CREATE PROCEDURE [dbo].[Transaction_SelectByAccountIds]
	@Ids int
AS 
BEGIN
SELECT 	
		Id,
		Amount,
		Date,
		Type, 
		AccountId,
		Currency

	FROM [dbo].[Transaction] 
	WHERE AccountId IN (SELECT ID FROM FnSplitter(@Ids))
END