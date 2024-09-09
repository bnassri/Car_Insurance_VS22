using Microsoft.EntityFrameworkCore;
using Car_Insurance_VS22.Models;

namespace Car_Insurance_VS22.Data
{
	public class QuoteContext : DbContext
	{
        public QuoteContext(DbContextOptions<QuoteContext> options) : base(options) 
        {
        
        }
        
        public DbSet<Insuree> Insuree { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Insuree>()
                .Property(e => e.DateOfBirth)
                .HasColumnType("date");
        }



    }
}
