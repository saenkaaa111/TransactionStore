CREATE proc dbo.Transaction_Insert
			@Date date, 
			@Amount int, 
			@AccountId int,
			@Type int
AS
BEGIN
	insert into dbo.[Transaction] (Date, Amount, AccountId, Type) 
	values (@Date, @Amount, @AccountId, @Type);
	select scope_identity()
END