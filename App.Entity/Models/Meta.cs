using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class Meta
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(10)]
        public string CreditGiven { get; set; }

        [Required]
        [StringLength(10)]
        public string CreditAvailable { get; set; }

        [Required]
        [StringLength(10)]
        public string Commands { get; set; }
    }
}