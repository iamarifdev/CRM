using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class LoginAttempts
    {
        public long Id { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "IP Address")]
        public string IpAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }
        public long? Time { get; set; }
    }
}