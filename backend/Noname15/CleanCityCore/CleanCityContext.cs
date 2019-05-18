using CleanCityCore.Sql;
using Microsoft.EntityFrameworkCore;

namespace CleanCityCore
{
    public class CleanCityContext : DbContext
    {
        public DbSet<EmailMessageSql> Emails { get; set; }
        public DbSet<ResponsibleSql> ResponsibleList { get; set; }
        public DbSet<ReportSql> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=db;Database=database;Username=admin;Password=password");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmailMessageSql>().HasIndex(x => x.IsSent);
        }
    }
}