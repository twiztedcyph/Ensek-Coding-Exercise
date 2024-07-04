using EnsekCodingExercise.ApiService.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EnsekCodingExercise.ApiService.Infrastructure.Contexts
{
    /// <summary>
    /// Context for the customer database
    /// </summary>
    public class CustomerContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<Reading> Readings { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options){ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
