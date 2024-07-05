using EnsekCodingExercise.ApiService.Models.External;

namespace EnsekCodingExercise.ApiService.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services handling reading data operations.
    /// </summary>
    public interface IReadingsService
    {
        /// <summary>
        /// Retrieves all meter readings.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ReadingDto"/>.</returns>
        Task<List<ReadingDto>> GetAllReadings();

        /// <summary>
        /// Retrieves a specific meter reading by its ID.
        /// </summary>
        /// <param name="id">The ID of the meter reading to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ReadingDto"/> if found, otherwise null.</returns>
        Task<ReadingDto?> GetReadingById(int id);

        /// <summary>
        /// Creates a new meter reading.
        /// </summary>
        /// <param name="createReadingModel">The model containing the data needed to create a new reading.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created reading.</returns>
        Task<int> CreateReading(CreateReadingModel createReadingModel);

        /// <summary>
        /// Edits an existing meter reading.
        /// </summary>
        /// <param name="editReadingModel">The model containing the updated data for the reading.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EditReading(EditReadingModel editReadingModel);

        /// <summary>
        /// Deletes a meter reading by its ID.
        /// </summary>
        /// <param name="id">The ID of the meter reading to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteReadingById(int id);

        /// <summary>
        /// Processes a file upload containing meter readings and uploads them to the system.
        /// </summary>
        /// <param name="formFile">The uploaded file containing meter readings.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UploadResults"/> detailing the outcome of the upload.</returns>
        Task<UploadResults> UploadReadingsFromFile(IFormFile formFile);
    }
}
