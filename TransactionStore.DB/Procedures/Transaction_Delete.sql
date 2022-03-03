CREATE proc dbo.Transaction_Delete
			@Id int
AS
BEGIN
	delete from dbo.[Transaction]
	where Id = @id
END