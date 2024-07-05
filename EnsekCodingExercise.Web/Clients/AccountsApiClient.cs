using EnsekCodingExercise.Web.Models;

namespace EnsekCodingExercise.Web.Clients
{
    public class AccountsApiClient : BaseClient
    {
        public AccountsApiClient(HttpClient httpClient) : base(httpClient) { }

        // Get all accounts
        public async Task<List<Account>> GetAccountsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("/v1/accounts", cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<Account>>(cancellationToken: cancellationToken) ?? new List<Account>();
            return result;
        }

        // Get account by ID
        public async Task<Account?> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"/v1/accounts/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Account>(cancellationToken: cancellationToken);
            return result;
        }

        // Create account
        public async Task<int> CreateAccountAsync(CreateAccount createAccount, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("/v1/accounts", createAccount, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
            return result;
        }

        // Edit account
        public async Task EditAccountAsync(int id, EditAccount editAccount, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync($"/v1/accounts/{id}", editAccount, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        // Delete account
        public async Task DeleteAccountAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync($"/v1/accounts/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
