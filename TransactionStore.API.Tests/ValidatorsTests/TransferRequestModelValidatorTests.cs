using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using NUnit.Framework;
using TransactionStore.API.Validators;
using FluentValidation.TestHelper;

namespace TransactionStore.API.Tests
{
    public class TransferRequestModelValidatorTests
    {
        private TransferRequestModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new TransferRequestModelValidator();
        }

        [Test]
        public void TransferRequestModel_IsValid_ValidationPassed()
        {
            //given
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = _validator.TestValidate(transfer);

            //then
            validationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void TransferRequestModel_IsNull_ValidationFailed()
        {
            //given

            //when
            var validationResult = _validator!.TestValidate(null);

            //then
            validationResult.ShouldHaveAnyValidationError();
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        [TestCase(7777777)]
        public void Amount_NotValid_ValidationFailed(decimal amount)
        {
            //given
            var transfer = new TransferRequestModel
            {
                Amount = amount,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = _validator.TestValidate(transfer);

            //then
            validationResult.ShouldHaveValidationErrorFor(transfer => transfer.Amount);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void AccountIdFrom_NotValid_ValidationFailed(int accountIdFrom)
        {
            //given
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = accountIdFrom,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = _validator.TestValidate(transfer);

            //then
            validationResult.ShouldHaveValidationErrorFor(transfer => transfer.AccountIdFrom);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        [TestCase(1)]
        public void AccountIdTo_NotValid_ValidationFailed(int accountIdTo)
        {
            //given
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = accountIdTo,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = _validator.TestValidate(transfer);

            //then
            validationResult.ShouldHaveValidationErrorFor(transfer => transfer.AccountIdTo);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void CurrencyFrom_NotValid_ValidationFailed(int currency)
        {
            //given
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = (Currency)currency,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = _validator.TestValidate(transfer);

            //then
            validationResult.ShouldHaveValidationErrorFor(transfer => transfer.CurrencyFrom);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void CurrencyTo_NotValid_ValidationFailed(int currency)
        {
            //given
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.USD,
                CurrencyTo = (Currency)currency
            };

            //when
            var validationResult = _validator.TestValidate(transfer);

            //then
            validationResult.ShouldHaveValidationErrorFor(transfer => transfer.CurrencyTo);
        }
    }
}
