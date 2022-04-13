using FluentValidation;
using Marvelous.Contracts.RequestModels;

namespace TransactionStore.API.Validation
{

    public class TransactionRequestModelValidator : AbstractValidator<TransactionRequestModel>
    {
        public TransactionRequestModelValidator()
        {
            RuleFor(x => x.Amount).NotNull().ExclusiveBetween(0.0m, 100000m);
            RuleFor(x => x.AccountId).NotNull();
            RuleFor(x => x.Currency).NotNull().IsInEnum();
        }
    }
}
