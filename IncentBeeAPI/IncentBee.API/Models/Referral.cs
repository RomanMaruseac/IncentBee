using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Referral
    {
        [Key]
        public int ReferralId { get; set; }
        
        [Required]
        public int ReferrerId { get; set; }
        
        [Required]
        public int ReferredId { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required, StringLength(20)]
        public string Status { get; set; } = "active";
        
        // Navigation properties
        [ForeignKey("ReferrerId")]
        public User Referrer { get; set; }
        
        [ForeignKey("ReferredId")]
        public User Referred { get; set; }
        
        public ICollection<ReferralCommission> Commissions { get; set; }
    }
}