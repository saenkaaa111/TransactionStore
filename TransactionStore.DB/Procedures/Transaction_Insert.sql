CREATE proc dbo.Transaction_Insert
			@Amount int, 
			@AccountId int,
			@Type int,
			@Currency int
AS
BEGIN
	insert into dbo.[Transaction] (Date, Amount, AccountId, Type, Currency) 
	values (GETDATE(), @Amount, @AccountId, @Type, @Currency);
	select scope_identity()
END