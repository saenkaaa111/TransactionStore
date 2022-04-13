using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using NUnit.Framework;
using TransactionStore.API.Validation;

namespace TransactionStore.API.Tests
{
    public class TransferRequestModelTests
    {
        [Test]
        public void TransferRequestModel_IsValid()
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
        [TestCase(-100)]
        [TestCase(1000000)]
        public void Amount_HasErrorWhenNullOrNotBetweenZeroAndTenThouthands(decimal amount)
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
        [TestCase(-100)]
        public void AccountIdFrom_IsNullOrLessThenZero(int accountIdFrom)
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
        [TestCase(-100)]
        public void AccountIdTo_IsNullOrLessThenZero(int accountIdTo)
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

        [Test]
        public void CurrencyFrom_IsNull()
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyTo = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void CurrencyTo_IsNull()
        {
            //given
            var validator = new TransferRequestModelValidator();
            var transfer = new TransferRequestModel
            {
                Amount = 77,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.USD,
            };

            //when
            var validationResult = validator.Validate(transfer);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}
