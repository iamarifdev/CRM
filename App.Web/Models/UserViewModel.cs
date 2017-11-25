using System.ComponentModel.DataAnnotations;
using App.Entity.Models;

namespace App.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "User Name")]
        [RegularExpression("^[a-zA-z]+$", ErrorMessage = "Username can only contain letters.")]
        [System.Web.Mvc.Remote("IsUserAvailable", "Users", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Username already taken, try another.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "User Email")]
        [System.Web.Mvc.Remote("IsEmailAvailable", "Users", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Email already in use, try another.")]
        public string Email { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        public UserLevel Level { get; set; }

        [Display(Name = "Group Name")]
        public int GroupId { get; set; }
    }
}