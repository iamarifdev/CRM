using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class GeneralSetting
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string SettingName { get; set; }

        [Required]
        [StringLength(50)]
        public string SettingValue { get; set; }
    }
}