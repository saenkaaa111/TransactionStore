CREATE PROCEDURE [dbo].[Transaction_InsertTransferFrom]
			@Amount int, 
			@AccountId int,
			@Type int,
			@Currency int
AS
BEGIN
	insert into dbo.[Transaction] (Date, Amount, AccountId, Type, Currency) 
	values (GETDATE(), @Amount, @AccountId, @Type, @Currency);
	select GETDATE()
END
