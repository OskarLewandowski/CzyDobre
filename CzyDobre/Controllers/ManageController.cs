using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CzyDobre.Models;
using CzyDobre.Extensions;
using System.Configuration;
using System.Net.Mail;
using CloudinaryDotNet;
using System.Collections.Generic;
using System.IO;
using CloudinaryDotNet.Actions;

namespace CzyDobre.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult MyData()
        {
            DBEntities db = new DBEntities();
            MyDataViewModel dane = new MyDataViewModel();
            var userId = User.Identity.GetUserId();

            try
            {
                if (userId != null)
                {
                    var nickName = db.AspNetUsers.Where(m => m.Id == userId).Select(m => m.NickName).FirstOrDefault();
                    var firstName = db.AspNetUsers.Where(m => m.Id == userId).Select(m => m.FirstName).FirstOrDefault();
                    var lastName = db.AspNetUsers.Where(m => m.Id == userId).Select(m => m.LastName).FirstOrDefault();

                    dane.Id = userId;
                    dane.FirstName = firstName;
                    dane.LastName = lastName;
                    dane.NickName = nickName;

                    if (dane != null)
                    {
                        return View(dane);
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
            }
            this.AddNotification($"Ups!, napotkaliśmy pewien problem. Proszę spróbować za kilka minut.", NotificationType.ERROR);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveMyData(MyDataViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DBEntities db = new DBEntities();
                    var userId = User.Identity.GetUserId();
                    var aspNetUser = db.AspNetUsers.FirstOrDefault(m => m.Id == userId);

                    aspNetUser.FirstName = model.FirstName;
                    aspNetUser.LastName = model.LastName;
                    aspNetUser.NickName = model.NickName;

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    this.AddNotification($"Dane, zostały zapisane pomyślnie", NotificationType.SUCCESS);
                    return View("MyData");
                }
                this.AddNotification($"Błędne dane", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return View("MyData");
            }
            return View("MyData");
        }


        public ActionResult Avatar()
        {
            DBEntities db = new DBEntities();
            AvatarViewModel dane = new AvatarViewModel();
            var userId = User.Identity.GetUserId();

            try
            {
                if (ModelState.IsValid && userId != null)
                {
                    var avatarUrl = db.AspNetUsers.Where(m => m.Id == userId).Select(m => m.AvatarUrl).FirstOrDefault();
                    var avatarDefault = "https://res.cloudinary.com/czydobre-pl/image/upload/v1640786753/CzyDobre-www/awatar-domyslny-ramka_u9xv4t.png";
                    var linkCzydobre = "https://res.cloudinary.com/czydobre-pl/image/upload/v1640790462/CzyDobre-awatary/";

                    if (avatarUrl == null || avatarUrl == "")
                    {
                        dane.AvatarUrl = avatarDefault;
                    }
                    else
                    {
                        dane.AvatarUrl = linkCzydobre + avatarUrl;
                    }

                    if (dane != null)
                    {
                        return View(dane);
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
            }
            this.AddNotification($"Ups!, napotkaliśmy pewien problem. Proszę spróbować za kilka minut.", NotificationType.ERROR);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(AvatarViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account account = new Account(
                        ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                    Cloudinary cloudinary = new Cloudinary(account);

                    DBEntities db = new DBEntities();

                    var userId = User.Identity.GetUserId();
                    var awatarOldUrl = db.AspNetUsers.Where(m => m.Id == userId).Select(m => m.AvatarUrl).FirstOrDefault();
                    var aspNetUser = db.AspNetUsers.FirstOrDefault(m => m.Id == userId);
                    var awatarNewUrl = SaveAvatar(model);
                    aspNetUser.AvatarUrl = awatarNewUrl[0];

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    if (awatarOldUrl != null)
                    {
                        //usuwan rozszezenia .jpg 4 i .jpng 5
                        var ID4 = awatarOldUrl.Remove(awatarOldUrl.Length - 4);
                        var ID5 = awatarOldUrl.Remove(awatarOldUrl.Length - 5);
                        var nameId4 = "CzyDobre-awatary/" + ID4;
                        var nameId5 = "CzyDobre-awatary/" + ID5;

                        var deletionParams4 = new DeletionParams(ID4)
                        {
                            PublicId = nameId4
                        };
                        var deletionResult = cloudinary.Destroy(deletionParams4);

                        var deletionParams5 = new DeletionParams(ID5)
                        {
                            PublicId = nameId5
                        };
                        var deletionResult4 = cloudinary.Destroy(deletionParams5);
                    }

                    this.AddNotification($"Awatar, został zapisany pomyślnie", NotificationType.SUCCESS);
                    return RedirectToAction("Avatar");
                }
                this.AddNotification($"Plik nie spełnia wymagań", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("Avatar");
            }
            return RedirectToAction("Avatar");
        }

        private List<string> SaveAvatar(AvatarViewModel model)
        {
            bool OK = false;
            int allSize = 0;
            string ext = null;
            //nazwy plikow do zapisania do bazy
            List<string> imagesData = new List<string>();
            //zewryfikowane zdjecia do wyslania 
            List<HttpPostedFileBase> checkedFiles = new List<HttpPostedFileBase>();

            if (ModelState.IsValid)
            {
                try
                {
                    Account account = new Account(
                    ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                    ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                    ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                    Cloudinary cloudinary = new Cloudinary(account);

                    //Weryfikacja plików
                    foreach (HttpPostedFileBase item in model.Icon)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            ext = Path.GetExtension(item.FileName.ToLower());

                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg")
                            {
                                checkedFiles.Add(item);
                                var byteCount = item.ContentLength;

                                allSize = allSize + byteCount;
                                if (allSize < 5242880)
                                {
                                    OK = true;
                                }
                                else
                                {
                                    OK = false;
                                    this.AddNotification("Zdjęcie waży za dużo! Maksymalna wartość zdjęć wynosi 5MB", NotificationType.WARNING);
                                    return null;
                                }
                            }
                            else
                            {
                                this.AddNotification("Plik nieprawidłowy: " + item.FileName, NotificationType.INFO);
                            }
                        }
                        else
                        {
                            this.AddNotification("Nie wybrano pliku", NotificationType.INFO);
                        }
                    }

                    //Po weryfikacji
                    if (OK == true)
                    {
                        foreach (HttpPostedFileBase item in checkedFiles)
                        {
                            var filename = UniqueNumber() + item.FileName;
                            imagesData.Add(filename);

                            var uploadParams = new ImageUploadParams()
                            {
                                UseFilename = true,
                                UniqueFilename = false,
                                File = new FileDescription(filename, item.InputStream),
                                Folder = "CzyDobre-awatary"
                            };
                            var uploadResult = cloudinary.Upload(uploadParams);
                            //this.AddNotification(filename, NotificationType.SUCCESS);
                        }
                        //ModelState.Clear();
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }
                }
                catch (Exception)
                {
                    //ModelState.Clear();
                    //this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }

            return imagesData;
        }

        private String characters = "abcdeCDEfghijkzMABFHIJKLNOlmnopqrPQRSTstuvwxyUVWXYZ";

        private string UniqueNumber()
        {
            Random rnd = new Random();
            string s = "CzyDobre_";
            int unique;
            int n = 0;
            while (n < 10)
            {
                if (n % 2 == 0)
                {
                    s += rnd.Next(10).ToString();

                }
                else
                {
                    unique = rnd.Next(52);
                    if (unique < this.characters.Length)
                        s = String.Concat(s, this.characters[unique]);
                }
                n++;
            }
            var timeNuber = DateTimeOffset.Now.ToUnixTimeSeconds();
            return s + timeNuber.ToString() + "_";
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Zmieniono hasło."
                : message == ManageMessageId.SetPasswordSuccess ? "Ustawiono hasło."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Ustawiono dostawcę uwierzytelniania dwuetapowego."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : message == ManageMessageId.AddPhoneSuccess ? "Dodano numer telefonu."
                : message == ManageMessageId.RemovePhoneSuccess ? "Usunięto numer telefonu."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Wygeneruj i wyślij token
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Twój kod zabezpieczający: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Wyślij wiadomość SMS za pośrednictwem dostawcy usług SMS w celu zweryfikowania numeru telefonu
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Jeśli dotarliśmy tak daleko, oznacza to, że wystąpił błąd. Wyświetl ponownie formularz
            ModelState.AddModelError("", "Nie można zweryfikować numeru telefonu");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult DeleteAccount()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.Password != null)
                {
                    var userId = User.Identity.GetUserId();
                    var userEmail = User.Identity.GetUserName();
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    var checkPass = await UserManager.CheckPasswordAsync(user, model.Password);

                    if (checkPass == true)
                    {
                        DBEntities db = new DBEntities();
                        var id = db.AspNetUsers.Where(m => m.Id == userId).FirstOrDefault();
                        if (id != null)
                        {
                            db.AspNetUsers.Remove(id);
                            db.SaveChanges();
                            DeleteAccountEmail(userEmail);
                            LogOff();
                            this.AddNotification($"Twoje konto zostało usuniete", NotificationType.INFO);
                            return RedirectToAction("Index", "Home");
                        }
                        this.AddNotification($"Wystapił błąd, proszę spróbować za kilka minut", NotificationType.INFO);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this.AddNotification($"Nieprawidłowe hasło", NotificationType.ERROR);
                        return View("DeleteAccount");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public void DeleteAccountEmail(string DoEmail)
        {
            var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(wiadomosc);
            msg.To.Add(DoEmail);
            msg.Subject = "Twoje konto zostało usuniete - CzyDobre.pl";
            msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Informujemy, że twoje konto zostało usunięte." + "\n" + "Przykro nam, że się z nami roztajesz, będziemy czekać na Twój powrót." + "\n\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";

            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Jeśli dotarliśmy tak daleko, oznacza to, że wystąpił błąd. Wyświetl ponownie formularz
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Usunięto logowanie zewnętrzne."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Żądaj przekierowania do dostawcy logowania zewnętrznego w celu połączenia logowania bieżącego użytkownika
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Pomocnicy
        // Służy do ochrony XSRF podczas dodawania logowań zewnętrznych
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}