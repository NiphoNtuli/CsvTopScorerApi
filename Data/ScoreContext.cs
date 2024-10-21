using CsvTopScorerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CsvTopScorerApi.Data
{
    public class ScoreContext : DbContext
    {
        public ScoreContext(DbContextOptions<ScoreContext> options) : base(options) { }

        // DbSet property representing the Scores table in the database
        public DbSet<Score> Scores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Check if the options are already configured
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=scores.db"); 
            }
        }
    }
}
