using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;

namespace App.Web.Helper
{
    public static class Common
    {
        public static string GetUserName(this int id)
        {
            using (var db = new CrmDbContext())
            {
                return db.Users.First(u => u.Id == id).Username;
            }
        }

        public static List<SelectListItem> StatusList {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new
                       SelectListItem {Text = v.ToString(), Value = ((int) v).ToString()}).ToList();
            }
        }
    }
}