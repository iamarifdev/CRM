using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class SuppliersInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string SupplierPhone { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string SupplierAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string SupplierMobileNo { get; set; }
    }
}