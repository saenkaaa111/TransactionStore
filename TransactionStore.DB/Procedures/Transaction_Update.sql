CREATE proc dbo.Transaction_Update
			@Id int,
			@Amount int,
			@Type int,
			@AccountId int,
			@Date datetime
AS
BEGIN
	UPDATE dbo.[Transaction] Set Amount = @Amount,
	Type = @Type, AccountId = @AccountId, Date = @Date
	where Id = @id
END