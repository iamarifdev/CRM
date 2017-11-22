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
        [StringLength(40, MinimumLength = 3)]
        [System.Web.Mvc.Remote("IsGroupAvailable", "Groups", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Group Name already exist, try another.")]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public bool Crm { get; set; }
        public bool Billing { get; set; }
        public bool Account { get; set; }
        public bool Report { get; set; }
        public bool Hrm { get; set; }
        public bool Setup { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
