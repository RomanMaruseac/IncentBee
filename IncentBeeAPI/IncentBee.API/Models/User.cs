using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required, StringLength(50)]
        public string Username { get; set; }
        
        [Required, StringLength(100)]
        public string Email { get; set; }
        
        [Required, StringLength(255)]
        public string PasswordHash { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLogin { get; set; }
        
        [Column(TypeName = "decimal(12,2)")]
        public decimal PointsBalance { get; set; }
        
        public int? ReferrerId { get; set; }
        
        [StringLength(100)]
        public string ExternalId { get; set; }
        
        [Required, StringLength(20)]
        public string AccountStatus { get; set; } = "active";
        
        [StringLength(255)]
        public string ProfileImageUrl { get; set; }
        
        [StringLength(50)]
        public string Country { get; set; }
        
        [StringLength(45)]
        public string IpAddress { get; set; }
        
        public string DeviceInfo { get; set; }
        
        // Navigation properties
        [ForeignKey("ReferrerId")]
        public User Referrer { get; set; }
        
        public ICollection<User> Referrals { get; set; }
        public ICollection<Completion> Completions { get; set; }
        public ICollection<Redemption> Redemptions { get; set; }
        public ICollection<Referral> ReferralsCreated { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}