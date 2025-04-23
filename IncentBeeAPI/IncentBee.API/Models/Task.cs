using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        
        [Required, StringLength(100)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PointsReward { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? UsdValue { get; set; }
        
        [Required, StringLength(50)]
        public string AffiliateNetwork { get; set; }
        
        [StringLength(100)]
        public string ExternalOfferId { get; set; }
        
        [Required, StringLength(20)]
        public string OfferType { get; set; }
        
        [Required, StringLength(20)]
        public string Status { get; set; } = "active";
        
        public string Requirements { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        
        // Navigation properties
        public ICollection<Completion> Completions { get; set; }
    }
}