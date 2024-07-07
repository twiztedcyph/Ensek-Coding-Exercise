using EnsekCodingExercise.Web.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EnsekCodingExercise.Web.Clients
{
    public class ReadingsApiClient : BaseClient
    {
        public ReadingsApiClient(HttpClient httpClient) : base(httpClient) { }

        // Get all readings
        public async Task<List<Reading>> GetAllReadingsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("/v1/readings", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Reading>>(cancellationToken: cancellationToken) ?? new List<Reading>();
                return result;
            }
            else
            {
                // Handle error response
                throw new HttpRequestException($"Error fetching readings: {response.ReasonPhrase}");
            }
        }

        // Get reading by ID
        public async Task<Reading?> GetReadingByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"/v1/readings/{id}", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Reading>(cancellationToken: cancellationToken);
                return result;
            }
            else
            {
                // Handle error response
                throw new HttpRequestException($"Error fetching reading with ID {id}: {response.ReasonPhrase}");
            }
        }

        // Get readings by account ID
        public async Task<List<Reading>> GetReadingsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"/v1/readings/account/{accountId}", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Reading>>(cancellationToken: cancellationToken) ?? new List<Reading>();
                return result;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                   return new List<Reading>();
                }
                else
                {
                    // Handle error response
                    throw new HttpRequestException($"Error fetching readings for account ID {accountId}: {response.ReasonPhrase}");
                }
            }
        }

        // Create reading
        public async Task CreateReadingAsync(CreateReading createReadingModel, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("/v1/readings", createReadingModel, cancellationToken);
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>(cancellationToken: cancellationToken);
            if (result != null && result.ContainsKey("error"))
            {
                // super hax to demo getting the error.
                throw new HttpRequestException($"Error creating reading: {result["error"]}");
            }
        }

        // Edit reading
        public async Task EditReadingAsync(int id, EditReading editReadingModel, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync($"/v1/readings/{id}", editReadingModel, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                throw new HttpRequestException($"Error editing reading with ID {id}: {response.ReasonPhrase}");
            }
        }

        // Delete reading
        public async Task DeleteReadingAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync($"/v1/readings/{id}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                throw new HttpRequestException($"Error deleting reading with ID {id}: {response.ReasonPhrase}");
            }
        }

        // Upload meter readings
        public async Task<UploadResults?> MeterReadingUploadAsync(IBrowserFile file, CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            using (var fileStream = file.OpenReadStream(cancellationToken: cancellationToken))
            {
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "formFile", file.Name);

                var response = await _httpClient.PostAsync("/v1/readings/meter-reading-uploads", content, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UploadResults>(cancellationToken: cancellationToken);
                    return result;
                }
                else
                {
                    // Handle error response
                    throw new HttpRequestException($"Error uploading meter readings: {response.ReasonPhrase}");
                }
            }
        }
    }
}
