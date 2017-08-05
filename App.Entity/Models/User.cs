using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class User
    {
        public User()
        {
            UserGroups = new List<UserGroup>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Uid { get; set; }

        public int GroupId { get; set; }

        [Required]
        [StringLength(45)]
        public string IpAddress { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        //[Required]
        //[StringLength(255)]
        //public string Password { get; set; }

        //[StringLength(255)]
        //public string Salt { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        //[StringLength(40)]
        //public string ActivationCode { get; set; }

        //[StringLength(40)]
        //public string ForgottenPasswordCode { get; set; }
        //public long? ForgottenPasswordTime { get; set; }

        //[StringLength(40)]
        //public string RememberCode { get; set; }
        public long CreatedOn { get; set; }

        public long? LastLogin { get; set; }

        public Status? Active { get; set; }

        //[Required]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        //[Required]
        [StringLength(20)]
        public string BranchId { get; set; }

        [Required]
        [StringLength(20)]
        public string Level { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}