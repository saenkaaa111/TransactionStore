using FluentValidation;
using FluentValidation.Results;
using Marvelous.Contracts.RequestModels;

namespace TransactionStore.API.Validators
{

    public class TransactionRequestModelValidator : AbstractValidator<TransactionRequestModel>
    {
        public TransactionRequestModelValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("Amount is empty")
                .ExclusiveBetween(0.0m, 100000m)
                .WithMessage("Amount is less than 0 and more than 99999.99"); 

            RuleFor(x => x.AccountId)
                .NotNull()
                .WithMessage("AccountId is empty")
                .GreaterThan(0)
                .WithMessage("AccountId is less than 0");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency is empty")
                .IsInEnum();
        }

        public override ValidationResult Validate(ValidationContext<TransactionRequestModel> context)
        {
            return context.InstanceToValidate == null
                ? new ValidationResult(new[] { new ValidationFailure(nameof(TransferRequestModel), 
                "TransactionRequestModel cannot be null") }) : base.Validate(context);
        }
    }
}
