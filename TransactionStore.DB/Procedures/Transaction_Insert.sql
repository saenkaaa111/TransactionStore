CREATE PROCEDURE [dbo].[Transaction_Insert]
			@Amount int, 
			@AccountId int,
			@Type int,
			@Currency int,
			@Date datetime
AS
BEGIN
	IF (SELECT Date 
	FROM [dbo].[Transaction]
	where Id=(select max(Id) from [dbo].[Transaction] where AccountID = @AccountId )) != @Date
		RAISERROR('Flow crossing',16,1)
	ELSE		
		insert into dbo.[Transaction] (Date, Amount, AccountId, Type, Currency) 
		values (GETDATE(), @Amount, @AccountId, @Type, @Currency)
		select scope_identity()		
END