using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using NUnit.Framework;
using TransactionStore.API.Validators;

namespace TransactionStore.API.Tests
{
    public class TransferRequestModelTests
    {
        [Test]
        public void TransferRequestModel_IsValid_ValidationPassed()
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        [TestCase(7777777)]
        public void Amount_NotValid_ValidationFailed(decimal amount)
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = amount,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void AccountIdFrom_NotValid_ValidationFailed(int accountIdFrom)
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = accountIdFrom,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void AccountIdTo_NotValid_ValidationFailed(int accountIdTo)
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = accountIdTo,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void CurrencyFrom_NotValid_ValidationFailed(int currency)
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = (Currency)currency,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-77)]
        public void CurrencyTo_NotValid_ValidationFailed(int currency)
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.USD,
                CurrencyTo = (Currency)currency
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}
