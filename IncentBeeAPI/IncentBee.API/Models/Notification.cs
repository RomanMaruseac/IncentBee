using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncentBee.API.Models
{
    public class Notification
    {
        public Notification()
        {
            Title = string.Empty;
            Message = string.Empty;
            Type = string.Empty;
            LinkUrl = string.Empty;
        }
        
        [Key]
        public int NotificationId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required, StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ReadAt { get; set; }
        
        [Required, StringLength(50)]
        public string Type { get; set; }
        
        [StringLength(255)]
        public string LinkUrl { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}