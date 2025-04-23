using Microsoft.EntityFrameworkCore;
using IncentBee.API.Models;

namespace IncentBee.API
{
    public class IncentBeeDbContext : DbContext
    {
        public IncentBeeDbContext(DbContextOptions<IncentBeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OfferTask> Tasks { get; set; }


        public DbSet<Completion> Completions { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<ReferralCommission> ReferralCommissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User self-referencing relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Referrer)
                .WithMany(u => u.Referrals)
                .HasForeignKey(u => u.ReferrerId)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
                
            modelBuilder.Entity<Completion>()
                .HasIndex(c => c.TransactionId)
                .IsUnique();
                
            modelBuilder.Entity<Referral>()
                .HasIndex(r => r.ReferredId)
                .IsUnique();
                
            modelBuilder.Entity<Setting>()
                .HasIndex(s => s.SettingKey)
                .IsUnique();
        }
    }
}