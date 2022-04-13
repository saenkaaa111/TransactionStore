using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using NUnit.Framework;
using TransactionStore.API.Validation;

namespace TransactionStore.API.Tests
{
    public class RequestModelTests
    {

        [SetUp]
        public void Setup()
        {
            
        }

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
        
        
        [Test]
        public void TransactionRequestModel_AmountLessThenZero_NotValid()
        {
            //given
            var validator = new TransactionRequestModelValidator();
            var transaction = new TransactionRequestModel
            {                
                Amount = -45,
                AccountId = 2,
                Currency = Currency.RUB
            };
            
            //when
            var validationResult = validator.Validate(transaction);
            
            // then
            Assert.IsFalse(validationResult.IsValid);
        }
        
        [Test]
        public void TransactionRequestModel_AccountIdNotCorrect_NotValid()
        {
            //given
            var validator = new TransactionRequestModelValidator();
            var transaction = new TransactionRequestModel
            {                
                Amount = 45,
                AccountId = -7,
                Currency = Currency.RUB
            };
            
            //when
            var validationResult = validator.Validate(transaction);
            
            // then
            Assert.IsFalse(validationResult.IsValid);
        }        
        
    }
}