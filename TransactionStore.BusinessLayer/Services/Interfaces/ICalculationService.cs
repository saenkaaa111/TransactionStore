namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        decimal ConvertCurrency();
        decimal GetAccountBallance();
    }
}