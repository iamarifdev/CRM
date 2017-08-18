using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace App.Entity.Models
{
    public class ServiceInfo : BaseModel
    {
        [Required]
        [StringLength(20)]
        [Index("IX_Service_Id", 1, IsUnique = true)]
        [Display(Name = "Service Id")]
        public string ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        [Index("IX_Service_Name", 2, IsUnique = true)]
        [Remote("IsServiceAvailable", "Services", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "The Service already exists! Try new one!")]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public Status Status { get; set; }
    }
}