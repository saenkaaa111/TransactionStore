CREATE proc dbo.Transaction_SelectById
			@Id int
AS
BEGIN
SELECT
		t.Id,
		t.Amount,
		t.Date,
		t.Type,
		a.Id,
		a.Name
	FROM dbo.[Transaction]
	WHERE t.Id = @Id						
END