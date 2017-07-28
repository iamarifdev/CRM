using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using App.Data.Context;
using App.Entity.Models;
using EntityFramework.Extensions;
using EntityFramework.Utilities;
using WebGrease.Css.Extensions;

namespace App.Web.Controllers
{
    public class GeneralSettingsController : Controller
    {
        #region Private Fields

        private readonly CrmDbContext _db = new CrmDbContext();
        private readonly List<string> _allowedLogoFileTypes = new List<string> { ".png", "jpg", ".jpeg", ".gif", ".bmp" };

        #endregion

        // GET: GeneralSettings
        public ActionResult Index()
        {
            var generalSettings = _db.GeneralSettings.ToDictionary(x => x.SettingName);
            ViewData["Address"] = generalSettings["Address"].SettingName ?? string.Empty;
            //ViewData["SiteLogo"] = generalSettings[1].ToString();
            ViewData["SiteName"] = generalSettings["SiteName"].SettingValue ?? string.Empty;
            ViewData["SmsPassword"] = generalSettings["SmsPassword"].SettingValue ?? string.Empty;
            ViewData["SmsSender"] = generalSettings["SmsSender"].SettingValue ?? string.Empty;
            ViewData["SmsUrl"] = generalSettings["SmsUrl"].SettingValue ?? string.Empty;
            ViewData["SmsUser"] = generalSettings["SmsUser"].SettingValue ?? string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var generalSettings = new List<GeneralSetting>();
            var siteLogo = Request.Files["SiteLogo"];
            
            if (siteLogo.ContentLength > 0 )
            {
                // 1048567 bytes = 1 MegaBytes
                if (siteLogo.FileName == string.Empty || siteLogo.ContentLength > 1048576) return View();
                var extension = Path.GetExtension(siteLogo.FileName);
                
                if (extension == null) return View();
                extension = extension.ToLower();
                if (_allowedLogoFileTypes.IndexOf(extension) == -1) return View();

                var image = Image.FromStream(siteLogo.InputStream);
                if (image.Width != 256 || image.Height != 256) return View();

                var setting = new GeneralSetting {SettingName = "SiteLogo", SettingValue = siteLogo.FileName};
                generalSettings.Add(setting);
                siteLogo.SaveAs(Server.MapPath("~/Content/Template/img/site/site-logo"+extension));
            }

            collection.AllKeys.ForEach(key =>
            {
                var setting = new GeneralSetting {SettingName = key, SettingValue = collection[key].ToString()};
                generalSettings.Add(setting);
            });

            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    generalSettings.ForEach(setting =>
                    {
                        if (_db.GeneralSettings.Count(x => x.SettingName == setting.SettingName) > 0)
                        {
                            _db.GeneralSettings
                                .Where(x => x.SettingName == setting.SettingName)
                                .Update(u => new GeneralSetting { SettingValue = setting.SettingValue });

                            _db.SaveChanges();
                        }
                        else
                        {
                            _db.GeneralSettings.Add(setting);
                            _db.SaveChanges();
                        }
                    });

                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                }
            }
            
            return RedirectToAction("Index", "GeneralSettings");
        }

        public JsonResult IsLogoExist()
        {
            var flag =  _db.GeneralSettings.Count(x => x.SettingName == "SiteLogo") > 0;
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}