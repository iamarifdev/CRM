using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using App.Web.Context;
using App.Entity.Models;
using App.Web.Helper;
using EntityFramework.Extensions;
using WebGrease.Css.Extensions;

namespace App.Web.Controllers
{
    [CrmAuthorize(Roles = "Admin")]
    public class GeneralSettingsController : Controller
    {
        #region Private Fields

        private readonly CrmDbContext _db = new CrmDbContext();
        private readonly List<string> _allowedLogoFileTypes = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        #endregion

        // GET: GeneralSettings
        public ActionResult Index()
        {
            var generalSettings = _db.GeneralSettings.ToDictionary(x => x.SettingName);

            ViewData["Address"] = generalSettings.ContainsKey("Address") ? generalSettings["Address"].SettingValue : string.Empty;
            //ViewData["SiteLogo"] = generalSettings[1].ToString();
            ViewData["SiteName"] = generalSettings.ContainsKey("SiteName") ? generalSettings["SiteName"].SettingValue : string.Empty;
            ViewData["SmsPassword"] = generalSettings.ContainsKey("SmsPassword") ? generalSettings["SmsPassword"].SettingValue : string.Empty;
            ViewData["SmsSender"] = generalSettings.ContainsKey("SmsSender") ?generalSettings["SmsSender"].SettingValue : string.Empty;
            ViewData["SmsUrl"] = generalSettings.ContainsKey("SmsUrl") ? generalSettings["SmsUrl"].SettingValue : string.Empty;
            ViewData["SmsUser"] = generalSettings.ContainsKey("SmsUser") ? generalSettings["SmsUser"].SettingValue : string.Empty;

            ViewData["IsLogoExist"] = _db.GeneralSettings.Count(x => x.SettingName == "SiteLogo") > 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var generalSettings = new List<GeneralSetting>();
            var siteLogo = Request.Files["SiteLogo"];

            // ReSharper disable once PossibleNullReferenceException
            if (siteLogo.ContentLength > 0)
            {
                // 1048567 bytes = 1 MegaBytes
                if (siteLogo.FileName == string.Empty || siteLogo.ContentLength > 1048576)
                {
                    TempData["Toastr"] = Toastr.CustomError("Max file size: 1 MB!");
                    return RedirectToAction("Index");
                }
                var extension = Path.GetExtension(siteLogo.FileName);

                if (extension == null) return View();
                extension = extension.ToLower();
                if (_allowedLogoFileTypes.IndexOf(extension) == -1)
                {
                    TempData["Toastr"] = Toastr.CustomError("Only .png, .jpg, .jpeg, .gif, .bmp file types allowed.");
                    return RedirectToAction("Index");
                }

                var image = Image.FromStream(siteLogo.InputStream);
                if (image.Width != 256 || image.Height != 256)
                {
                    TempData["Toastr"] = Toastr.CustomError("Image size should be 256 px X 256 px.");
                    return RedirectToAction("Index");
                }

                var setting = new GeneralSetting { SettingName = "SiteLogo", SettingValue = siteLogo.FileName };
                generalSettings.Add(setting);
                siteLogo.SaveAs(Server.MapPath("~/Content/Template/img/site/site-logo" + extension));
            }

            collection.AllKeys.ForEach(key =>
            {
                var setting = new GeneralSetting { SettingName = key, SettingValue = collection[key].ToString() };
                generalSettings.Add(setting);
            });

            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    generalSettings.ForEach(setting =>
                    {
                        if (_db.GeneralSettings.Any(x => x.SettingName == setting.SettingName))
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
                catch (Exception ex)
                {
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    dbTransaction.Rollback();
                    return RedirectToAction("Index");
                }
            }
            TempData["Toastr"] = Toastr.CustomSuccess("General Setting Saved!");
            return RedirectToAction("Index", "GeneralSettings");
        }

        public JsonResult IsLogoHaveToAdd()
        {
            var flag = _db.GeneralSettings.Any(x => x.SettingName == "SiteLogo");
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDbBackUp()
        {
            try
            {
                var connectionString = WebConfigurationManager.ConnectionStrings["CrmDbContext"].ConnectionString;
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;

                var backUpDirectory = Server.MapPath("~/Backup/");
                var fileName = "CRM_" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".bak";
                var backupPath = Path.Combine(backUpDirectory, fileName);
                if (!Directory.Exists(backUpDirectory)) Directory.CreateDirectory(backUpDirectory);
                foreach (var file in Directory.GetFiles(backUpDirectory, "*.bak").Where(item => item.EndsWith(".bak")))
                {
                    System.IO.File.Delete(file);
                }

                var query = string.Format("BACKUP DATABASE [{0}] TO DISK ='{1}'", databaseName, backupPath);
                _db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, query);

                var fileBytes = System.IO.File.ReadAllBytes(backupPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}