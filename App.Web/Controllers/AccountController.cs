using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using App.Web.Models;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Web.Controllers
{
    [CrmAuthorize]
    public class AccountController : Controller
    {
        private readonly CrmDbContext _db = new CrmDbContext();
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                switch (result)
                {
                    case SignInStatus.Success:

                        var appData = new AppData();
                        var userName = model.UserName;
                        var environment = System.Configuration.ConfigurationManager.AppSettings["Environment"];
                        var query = _db.Menus.Where(x => x.Status == Status.Active).AsQueryable();
                        var userImgUrl = "/Content/template/img/avatars/default.png";
                        if (_db.Users.Any(x => x.UserName == userName && x.Status == Status.Active))
                        {
                            appData.Group = _db.Users.Include(x => x.Group).First(x => x.UserName == userName).Group;
                            userImgUrl = _db.Users
                                .Where(x => x.UserName == userName && x.EmployeeId != null)
                                .Select(x=> _db.EmployeeBasicInfos.Where(w=> w.Id == x.EmployeeId).Select(s=>s.ImageUrl).FirstOrDefault())
                                .FirstOrDefault() ?? userImgUrl;
                        }

                        else if (_db.AgentInfos.Any(x => x.UserName == userName && x.Status == Status.Active))
                            appData.Group = new Group {Crm = true, Billing = true};
                        else
                        {
                            Session.RemoveAll();
                            Session.Abandon();
                            ModelState.AddModelError("UserName", @"User name may be invalid!");
                            ModelState.AddModelError("Password", @"Entered Password may be wrong!");
                            return View(model);
                        }

                        if (_db.Menus.Any())
                        {
                            if (!appData.Group.Account) query = query.Where(x => x.ModuleName != Module.Account);
                            if (!appData.Group.Billing) query = query.Where(x => x.ModuleName != Module.Billing);
                            if (!appData.Group.Crm) query = query.Where(x => x.ModuleName != Module.Crm);
                            if (!appData.Group.Hrm) query = query.Where(x => x.ModuleName != Module.Hrm);
                            if (!appData.Group.Report) query = query.Where(x => x.ModuleName != Module.Report);
                            if (!appData.Group.Setup) query = query.Where(x => x.ModuleName != Module.Setup);
                        }
                        appData.MenuList = query.ToList();
                        appData.UserName = userName;
                        appData.UserImgUrl = userImgUrl;
                        appData.IsDevelopmentMode = environment == "DEV";
                        appData.CompanyName = _db.GeneralSettings.Where(x => x.SettingName == "SiteName").Select(x => x.SettingValue)
                            .FirstOrDefault() ?? "CRM";
                        appData.SiteLogo =_db.GeneralSettings.Where(x => x.SettingName == "SiteLogo").Select(x => x.SettingValue)
                            .FirstOrDefault() ?? "site-logo.png";
                        Session.Set("AppData",appData);

                        return RedirectToLocal(returnUrl);

                    case SignInStatus.LockedOut:
                        return View("Lockout");

                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                    //case SignInStatus.Failure:

                    default:
                        ModelState.AddModelError("UserName", @"User name may be invalid!");
                        ModelState.AddModelError("Password", @"Entered Password may be wrong!");
                        return View(model);
                }
            }
            catch(Exception ex)
            {
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToRoute(new RouteValueDictionary
                {
                    {"action", "InternalServerError"},
                    {"controller", "Error"}
                });
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe,
                        rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.UserRoles = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name",
                "Name");

            ViewBag.BranchId = new SelectList(_db.BranchInfos.ToList(), "BranchId", "BranchName");
            //ViewBag.EmployeeId = new SelectList(_db.EmployeeBasicInfos.ToList(), "EmployeeId", "EmployeeName");
            ViewBag.Active = Common.ToSelectList<Status>();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    await UserManager.AddToRoleAsync(user.Id, model.UserRoles);

                    return RedirectToAction("Index", "Users");
                }
                ViewBag.UserRoles = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin"))
                    .ToList(), "Name", "Name");
                ViewBag.BranchId = new SelectList(_db.BranchInfos.ToList(), "BranchId", "BranchName");
                //ViewBag.EmployeeId = new SelectList(_db.EmployeeBasicInfos.ToList(), "EmployeeId", "EmployeeName");
                ViewBag.Active = Common.ToSelectList<Status>();
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return
                View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode",
                new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                //case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                if(Session["AppData"] != null) Session.Clean("AppData");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new RouteValueDictionary
                {
                    {"action", "InternalServerError"},
                    {"controller", "Error"}
                });
            }
            
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        [HttpPost]
        public JsonResult IsAgentAvailable(string username, int? id)
        {
            try
            {
                var flag = true;
                //create mode
                if (id == null)
                {
                    flag = !_context.Users.Any(x => x.UserName == username);
                }
                // edit mode
                else
                {
                    var agent = _db.AgentInfos.Find(id);
                    if (agent == null) return Json(false, JsonRequestBehavior.AllowGet);
                    if (agent.UserName != username) flag = !_context.Users.Any(x => x.UserName == username);
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var model = new ChangePassword
            {
                Username = User.Identity.GetUserName()
            };
            return View(model);
        }

        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmChangePassword(ChangePassword model)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (!ModelState.IsValid) return RedirectToAction("ChangePassword");
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                    var user = manager.FindByName(model.Username);

                    if (manager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.OldPassword) == PasswordVerificationResult.Failed)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Password Validation!", "Your given password does not match, try again.");
                        return RedirectToAction("ChangePassword");
                    }

                    if (!Common.ChangePassword(user, model.Password))
                    {
                        TempData["Toastr"] = Toastr.DbError(string.Empty);
                        return RedirectToAction("ChangePassword");
                    }

                    if (User.IsInRole("Agent"))
                    {
                        _db.AgentInfos.Where(x => x.UserName == model.Username)
                            .Update(x => new AgentInfo { Password = model.Password });
                    }

                    scope.Complete();
                    TempData["Toastr"] = Toastr.Updated;
                    return RedirectToAction("ChangePassword");

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("ChangePassword");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}