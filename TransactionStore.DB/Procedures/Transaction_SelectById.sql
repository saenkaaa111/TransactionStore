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
	from dbo.[Transaction] t inner join dbo.[Account] a on t.AccountId = a.Id 
	where t.Id = @Id						
END