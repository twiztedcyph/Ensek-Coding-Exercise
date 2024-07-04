using EnsekCodingExercise.ApiService.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EnsekCodingExercise.ApiService.Infrastructure.Contexts
{
    /// <summary>
    /// Context for the customer database
    /// </summary>
    public class CustomerContext : DbContext
    {
        /// <summary>
        /// Accounts DbSet
        /// </summary>
        public DbSet<Account> Accounts { get; set; }
        
        /// <summary>
        /// Readings DbSet
        /// </summary>
        public DbSet<Reading> Readings { get; set; }

        /// <summary>
        /// Customer Context Constructor
        /// </summary>
        /// <param name="options"></param>
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options){ }
    }
}
