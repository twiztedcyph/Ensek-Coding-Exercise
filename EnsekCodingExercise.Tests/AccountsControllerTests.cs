using EnsekCodingExercise.ApiService.Controllers;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace EnsekCodingExercise.Tests
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountsService> _mockAccountsService;
        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            _mockAccountsService = new Mock<IAccountsService>();
            _controller = new AccountsController(_mockAccountsService.Object);
        }

        [Fact]
        public async Task GetAllAccounts_ReturnsAllAccounts()
        {
            // Arrange
            var accounts = new List<AccountDto>
        {
            new AccountDto { AccountId = 1, FirstName = "John", LastName = "Doe" },
            new AccountDto { AccountId = 2, FirstName = "Jane", LastName = "Doe" }
        };
            _mockAccountsService.Setup(service => service.GetAllAccounts()).ReturnsAsync(accounts);

            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccounts = Assert.IsType<List<AccountDto>>(okResult.Value);
            Assert.Equal(2, returnedAccounts.Count);
        }

        [Fact]
        public async Task GetAccountById_ReturnsAccount_WhenAccountExists()
        {
            // Arrange
            var accountId = 1;
            var account = new AccountDto { AccountId = accountId, FirstName = "John", LastName = "Doe" };
            _mockAccountsService.Setup(service => service.GetAccountById(accountId)).ReturnsAsync(account);

            // Act
            var result = await _controller.GetAccountById(accountId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccount = Assert.IsType<AccountDto>(okResult.Value);
            Assert.Equal(accountId, returnedAccount.AccountId);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAccountId()
        {
            // Arrange
            var createAccountModel = new CreateAccountModel { FirstName = "New", LastName = "User" };
            _mockAccountsService.Setup(service => service.CreateAccount(createAccountModel)).ReturnsAsync(3);

            // Act
            var result = await _controller.CreateAccount(createAccountModel);

            // Assert
            var createdAtResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(3, createdAtResult.Value);
        }

        [Fact]
        public async Task EditAccount_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var editAccountModel = new EditAccountModel { AccountId = 1, FirstName = "Updated", LastName = "User" };
            _mockAccountsService.Setup(service => service.EditAccount(editAccountModel)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditAccount(1, editAccountModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAccountById_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var accountId = 1;
            _mockAccountsService.Setup(service => service.DeleteAccountById(accountId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAccountById(accountId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
