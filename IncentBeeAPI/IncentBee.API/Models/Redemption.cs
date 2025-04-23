using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Redemption
    {
        [Key]
        public int RedemptionId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int RewardId { get; set; }
        
        [Required]
        public DateTime RedemptionDate { get; set; }
        
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PointsSpent { get; set; }
        
        [Required, StringLength(20)]
        public string Status { get; set; } = "pending";
        
        public string FulfillmentDetails { get; set; }
        
        public string RejectionReason { get; set; }
        
        public string AdminNotes { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [ForeignKey("RewardId")]
        public Reward Reward { get; set; }
    }
}