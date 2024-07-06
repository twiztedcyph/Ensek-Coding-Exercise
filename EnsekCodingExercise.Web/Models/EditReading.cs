using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EnsekCodingExercise.Web.Models
{
    public class EditReading : IValidatableObject
    {
        /// <summary>
        /// Unique identifier for the reading
        /// </summary>
        public int ReadingId { get; set; }

        /// <summary>
        /// Reading date
        /// </summary>
        [Required]
        public DateOnly? ReadingDate { get; set; }

        /// <summary>
        /// Reading time
        /// </summary>
        [Required]
        public TimeOnly? ReadingTime { get; set; }

        /// <summary>
        /// Date and time when the reading was taken
        /// </summary>
        public DateTime? ReadingDateTime { get { return ReadingDate!.Value.ToDateTime(ReadingTime!.Value); } }

        /// <summary>
        /// Value of the reading
        /// </summary>
        [Required]
        [MaxLength(5)]
        [MinLength(5)]
        public string MeterReadValue { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Regex.IsMatch(MeterReadValue, @"^\d{5}$"))
            {
                yield return new ValidationResult("Meter read value must be a 5 digit number", new[] { nameof(MeterReadValue) });
            }
        }
    }
}
