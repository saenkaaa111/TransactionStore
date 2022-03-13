CREATE proc dbo.Transaction_Transfer
	@AccountIdFrom			int,
	@AccountIdTo			int,
	@Amount					decimal (18, 4),
	@ConvertedAmount		decimal (18, 4),
	@CurrencyFrom			int,
	@CurrencyTo				int
AS
BEGIN
		DECLARE
		@CurrentDate datetime2 = getdate(),
		@Transfer int = 3

		INSERT INTO [dbo].[Transaction] (AccountId, Amount, Currency, [Type], [Date])
		VALUES (@AccountIdFrom, -@Amount, @CurrencyFrom, @Transfer, @CurrentDate)
		DECLARE @TransactionIdFrom int = @@IDENTITY
	
		INSERT INTO [dbo].[Transaction] (AccountId, Amount, Currency, [Type], [Date])
		VALUES (@AccountIdTo, @ConvertedAmount, @CurrencyTo, @Transfer, @CurrentDate)
		DECLARE @TransactionIdTo int = @@IDENTITY

		SELECT @TransactionIdFrom, @TransactionIdTo
END
