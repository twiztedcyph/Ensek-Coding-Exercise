using System.ComponentModel.DataAnnotations;

namespace EnsekCodingExercise.ApiService.Models.Database
{
    /// <summary>
    /// Represents a reading associated with an account
    /// </summary>
    public class Reading
    {
        /// <summary>
        /// Unique identifier for the reading
        /// </summary>
        [Key]
        [Required]
        public int ReadingId { get; set; }

        /// <summary>
        /// Identifier for the associated account
        /// </summary>
        [Required]
        public int AccountId { get; set; }

        /// <summary>
        /// Date and time when the reading was taken
        /// </summary>
        [Required]
        public DateTime? ReadingDateTime { get; set; }

        /// <summary>
        /// Value of the reading
        /// </summary>
        [Required]
        public int Value { get; set; }
    }
}
