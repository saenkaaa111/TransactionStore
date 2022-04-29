CREATE PROCEDURE [dbo].[Transaction_Transfer]
	@AccountIdFrom			int,
	@AccountIdTo			int,
	@Amount					decimal (9, 2),
	@ConvertedAmount		decimal (9, 2),
	@CurrencyFrom			tinyint,
	@CurrencyTo				tinyint,
	@Type                   tinyint
AS
BEGIN
		DECLARE
		@CurrentDate datetime2 = getdate()

		INSERT INTO [dbo].[Transaction] (AccountId, Amount, Currency, [Type], [Date])
		VALUES (@AccountIdFrom, @Amount, @CurrencyFrom, @Type, @CurrentDate)
		DECLARE @TransactionIdFrom bigint = @@IDENTITY
		
				
		INSERT INTO [dbo].[Transaction] (AccountId, Amount, Currency, [Type], [Date])
		VALUES (@AccountIdTo, @ConvertedAmount, @CurrencyTo, @Type, @CurrentDate)
		DECLARE @TransactionIdTo bigint = @@IDENTITY
		
		SELECT @TransactionIdFrom, @TransactionIdTo
END
