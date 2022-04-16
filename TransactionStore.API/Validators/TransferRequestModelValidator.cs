﻿using FluentValidation;
using Marvelous.Contracts.RequestModels;

namespace TransactionStore.API.Validators
{

    public class TransferRequestModelValidator : AbstractValidator<TransferRequestModel>
    {
        public TransferRequestModelValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Amount is empty")
                .ExclusiveBetween(0.0m, 100000m)
                .WithMessage("Amount is less than 0 and more than 99999.99");

            RuleFor(x => x.AccountIdFrom)
                .NotEmpty()
                .WithMessage("AccountIdFrom is empty")
                .GreaterThan(0)
                .WithMessage("AccountIdFrom is less than 0");

            RuleFor(x => x.AccountIdTo)
                .NotEmpty()
                .WithMessage("AccountId is empty")
                .GreaterThan(0)
                .WithMessage("AccountId is less than 0");

            RuleFor(x => x.CurrencyFrom)
                .NotEmpty()
                .WithMessage("CurrencyFrom is empty")
                .IsInEnum();

            RuleFor(x => x.CurrencyTo)
                .NotEmpty()
                .WithMessage("CurrencyTo is empty")
                .IsInEnum();
        }
    }
}
