using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class PaymentMethod : BaseModel
    {

        [Required]
        [StringLength(50)]
        [Display(Name = "Method Id")]
        public string MethodId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Method Name")]
        public string MethodName { get; set; }

        [DefaultValue(0.00)]
        [Display(Name = "Current Value")]
        public decimal CurrentValue { get; set; }
    }
}