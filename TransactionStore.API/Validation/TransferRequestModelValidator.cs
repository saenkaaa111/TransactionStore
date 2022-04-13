using FluentValidation;
using Marvelous.Contracts.RequestModels;

namespace TransactionStore.API.Validation
{

    public class TransferRequestModelValidator : AbstractValidator<TransferRequestModel>
    {
        public TransferRequestModelValidator()
        {
            RuleFor(x => x.Amount).NotNull().ExclusiveBetween(0.0m, 100000m);
            RuleFor(x => x.AccountIdFrom).NotNull().GreaterThan(0);
            RuleFor(x => x.AccountIdTo).NotNull().GreaterThan(0);
            RuleFor(x => x.CurrencyFrom).NotNull().IsInEnum();
            RuleFor(x => x.CurrencyTo).NotNull().IsInEnum();
        }
    }
}
