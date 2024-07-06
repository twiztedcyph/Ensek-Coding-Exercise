using System.ComponentModel.DataAnnotations;

namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Model for creating an Account
    /// </summary>
    public class CreateAccountModel
    {
        /// <summary>
        /// The first name of the account holder
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the account holder
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}
