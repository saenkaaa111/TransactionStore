CREATE proc dbo.Transaction_SelectById
			@Id int
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