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
                
            // Configure Referral relationships
            modelBuilder.Entity<Referral>()
                .HasOne(r => r.Referrer)
                .WithMany(u => u.ReferralsCreated)
                .HasForeignKey(r => r.ReferrerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Referral>()
                .HasOne(r => r.Referred)
                .WithOne()
                .HasForeignKey<Referral>(r => r.ReferredId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Configure relationship between User and Completions
            modelBuilder.Entity<Completion>()
                .HasOne(c => c.User)
                .WithMany(u => u.Completions)
                .HasForeignKey(c => c.UserId);
                
            // Configure relationship between OfferTask and Completions
modelBuilder.Entity<Completion>()
    .HasOne(c => c.Task)
    .WithMany(t => t.Completions)
    .HasForeignKey(c => c.TaskId);
                
            // Configure relationship between User and Redemptions
            modelBuilder.Entity<Redemption>()
                .HasOne(r => r.User)
                .WithMany(u => u.Redemptions)
                .HasForeignKey(r => r.UserId);
                
            // Configure relationship between Reward and Redemptions
            modelBuilder.Entity<Redemption>()
                .HasOne(r => r.Reward)
                .WithMany(rw => rw.Redemptions)
                .HasForeignKey(r => r.RewardId);
                
            // Configure relationship between User and Notifications
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);
                
            // Configure relationship between Referral and ReferralCommissions
            modelBuilder.Entity<ReferralCommission>()
                .HasOne(rc => rc.Referral)
                .WithMany(r => r.Commissions)
                .HasForeignKey(rc => rc.ReferralId);

            // Configure relationship between Completion and ReferralCommissions                
            modelBuilder.Entity<ReferralCommission>()
                .HasOne(rc => rc.Completion)
                .WithMany(c => c.ReferralCommissions)
                .HasForeignKey(rc => rc.CompletionId);
                
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