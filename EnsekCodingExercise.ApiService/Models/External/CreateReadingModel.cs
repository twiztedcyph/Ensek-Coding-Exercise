using System.ComponentModel.DataAnnotations;

namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Represents the model for creating a new Reading
    /// </summary>
    public class CreateReadingModel
    {
        /// <summary>
        /// Identifier for the associated Account
        /// </summary>
        [Required]
        public int AccountId { get; set; }

        /// <summary>
        /// Date and time when the Reading is taken
        /// </summary>
        [Required]
        public DateTime? ReadingDateTime { get; set; }

        /// <summary>
        /// Value of the Reading
        /// </summary>
        [Required]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Must be in the format of NNNNN")]
        public string MeterReadValue { get; set; } = string.Empty;
    }
}
