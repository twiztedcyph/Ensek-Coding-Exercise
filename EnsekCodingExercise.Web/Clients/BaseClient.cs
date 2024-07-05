namespace EnsekCodingExercise.Web.Clients
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _httpClient;

        protected BaseClient(HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7569");
        }
    }
}
