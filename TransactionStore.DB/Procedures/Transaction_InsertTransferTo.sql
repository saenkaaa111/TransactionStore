CREATE PROCEDURE [dbo].[Transaction_InsertTransferTo]
			@Date datetime2, 
			@Amount int, 
			@AccountId int,
			@Type int,
			@Currency int
AS
BEGIN
	insert into dbo.[Transaction] (Date, Amount, AccountId, Type, Currency) 
	values (@Date, @Amount, @AccountId, @Type, @Currency);
	select scope_identity()
END