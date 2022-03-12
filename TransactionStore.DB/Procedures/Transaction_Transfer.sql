CREATE proc dbo.Transaction_Transfer
	@AccountId				int,
	@AccountIdTo			int,
	@Amount					decimal (18, 4),
	@AmountTo				decimal (18, 4),
	@Currency				int,
	@CurrencyTo				int,
	@Date					datetime2
AS
BEGIN
	IF @Date = (
		select 
		top (1) t.Date from dbo.[Transaction] as t
		where AccountId=@AccountId
		order by [Date] desc)

		DECLARE
		@CurrentDate datetime2 = getdate(),
		@Transfer int = 3

		INSERT INTO [dbo].[Transaction] (AccountId, Amount, Currency, [Type], [Date])
		VALUES (@AccountId, -@Amount, @Currency, @Transfer, @CurrentDate)
		DECLARE @TransactionIdFrom int = @@IDENTITY
	
		INSERT INTO [dbo].[Transaction] (AccountId, Amount, Currency, [Type], [Date])
		VALUES (@AccountIdTo, @AmountTo, @CurrencyTo, @Transfer, @CurrentDate)
		DECLARE @TransactionIdTo int = @@IDENTITY

		SELECT @TransactionIdFrom, @TransactionIdTo
END
