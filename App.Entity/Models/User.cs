using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class User
    {
        //public User()
        //{
        //    Groups = new List<Group>();
        //}
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Uid { get; set; }

        [Required]
        [StringLength(45)]
        [Display(Name = "IP Address")]
        public string IpAddress { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        public UserLevel Level { get; set; }

        [Display(Name = "Group Name")]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        //public virtual ICollection<Group> Groups { get; set; }

    }
}