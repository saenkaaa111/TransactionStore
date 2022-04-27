using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.BuisnessLayer.Configuration;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class TransactionServiceTests : VerifyLoggerHelper<TransactionService>
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private TransactionService _transactionService;
        private Mock<ICalculationService> _calculationServiceMock;
        private Mock<IBalanceRepository> _balanceRepositoryMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DataMapper>()));
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _balanceRepositoryMock = new Mock<IBalanceRepository>();
            _logger = new Mock<ILogger<TransactionService>>();
            _calculationServiceMock = new Mock<ICalculationService>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object,
            _calculationServiceMock.Object, _balanceRepositoryMock.Object, _mapper, _logger.Object);
        }

        [TestCaseSource(typeof(DepositTestCaseSourse))]
        public async Task AddDepositTest_ValidRequestReceived_ShouldAddDeposit(TransactionModel depositModel, TransactionDto depositDto, long expected)
        {
            //given
            _transactionRepositoryMock.Setup(d => d.AddTransaction(depositDto)).ReturnsAsync(expected);

            //when
            var actual = await _transactionService.AddDeposit(depositModel);

            // then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(s => s.AddTransaction(depositDto), Times.Once);
            LoggerVerify("Request to add Deposit", LogLevel.Information);
        }

        [TestCaseSource(typeof(TransferTestCaseSource))]
        public async Task AddTransfer_ValidRequestReceived_ShouldAddTransfer(TransferModel transferModel,
            TransferDto transferDto, List<long> expected, ArrayList array, decimal convertedAmount)
        {
            //given
            _transactionRepositoryMock.Setup(t => t.AddTransfer(transferDto)).ReturnsAsync(expected);
            _balanceRepositoryMock.Setup(b => b.GetBalanceByAccountId(transferModel.AccountIdFrom)).ReturnsAsync(array);
            _balanceRepositoryMock.Setup(h => h.GetLastDate()).Returns((DateTime)array[1]);
            _calculationServiceMock.Setup(c => c.ConvertCurrency(transferModel.CurrencyFrom,
                transferModel.CurrencyTo, transferModel.Amount)).Returns(convertedAmount);

            // when
            var actual = await _transactionService.AddTransfer(transferModel);

            // then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(t => t.AddTransfer(transferDto), Times.Once);
            _balanceRepositoryMock.Verify(b => b.GetBalanceByAccountId(transferModel.AccountIdFrom), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            _calculationServiceMock.Verify(c => c.ConvertCurrency(transferModel.CurrencyFrom, transferModel.CurrencyTo,
                transferModel.Amount), Times.Once);
            LoggerVerify("Request to add Transfer", LogLevel.Information);
        }

        [TestCaseSource(typeof(TransferNegativeTestCaseSource))]
        public async Task AddTransfer_BalanceLessThanAmount_ShouldThrowInsufficientFundsException(TransferModel transferModel,
            ArrayList array)
        {
            //given
            
            _transactionRepositoryMock.Setup(w => w.AddTransfer(It.IsAny<TransferDto>()));
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transferModel.AccountIdFrom))
                .ReturnsAsync(array);
            _balanceRepositoryMock.Setup(h => h.GetLastDate()).Returns((DateTime)array[1]);
            var expectedMessage = "Insufficient funds";

            //when
            InsufficientFundsException? exception = Assert.ThrowsAsync<InsufficientFundsException>(() =>
            _transactionService.AddTransfer(transferModel));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            _balanceRepositoryMock.Verify(b => b.GetBalanceByAccountId(transferModel.AccountIdFrom), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            LoggerVerify("Exception: Insufficient funds", LogLevel.Error);
        }

        [TestCaseSource(typeof(WithdrawTestCaseSourse))]
        public async Task Withdraw_ValidRequestReceived_ShouldAddTransation(TransactionModel transactionModel, TransactionDto transactionDto, long expected, ArrayList array)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(transactionDto)).ReturnsAsync(expected);
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transactionModel.AccountId))
                .ReturnsAsync(array);
            _balanceRepositoryMock.Setup(h => h.GetLastDate()).Returns((DateTime)array[1]);



            //when
            var actual = await _transactionService.Withdraw(transactionModel);

            // then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(s => s.AddTransaction(transactionDto), Times.Once);
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(transactionModel.AccountId), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            LoggerVerify("Request to add Withdraw", LogLevel.Information);
        }

        [TestCaseSource(typeof(WithdrawNegativeTestCaseSourse))]
        public async Task Withdraw_BalanceLessThenAmount_ShouldThrowInsufficientFundsException(TransactionModel transactionModel, ArrayList array)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transactionModel.AccountId))
                .ReturnsAsync(array );
            _balanceRepositoryMock.Setup(n => n.GetLastDate()).Returns((DateTime)array[1]);
            var expectedMessage = "Insufficient funds";

            //when
            InsufficientFundsException? exception = Assert.ThrowsAsync<InsufficientFundsException>(() =>
            _transactionService.Withdraw(transactionModel));

            // then
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(transactionModel.AccountId), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Exception: Insufficient funds", LogLevel.Error);
        }
        
        [TestCaseSource(typeof(WithdrawNegativeTestCaseSourse))]
        public async Task Withdraw_DateDoesntMatch_ShouldThrowBDTimeoutException(TransactionModel transactionModel, ArrayList array)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transactionModel.AccountId))
                .ReturnsAsync(array );
            _balanceRepositoryMock.Setup(n => n.GetLastDate()).Returns(It.IsAny<DateTime>());
            var expectedMessage = "Flood crossing";

            //when
            DbTimeoutException? exception = Assert.ThrowsAsync<DbTimeoutException>(() =>
            _transactionService.Withdraw(transactionModel));

            // then
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(transactionModel.AccountId), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Exception: Flood crossing", LogLevel.Error);
        }

        [TestCaseSource(typeof(GetTransactionsByAccountIdsTestCaseSourse))]
        public async Task GetTransactionsByAccountIds_ValidRequestReceived_ShouldGetTransactionsByAccountId(
            List<int> ids, List<TransactionDto> transactions, ArrayList expected)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);

            //when
            var actual = await _transactionService.GetTransactionsByAccountIds(ids);

            //then
            Assert.AreEqual(actual, expected);
            _transactionRepositoryMock.Verify(s => s.GetTransactionsByAccountIds(ids), Times.Once);
            LoggerVerify($"Request to get transactions by AccountId = {ids}", LogLevel.Information);
        }

        [Test]
        public async Task GetTransactionsByAccountIds_TransactionsNotFound_ShouldThrowTransactionNotFoundException()
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(It.IsAny<List<int>>())).ReturnsAsync((List<TransactionDto>)null);
            var expectedMessage = $"Transactions weren't found";

            //when
            TransactionNotFoundException? exception = Assert.ThrowsAsync<TransactionNotFoundException>(async () =>
            await _transactionService.GetTransactionsByAccountIds(It.IsAny<List<int>>()));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify($"Error: Transactions weren't found", LogLevel.Error);
        }

        [TestCase(77)]
        public async Task GetTransactionById_ValidRequestReceived_ShouldGetTransaction(long id)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionById(id))
                .ReturnsAsync(new TransactionDto() { Id = id });

            //when
            var actual = await _transactionService.GetTransactionById(id);

            //then
            Assert.AreEqual(actual.Id, id);
            _transactionRepositoryMock.Verify(s => s.GetTransactionById(id), Times.Once);
            LoggerVerify($"Request to get transaction by id = {id}", LogLevel.Information);
        }

        [TestCase(-77)]
        public async Task GetTransactionById_TransactionNotFound_ShouldThrowTransactionNotFoundException(long id)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionById(id)).ReturnsAsync((TransactionDto)null);
            var expectedMessage = $"Transaction with Id = {id} wasn't found";

            //when
            TransactionNotFoundException? exception = Assert.ThrowsAsync<TransactionNotFoundException>(async () =>
            await _transactionService.GetTransactionById(id));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify($"Error: Transaction with Id = {id} wasn't found", LogLevel.Error);
        }
        
        [TestCase(77, 100, 120)]
        public async Task CheckDateAndBalance_ValidRequestReceived_ShouldReturnTrue(int accountId, decimal amount, decimal amountForCheck)
        {
            //given
            var date = DateTime.Now;
            ArrayList array = new ArrayList() { amountForCheck, date};
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(accountId)).ReturnsAsync(array);
            _balanceRepositoryMock.Setup(n => n.GetLastDate()).Returns(date);
                        
            //when
            var actual = _transactionService.CheckDateAndBalance(accountId, amount);

            // then
            Assert.IsTrue(actual);
            LoggerVerify("Correct information", LogLevel.Information);
        }
        
        [TestCase(77, 120, 100)]
        public async Task CheckDateAndBalance_BalanceLessThenAmount_ShouldThrowInsufficientFundsExceptionn(int accountId, decimal amount, decimal amountForCheck)
        {
            //given
            var date = DateTime.Now;
            ArrayList array = new ArrayList() { amountForCheck, date};
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(accountId)).ReturnsAsync(array);
            _balanceRepositoryMock.Setup(n => n.GetLastDate()).Returns(date);            
            var expectedMessage = "Insufficient funds";

            //when
            InsufficientFundsException? exception = Assert.Throws<InsufficientFundsException>(() =>
            _transactionService.CheckDateAndBalance(accountId, amount));

            // then
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(accountId), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Exception: Insufficient funds", LogLevel.Error);
        }
        
        [TestCase(77, 100, 120)]
        public async Task CheckDateAndBalance_DateDoesntMatch_ShouldThrowBDTimeoutException(int accountId, decimal amount, decimal amountForCheck)
        {
            //given
            var date = DateTime.Now;
            ArrayList array = new ArrayList() { amountForCheck, date};
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(accountId)).ReturnsAsync(array);
            _balanceRepositoryMock.Setup(n => n.GetLastDate()).Returns(It.IsAny<DateTime>());            
            var expectedMessage = "Flood crossing";

            //when
            DbTimeoutException? exception = Assert.ThrowsAsync<DbTimeoutException>(async () =>
            _transactionService.CheckDateAndBalance(accountId, amount));

            // then
            //Assert.IsFalse(Action);
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(accountId), Times.Once);
            _balanceRepositoryMock.Verify(n => n.GetLastDate(), Times.Once);
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Exception: Flood crossing", LogLevel.Error);
        }
    }

}