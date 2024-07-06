using System.ComponentModel.DataAnnotations;

namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Model for editing a Reading
    /// </summary>
    public class EditReadingModel
    {
        /// <summary>
        /// Unique identifier for the Reading
        /// </summary>
        [Required]
        public int ReadingId { get; set; }

        // No AccountId property in the EditReadingModel class as I don't think that swapping the account associated with a reading is a good idea.

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
