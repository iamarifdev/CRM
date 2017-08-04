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
        public bool Crm { get; set; }

        [Required]
        public bool Billing { get; set; }

        [Required]
        public bool Accounts { get; set; }

        [Required]
        public bool Report { get; set; }

        [Required]
        public bool Hrm { get; set; }

        [Required]
        public bool Setup { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}