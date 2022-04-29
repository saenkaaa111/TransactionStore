CREATE PROCEDURE [dbo].[Transaction_SelectById]
			@Id bigint
AS
BEGIN
SELECT
		Id,
		Amount,
		Date,
		Type, 
		AccountId,
		Currency
	FROM dbo.[Transaction]
	WHERE Id = @Id						
END