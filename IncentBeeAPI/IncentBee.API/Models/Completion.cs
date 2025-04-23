using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Completion
    {
        public Completion()
        {
            Status = "pending";
            TransactionId = string.Empty;
            AffiliateNetwork = string.Empty;
            CompletionDetails = string.Empty;
            IpAddress = string.Empty;
            ReferralCommissions = new List<ReferralCommission>();
        }
        
        [Key]
        public int CompletionId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? TaskId { get; set; }
        
        [Required]
        public DateTime CompletedAt { get; set; }
        
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PointsAwarded { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? UsdValue { get; set; }
        
        [Required, StringLength(20)]
        public string Status { get; set; }
        
        [StringLength(100)]
        public string TransactionId { get; set; }
        
        [Required, StringLength(50)]
        public string AffiliateNetwork { get; set; }
        
        public string CompletionDetails { get; set; }
        
        [StringLength(45)]
        public string IpAddress { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        [ForeignKey("TaskId")]
        public OfferTask? Task { get; set; }
        
        public ICollection<ReferralCommission> ReferralCommissions { get; set; }
    }
}