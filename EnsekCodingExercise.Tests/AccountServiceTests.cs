using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekCodingExercise.Tests
{
    public class AccountServiceTests
    {
        private readonly IAccountsService _accountsService;

        public AccountServiceTests()
        {
            // Set up a service collection
            var serviceCollection = new ServiceCollection();

            // Add DbContextFactory with a real database connection string
            var connectionString = "Server=(local);Database=EnsekCodingExercise;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
            serviceCollection.AddDbContextFactory<CustomerContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Add the AccountsService
            serviceCollection.AddScoped<IAccountsService, AccountsService>();

            // Build the service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get the AccountsService instance
            _accountsService = serviceProvider.GetRequiredService<IAccountsService>();
        }

        [Fact]
        public async Task GetAllAccounts_ShouldReturnAccounts()
        {
            // Act
            var accounts = await _accountsService.GetAllAccounts();

            // Assert
            Assert.NotNull(accounts);
            Assert.NotEmpty(accounts);
        }

        [Fact]
        public async Task GetAccountById_ShouldReturnAccount()
        {
            // Arrange
            var accountId = 1234;

            // Act
            var account = await _accountsService.GetAccountById(accountId);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(accountId, account.AccountId);
        }

        [Fact]
        public async Task GetAccountById_ShouldReturnNull()
        {
            // Arrange
            var accountId = 1;

            // Act
            var account = await _accountsService.GetAccountById(accountId);

            // Assert
            Assert.Null(account);
        }

        [Fact]
        public async Task TestCrud()
        {
            // Doing this as one test to ensure the order of operations with the intention being to leave the database in the same state it started in.

            // Create
            var createModel = new CreateAccountModel
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var accountId = await _accountsService.CreateAccount(createModel);
            Assert.True(accountId > 0);

            // Read
            var account = await _accountsService.GetAccountById(accountId);
            Assert.NotNull(account);
            Assert.Equal(createModel.FirstName, account.FirstName);
            Assert.Equal(createModel.LastName, account.LastName);

            // Edit
            var editModel = new EditAccountModel
            {
                AccountId = account.AccountId,
                FirstName = "Jane",
                LastName = "Doe"
            };
            await _accountsService.EditAccount(editModel);

            var accountFromDb = await _accountsService.GetAccountById(account.AccountId);
            Assert.NotNull(accountFromDb);
            Assert.Equal(editModel.FirstName, accountFromDb.FirstName);
            Assert.Equal(editModel.LastName, accountFromDb.LastName);

            // Delete
            var deleteResult = await _accountsService.DeleteAccountById(account.AccountId);
            Assert.True(deleteResult);
        }
    }
}
