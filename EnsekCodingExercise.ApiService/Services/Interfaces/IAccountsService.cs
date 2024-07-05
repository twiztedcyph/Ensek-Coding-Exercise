using EnsekCodingExercise.ApiService.Models.External;

namespace EnsekCodingExercise.ApiService.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services handling account data operations.
    /// </summary>
    public interface IAccountsService
    {
        /// <summary>
        /// Retrieves all accounts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="AccountDto"/>.</returns>
        Task<List<AccountDto>> GetAllAccounts();

        /// <summary>
        /// Retrieves a specific account by its ID.
        /// </summary>
        /// <param name="id">The ID of the account to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AccountDto"/> if found, otherwise null.</returns>
        Task<AccountDto?> GetAccountById(int id);

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="createAccountModel">The model containing the data needed to create a new account.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created account.</returns>
        Task<int> CreateAccount(CreateAccountModel createAccountModel);

        /// <summary>
        /// Edits an existing account.
        /// </summary>
        /// <param name="editAccountModel">The model containing the updated data for the account.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EditAccount(EditAccountModel editAccountModel);

        /// <summary>
        /// Deletes an account by its ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteAccountById(int id);
    }
}
