using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using NUnit.Framework;
using TransactionStore.API.Validators;

namespace TransactionStore.API.Tests
{
    public class TransactionRequestModelTests
    {

        [Test]
        public void TransactionRequestModel_IsValid()
        {
            //given
            var validator = new TransactionRequestModelValidator();
            var transaction = new TransactionRequestModel
            {
                Amount = 45,
                AccountId = 2,
                Currency = Currency.RUB
            };

            //when
            var validationResult = validator.Validate(transaction);

            // then
            Assert.IsTrue(validationResult.IsValid);
        }


        [TestCase(10000000)]
        [TestCase(-1)]
        [TestCase(null)]
        [TestCase(0)]
        public void TransactionRequestModel_AmountIsNullOrNotBetweenZeroAndTenThouthands_NotValid(decimal amount)
        {
            //given
            var validator = new TransactionRequestModelValidator();
            var transaction = new TransactionRequestModel
            {
                Amount = amount,
                AccountId = 2,
                Currency = Currency.RUB
            };

            //when
            var validationResult = validator.Validate(transaction);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(-7)]
        public void TransactionRequestModel_AccountIdNotCorrect_NotValid(int accountId)
        {
            //given
            var validator = new TransactionRequestModelValidator();
            var transaction = new TransactionRequestModel
            {
                Amount = 45,
                AccountId = accountId,
                Currency = Currency.RUB
            };

            //when
            var validationResult = validator.Validate(transaction);

            // then
            Assert.IsFalse(validationResult.IsValid);
        }

    }
}