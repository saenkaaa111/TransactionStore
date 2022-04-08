using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TestCaseSource;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class BalanceServiceTests
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<ICalculationService> _calculationServiceMock;
        private Mock<ILogger<TransactionService>> _logger;
        private BalanceService _balanceService;

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _logger = new Mock<ILogger<TransactionService>>();
            _calculationServiceMock = new Mock<ICalculationService>();
            _balanceService = new BalanceService(_transactionRepositoryMock.Object,
                _calculationServiceMock.Object, _logger.Object);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrencyTestCaseSource))]
        public void GetBalanceByAccountIdsInGivenCurrencyTest(List<int> id, Currency currency)
        {
            //given


            //when
            var actual = _balanceService.GetBalanceByAccountIdsInGivenCurrency(id, currency);

            //then


        }
    }
}
