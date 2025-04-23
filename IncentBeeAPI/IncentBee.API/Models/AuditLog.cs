using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }
        
        public int? UserId { get; set; }
        
        [Required, StringLength(100)]
        public string Action { get; set; }
        
        [Required, StringLength(50)]
        public string EntityType { get; set; }
        
        [Required, StringLength(50)]
        public string EntityId { get; set; }
        
        public string Details { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [StringLength(45)]
        public string IpAddress { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}