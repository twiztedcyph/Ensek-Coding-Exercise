using System.ComponentModel.DataAnnotations;

namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Represents the model for editing an account's details
    /// </summary>
    public class EditAccountModel
    {
        /// <summary>
        /// Identifier for the account
        /// </summary>
        [Required]
        public int AccountId { get; set; }

        /// <summary>
        /// First name of the account holder
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the account holder
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}
