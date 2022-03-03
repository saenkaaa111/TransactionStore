CREATE proc dbo.Transaction_Insert
			@Date date, 
			@Amount int, 
			@AccountId int 
AS
BEGIN
	insert into dbo.[Transaction] (Date, Amount, AccountId) 
	values (@Date, @Amount, @AccountId);
END