using System.ComponentModel.DataAnnotations;

namespace EnsekCodingExercise.Web.Models
{
    public class CreateAccount
    {
        /// <summary>
        /// First name of the account holder
        /// </summary>
        [Required]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the account holder
        /// </summary>
        [Required]
        public string? LastName { get; set; }
    }
}
