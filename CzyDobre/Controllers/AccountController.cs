using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CzyDobre.Models;
using reCAPTCHA.MVC;
using CzyDobre.Extensions;

namespace CzyDobre.Controllers
{
    //CzyDobre.pl
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;

        public AccountController()
        {
            _context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        //CzyDobre.pl/logowanie
        [Route("logowanie")]
        [Route("Account/Login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        //CzyDobre.pl/logowanie
        [Route("logowanie")]
        [Route("Account/Login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");

                    ViewBag.errorMessage = "Musisz zweryfikować swoje konto, aby móc się zalogować";
                    return View("Error");
                }
            }

            // Nie powoduje to liczenia niepowodzeń logowania w celu zablokowania konta
            // Aby włączyć wyzwalanie blokady konta po określonej liczbie niepomyślnych prób wprowadzenia hasła, zmień ustawienie na shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Nieprawidłowa próba logowania.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        //CzyDobre.pl/zweryfikuj-kod
        [Route("zweryfikuj-kod")]
        [Route("Account/VerifyCode")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Wymagaj, aby użytkownik zalogował się już za pomocą nazwy użytkownika/hasła lub logowania zewnętrznego
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        //CzyDobre.pl/zweryfikuj-kod
        [Route("zweryfikuj-kod")]
        [Route("Account/VerifyCode")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Poniższy kod chroni przed atakami na zasadzie pełnego przeglądu kodu dwuczynnikowego. 
            // Jeśli użytkownik będzie wprowadzać niepoprawny kod przez określoną ilość czasu, konto użytkownika 
            // zostanie zablokowane na określoną ilość czasu. 
            // Możesz skonfigurować ustawienia blokady konta w elemencie IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Nieprawidłowy kod.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        //CzyDobre.pl/rejestracja
        [Route("rejestracja")]
        [Route("Account/Register")]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        //CzyDobre.pl/rejestracja
        [Route("rejestracja")]
        [Route("Account/Register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidator(ErrorMessage = "Nieprawidłowe roziązanie pola Captcha", RequiredMessage = "Pole Captcha jest wymagane.")]
        public async Task<ActionResult> Register(RegisterViewModel model, bool captchaValid)
        {
            if (ModelState.IsValid && captchaValid==true)
            {
                var user = new ApplicationUser
                {
                    NickName = model.NickName,
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                await this.UserManager.AddToRolesAsync(user.Id, "User");
                if (result.Succeeded)
                {
                    //  Comment the following line to prevent log in until the user is confirmed.
                    //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // Aby uzyskać więcej informacji o sposobie włączania potwierdzania konta i resetowaniu hasła, odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=320771
                    // Wyślij wiadomość e-mail z tym łączem
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    string html = "<a href=\"" + callbackUrl + "\">link</a><br/>"; 
                    await UserManager.SendEmailAsync(user.Id, "Aktywacja Konta", "<h3> Aktywuj swoje konto klikając w ten "+ html + "lub kopiując poniższy link bezpośrednio do przeglądarki </h3>" + callbackUrl);
                    // Uncomment to debug locally 
                    // TempData["ViewBagLink"] = callbackUrl;

                    //string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Potwierdz swoje konto");

                    ViewBag.Message = "Sprawdź swoją skrzynkę e-mail ,aby potwierdzić swoje konto"

                        + " w przeciwnym wypadku nie uda Ci się zalogować :)!";

                    return View("Info");
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            return View(model);
        }

        //
        // GET: /Account/RegisterRole
        //CzyDobre.pl/rejestracja-roli
        [Route("przypisanie-roli")]
        [Route("Account/RegisterRole")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RegisterRole()
        {
            ViewBag.Name = new SelectList(_context.Roles.ToList(), "Name", "Name");
            ViewBag.UserName = new SelectList(_context.Users.ToList(), "UserName", "UserName");
            return View();
        }

        //
        // POST: /Account/RegisterRole
        //CzyDobre.pl/rejestracja-roli
        [Route("przypisanie-roli")]
        [Route("Account/RegisterRole")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterRole(RegisterViewModel model, ApplicationUser user)
        {
            try
            {
                var userId = _context.Users.Where(i => i.UserName == user.UserName).Select(s => s.Id);
                string updateId = "";
                foreach (var item in userId)
                {
                    updateId = item.ToString();
                }

                await this.UserManager.AddToRolesAsync(updateId, model.Name);
                this.AddNotification("Operacja wykonana pomyślnie!", NotificationType.SUCCESS);
                return RedirectToAction("RegisterRole", "Account");
            }
            catch (Exception ex)
            {
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("RegisterRole", "Account");
            }
        }

        //
        // GET: /Account/RegisterRole
        //CzyDobre.pl/rejestracja-roli
        [Route("usuwanie-przypisanej-roli")]
        [Route("Account/DeleteRole")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeleteRole()
        {
            ViewBag.Name = new SelectList(_context.Roles.ToList(), "Name", "Name");
            ViewBag.UserName = new SelectList(_context.Users.ToList(), "UserName", "UserName");
            return View();
        }

        //
        // POST: /Account/RegisterRole
        //CzyDobre.pl/rejestracja-roli
        [Route("usuwanie-przypisanej-roli")]
        [Route("Account/DeleteRole")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole(RegisterViewModel model, ApplicationUser user)
        {
            try
            {
                var userId = _context.Users.Where(i => i.UserName == user.UserName).Select(s => s.Id);
                string updateId = "";

                foreach (var item in userId)
                {
                    updateId = item.ToString();
                }

                await this.UserManager.RemoveFromRoleAsync(updateId, model.Name);
                this.AddNotification("Operacja wykonana pomyślnie!", NotificationType.SUCCESS);
                return RedirectToAction("DeleteRole", "Account");
            }
            catch (Exception ex)
            {
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("DeleteRole", "Account");
            }
        }

        //
        // GET: /Account/ConfirmEmail
        //CzyDobre.pl/potwierdz-email
        [Route("potwierdz-email")]
        [Route("Account/ConfirmEmail")]
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
        //CzyDobre.pl/zapomniales-hasla
        [Route("zapomniales-hasla")]
        [Route("Account/ForgotPassword")]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        //CzyDobre.pl/zapomniales-hasla
        [Route("zapomniales-hasla")]
        [Route("Account/ForgotPassword")]
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
                    // Nie ujawniaj informacji o tym, że użytkownik nie istnieje lub nie został potwierdzony
                    return View("ForgotPasswordConfirmation");
                }

                // Aby uzyskać więcej informacji o sposobie włączania potwierdzania konta i resetowaniu hasła, odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=320771
                // Wyślij wiadomość e-mail z tym łączem
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Resetuj hasło", "<h3> Resetuj hasło, klikając <a href=\"" + callbackUrl + "\">tutaj</a> </h3>");
                //await UserManager.SendEmailAsync(user.Id, "Resetuj hasło", "Resetuj hasło, klikając <a href=\"" + callbackUrl + "\">tutaj</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        //CzyDobre.pl/zapomniane-haslo-potwierdzenie
        [Route("zapomniane-haslo-potwierdzenie")]
        [Route("Account/ForgotPasswordConfirmation")]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        //CzyDobre.pl/reset-hasla
        [Route("reset-hasla")]
        [Route("Account/ResetPassword")]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        //CzyDobre.pl/reset-hasla
        [Route("reset-hasla")]
        [Route("Account/ResetPassword")]
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
                // Nie ujawniaj informacji o tym, że użytkownik nie istnieje
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
        //CzyDobre.pl/reset-hasla-potwierdzenie
        [Route("reset-hasla-potwierdzenie")]
        [Route("Account/ResetPasswordConfirmation")]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        //CzyDobre.pl/logowanie-zewnetrzne
        [Route("logowanie-zewnetrzne")]
        [Route("Account/ExternalLogin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Żądaj przekierowania do dostawcy logowania zewnętrznego
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        //CzyDobre.pl/wyslij-kod
        [Route("wyslij-kod")]
        [Route("Account/SendCode")]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        //CzyDobre.pl/wyslij-kod
        [Route("wyslij-kod")]
        [Route("Account/SendCode")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Wygeneruj i wyślij token
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        //CzyDobre.pl/logowanie-zewnetrzne-odpowiedz
        [Route("logowanie-zewnetrzne-odpowiedz")]
        [Route("Account/ExternalLoginCallback")]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Zaloguj użytkownika przy użyciu tego dostawcy logowania zewnętrznego, jeśli użytkownik ma już nazwę logowania
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Jeśli użytkownik nie ma konta, poproś go o utworzenie konta
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        //CzyDobre.pl/logowanie-zewnetrzne-potwierdzenie
        [Route("logowanie-zewnetrzne-potwierdzenie")]
        [Route("Account/ExternalLoginConfirmation")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }
            DBEntities db = new DBEntities(); 


            if (ModelState.IsValid)
            {
                // Uzyskaj informacje o użytkowniku od dostawcy logowania zewnętrznego
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser 
                { 
                    UserName = model.Email,
                    Email = model.Email,
                    NickName = model.NickName
                };
                var role = db.AspNetRoles.Where(r => r.Name == "User").Select(r=>r.Id).ToString();
                var result = await UserManager.CreateAsync(user);
                //await this.UserManager.AddToRole(user.Id,role );
                


                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddToRoleAsync(user.Id, "User");
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //CzyDobre.pl/wyloguj
        [Route("wyloguj")]
        [Route("Account/LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        //CzyDobre.pl/blad-logowania-zewnetrznego
        [Route("blad-logowania-zewnetrznego")]
        [Route("Account/ExternalLoginFailure")]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        // Przeglądanie profili użytkowników
        [Route("ProfilUzytkownika")]
        [Route("Account/ProfilUzytkownika/{id?}")]
        [AllowAnonymous]
        public ActionResult UserProfile(string id)
        {
            BrowseUserProfile userProfile = new BrowseUserProfile();
            DBEntities db = new DBEntities();

            var dataStream = (from AspNetUser in db.AspNetUsers
                           where AspNetUser.Id == id
                           select new
                           {
                               NickName = AspNetUser.NickName,
                               avatarURL = AspNetUser.AvatarUrl
                           }).FirstOrDefault();

            if (dataStream != null)
            {
                userProfile.NickName = dataStream.NickName;
                if (dataStream.avatarURL != null)
                {
                    userProfile.AvatarURL = dataStream.avatarURL;
                }
                userProfile.Opinions = new System.Collections.Generic.List<OpinionViewModels>();

                var dataStreamOpinions = ((
                               from AspNetRating in db.AspNetRatings
                               join AspNetProduct in db.AspNetProducts on AspNetRating.Id_Product equals AspNetProduct.Id_Product
                               join AspNetUser in db.AspNetUsers on AspNetRating.Who equals AspNetUser.Id
                               where AspNetUser.Id == id
                               select new
                               {
                                   RateService = AspNetRating.RateService,
                                   RateTaste = AspNetRating.RateTaste,
                                   RateIngredients = AspNetRating.RateIngredients,
                                   OpinionId = AspNetRating.Id_Rating,
                                   AddedDate = AspNetRating.Date,
                                   ProductName = AspNetProduct.ProductName
                               }));

                foreach (var data in dataStreamOpinions)
                {
                    string ImageUrl = (from AspNetRatingPicture in db.AspNetRatingPictures
                                       where AspNetRatingPicture.Id_Rating == data.OpinionId
                                       select AspNetRatingPicture.Url).FirstOrDefault();

                    OpinionViewModels opinion = new OpinionViewModels();

                    opinion.AddedDate = (DateTime)data.AddedDate;
                    opinion.RateService = data.RateService.ToString();
                    opinion.RateTaste = data.RateTaste.ToString();
                    opinion.RateIngredients  = data.RateIngredients.ToString();
                    opinion.ProductName = data.ProductName.ToString();
                    opinion.RatingId = data.OpinionId;
                    opinion.ImageUrls = new System.Collections.Generic.List<string> { ImageUrl };

                    userProfile.Opinions.Add(opinion);
                }
                
                return View(userProfile);
            }
            else
            {
                return RedirectToAction("Index", "Home");
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

        #region Pomocnicy
        // Używane w przypadku ochrony XSRF podczas dodawania logowań zewnętrznych
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

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            string html = "<a href=\"" + callbackUrl + "\">link</a><br/>";
            await UserManager.SendEmailAsync(userID, "Aktywacja Konta", "<h3> Aktywuj swoje konto klikając w ten " + html + "lub kopiując poniższy link bezpośrednio do przeglądarki </h3>" + callbackUrl);

            return callbackUrl;
        }


        #endregion
    }
}