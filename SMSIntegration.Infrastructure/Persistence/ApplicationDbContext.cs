using Microsoft.EntityFrameworkCore;
using SMSIntegration.Domain.Entities;

namespace SMSIntegration.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SmsLog> SmsLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmsLog>().ToTable("SmsLogs");
            base.OnModelCreating(modelBuilder);
        }
    }
}
