using System.Collections.Generic;
using App.Entity.Models;

namespace App.Web.Models
{
    public class AppData
    {
        public AppData()
        {
            Group = new Group();
            MenuList = new List<Menu>();
        }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string UserImgUrl { get; set; }
        public Group Group { get; set; } 
        public ICollection<Menu> MenuList { get; set; }
        public bool IsDevelopmentMode { get; set; }
    }
}