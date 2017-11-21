using System.Collections.Generic;
using App.Entity.Models;

namespace App.Web.Models
{
    public class AppData
    {
        public string UserName { get; set; }
        public Group Group { get; set; } 
        public ICollection<Menu> MenuList { get; set; }
        public bool IsDevelopmentMode { get; set; }
    }
}