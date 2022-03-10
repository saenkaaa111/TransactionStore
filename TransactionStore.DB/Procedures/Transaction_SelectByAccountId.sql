CREATE proc [dbo].[Transaction_SelectByAccountId]
			@AccountId int
AS
BEGIN
SELECT
		t.Id,
		t.Amount,
		t.Date,
		t.Type, 
		t.AccountId,
		t.Currency,
		prewDate = t_lag.Date,
		nextDate = t_lead.Date
		
	from dbo.[Transaction] t 
	left join [Transaction] t_lag on t_lag.id = t.id - 1
	left join [Transaction] t_lead on t_lead.id = t.id + 1
	
	where t.AccountId = @AccountId or 
	(t.Type = 3 and 
	(t.Date =t_lag.Date or t.Date =t_lead.Date) and  
	(t.AccountId = @AccountId or t_lag.AccountId = @AccountId or t_lead.AccountId = @AccountId))

END
