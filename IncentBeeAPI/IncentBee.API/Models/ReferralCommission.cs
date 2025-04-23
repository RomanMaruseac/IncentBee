using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class ReferralCommission
    {
        public ReferralCommission()
        {
            Status = "paid";
        }
        
        [Key]
        public int CommissionId { get; set; }
        
        [Required]
        public int ReferralId { get; set; }
        
        [Required]
        public int CompletionId { get; set; }
        
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required, StringLength(20)]
        public string Status { get; set; }
        
        // Navigation properties
        [ForeignKey("ReferralId")]
        public Referral? Referral { get; set; }
        
        [ForeignKey("CompletionId")]
        public Completion? Completion { get; set; }
    }
}