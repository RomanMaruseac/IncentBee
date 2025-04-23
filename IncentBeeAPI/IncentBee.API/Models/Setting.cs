using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Setting
    {
        public Setting()
        {
            SettingKey = string.Empty;
            SettingValue = string.Empty;
            Category = string.Empty;
            Description = string.Empty;
        }
        
        [Key]
        public int SettingId { get; set; }
        
        [Required, StringLength(100)]
        public string SettingKey { get; set; }
        
        [Required]
        public string SettingValue { get; set; }
        
        [Required, StringLength(50)]
        public string Category { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        public int? UpdatedBy { get; set; }
        
        // Navigation properties
        [ForeignKey("UpdatedBy")]
        public User? User { get; set; }
    }
}