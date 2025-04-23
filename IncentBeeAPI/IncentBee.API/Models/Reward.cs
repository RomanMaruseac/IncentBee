using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Reward
    {
        [Key]
        public int RewardId { get; set; }
        
        [Required, StringLength(100)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        [StringLength(255)]
        public string ImageUrl { get; set; }
        
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PointsCost { get; set; }
        
        public int? StockQuantity { get; set; }
        
        [Required, StringLength(20)]
        public string Status { get; set; } = "available";
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [StringLength(50)]
        public string Category { get; set; }
        
        [Required]
        public bool Featured { get; set; } = false;
        
        // Navigation properties
        public ICollection<Redemption> Redemptions { get; set; }
    }
}