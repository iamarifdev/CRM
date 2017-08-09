using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class SuppliersInfo : BaseModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Supplier Id")]
        public string SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Supplier Email")]
        public string SupplierEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Supplier Phone")]
        public string SupplierPhone { get; set; }

        [StringLength(250)]
        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Supplier Mobile No.")]
        public string SupplierMobileNo { get; set; }
    }
}