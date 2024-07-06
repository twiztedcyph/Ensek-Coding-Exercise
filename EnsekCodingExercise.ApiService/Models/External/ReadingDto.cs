namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Data transfer object for a reading
    /// </summary>
    public class ReadingDto
    {
        // Again, not a fan of exposing my database models directly to the outside world

        /// <summary>
        /// Unique identifier for the reading
        /// </summary>
        public int ReadingId { get; set; }

        /// <summary>
        /// Identifier for the associated account
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Date and time when the reading was taken
        /// </summary>
        public DateTime? ReadingDateTime { get; set; }

        /// <summary>
        /// Value of the reading
        /// </summary>
        public string MeterReadValue { get; set; } = string.Empty;
    }
}
