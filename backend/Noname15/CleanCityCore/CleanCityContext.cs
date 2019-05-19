using CleanCityCore.Sql;
using Microsoft.EntityFrameworkCore;

namespace CleanCityCore
{
    public class CleanCityContext : DbContext
    {
        public DbSet<EmailMessageSql> Emails { get; set; }
        public DbSet<ResponsibleSql> ResponsibleList { get; set; }
        public DbSet<ResponsibleSql> ResponsibleDoublerList { get; set; }
        public DbSet<ReportSql> Reports { get; set; }
        public DbSet<UserSql> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=db;Database=database;Username=admin;Password=password");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmailMessageSql>().HasIndex(x => x.IsSent);
            modelBuilder.Entity<ResponsibleDoublerSql>()
                .HasOne(p => p.OriginalResponsible)
                .WithMany(b => b.DoublerList)
                .HasForeignKey(p => p.OriginalId)
                .HasConstraintName("ForeignKey_Doubler_Responsible");
        }
    }
}