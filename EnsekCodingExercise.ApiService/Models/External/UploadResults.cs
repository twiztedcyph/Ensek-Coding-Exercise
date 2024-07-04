namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Represents the results of an upload operation
    /// </summary>
    public class UploadResults
    {
        /// <summary>
        /// The number of successful uploads
        /// </summary>
        public int Successful { get; set; } = 0;
        
        /// <summary>
        /// The number of failed uploads
        /// </summary>
        public int Failed { get; set; } = 0;

        /// <summary>
        /// The total number of records uploaded
        /// </summary>
        public int TotalRecords { get; set; } = 0;
    }
}
