using FluentValidation.TestHelper;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using NUnit.Framework;
using TransactionStore.API.Validators;

namespace TransactionStore.API.Tests
{
    public class TransactionRequestModelValidatorTests
    {
        private TransactionRequestModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new TransactionRequestModelValidator();
        }

        [Test]
        public void TransactionRequestModel_IsValid_ValidationPassed()
        {
            //given
            var transaction = new TransactionRequestModel
            {
                Amount = 45,
                AccountId = 2,
                Currency = Currency.RUB
            };

            //when
            var validationResult = _validator.TestValidate(transaction);

            // then
            validationResult.ShouldNotHaveAnyValidationErrors();
        }


        [TestCase(10000000)]
        [TestCase(-1)]
        [TestCase(null)]
        [TestCase(0)]
        public void TransactionRequestModel_AmountIsNullOrNotBetweenZeroAndTenThouthands_NotValid(decimal amount)
        {
            //given
            var transaction = new TransactionRequestModel
            {
                Amount = amount,
                AccountId = 2,
                Currency = Currency.RUB
            };

            //when
            var validationResult = _validator.TestValidate(transaction);

            // then
            validationResult.ShouldHaveValidationErrorFor(transaction => transaction.Amount);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-7)]
        public void TransactionRequestModel_AccountIdNotCorrect_NotValid(int accountId)
        {
            //given
            var transaction = new TransactionRequestModel
            {
                Amount = 45,
                AccountId = accountId,
                Currency = Currency.RUB
            };

            //when
            var validationResult = _validator.TestValidate(transaction);

            // then
            validationResult.ShouldHaveValidationErrorFor(transaction => transaction.AccountId);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-7)]
        public void TransactionRequestModel_CurrencyNotCorrect_NotValid(int currency)
        {
            //given
            var transaction = new TransactionRequestModel
            {
                Amount = 45,
                AccountId = 2,
                Currency = (Currency)currency
            };

            //when
            var validationResult = _validator.TestValidate(transaction);

            // then
            validationResult.ShouldHaveValidationErrorFor(transaction => transaction.Currency);
        }

    }
}