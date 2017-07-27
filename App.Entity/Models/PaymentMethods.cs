using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class PaymentMethods
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MethodId { get; set; }

        [Required]
        [StringLength(50)]
        public string MethodName { get; set; }

        public decimal CurrentValue { get; set; }
    }
}