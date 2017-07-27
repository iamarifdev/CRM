using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class Group
    {
        public Group()
        {
            UserGroups = new List<UserGroup>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(5)]
        public string Crm { get; set; }

        [Required]
        [StringLength(5)]
        public string Billing { get; set; }

        [Required]
        [StringLength(5)]
        public string Accounts { get; set; }

        [Required]
        [StringLength(5)]
        public string Report { get; set; }

        [Required]
        [StringLength(5)]
        public string Hrm { get; set; }

        [Required]
        [StringLength(5)]
        public string Setup { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}