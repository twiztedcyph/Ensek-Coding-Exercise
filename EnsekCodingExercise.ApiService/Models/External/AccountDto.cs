namespace EnsekCodingExercise.ApiService.Models.External
{
    /// <summary>
    /// Data transfer object for an account.
    /// </summary>
    public class AccountDto
    {
        // Yep, it's the same as Account but in the real world, it would likely be different.
        // I'm just not a fan of exposing my database models directly to the outside world.

        /// <summary>
        /// Unique identifier for the account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// First name of the account holder.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the account holder.
        /// </summary>
        public string? LastName { get; set; }
    }
}
