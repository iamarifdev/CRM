using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Web.Context;

namespace App.Web.Helper
{
    public static class HelperExtension
    {
        public static string GetUserName(this int id)
        {
            using (var db = new CrmDbContext())
            {
                return db.Users.First(u => u.Id == id).Username;
            }
        }
    }
}