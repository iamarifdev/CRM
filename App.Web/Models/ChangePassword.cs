using System.ComponentModel.DataAnnotations;

namespace App.Web.Models
{
    public class ChangePassword
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be 6-50 character.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}