CREATE proc dbo.Transaction_SelectAll
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
						
END