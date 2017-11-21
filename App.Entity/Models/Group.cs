using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class Group
    {
        public Group()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public Flag Crm { get; set; }
        public Flag Billing { get; set; }
        public Flag Account { get; set; }
        public Flag Report { get; set; }
        public Flag Hrm { get; set; }
        public Flag Setup { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
