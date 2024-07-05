using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using EnsekCodingExercise.ApiService.Models.Database;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnsekCodingExercise.ApiService.Services
{
    /// <summary>
    /// Collection of Accounts Services
    /// </summary>
    public class AccountsService : IAccountsService
    {
        private readonly IDbContextFactory<CustomerContext> _dbContextFactory;

        /// <summary>
        /// Account Service Constructor
        /// </summary>
        /// <param name="dbContextFactory"></param>
        public AccountsService(IDbContextFactory<CustomerContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Get a list of all Accounts
        /// </summary>
        /// <returns>A list of all Accounts</returns>
        public async Task<List<AccountDto>> GetAllAccounts()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Accounts.Select(x => new AccountDto
            {
                AccountId = x.AccountId,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToListAsync();
        }

        /// <summary>
        /// Get Account by ID
        /// </summary>
        /// <returns>An Account matching the ID given or null if there is no Account found</returns>
        public async Task<AccountDto?> GetAccountById(int id)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var account = await context.Accounts.FindAsync(id);
            if (account == null)
            {
                return null;
            }
            else
            {
                return new AccountDto
                {
                    AccountId = account.AccountId,
                    FirstName = account.FirstName,
                    LastName = account.LastName
                };
            }
        }

        /// <summary>
        /// Create an Account
        /// </summary>
        /// <param name="createAccountModel">Model containing the details of the Account to be created</param>
        /// <returns>The ID of the created account</returns>
        public async Task<int> CreateAccount(CreateAccountModel createAccountModel)
        {
            var account = new Account
            {
                FirstName = createAccountModel.FirstName,
                LastName = createAccountModel.LastName
            };
            using var context = await _dbContextFactory.CreateDbContextAsync();
            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();
            return account.AccountId;
        }

        /// <summary>
        /// Edit an Account
        /// </summary>
        /// <param name="editAccountModel">Model of the account to be edited</param>
        public async Task EditAccount(EditAccountModel editAccountModel)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var account = await context.Accounts.FindAsync(editAccountModel.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {editAccountModel.AccountId} not found");
            }
            else
            {
                if (editAccountModel.FirstName != account.FirstName)
                {
                    account.FirstName = editAccountModel.FirstName;
                }

                if (editAccountModel.LastName != account.LastName)
                {
                    account.LastName = editAccountModel.LastName;
                }

                if (context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Delete an Account by ID
        /// </summary>
        /// <param name="id">The ID of the account to be deleted</param>
        /// <returns>True if an Account was found and deleted. False otherwise.</returns>
        public async Task<bool> DeleteAccountById(int id)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var account = await context.Accounts.FindAsync(id);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {id} not found");
            }
            else
            {
                context.Accounts.Remove(account);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
