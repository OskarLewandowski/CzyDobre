using CzyDobre.Extensions;
using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        DBEntities db = new DBEntities();

        [Route("zarzadzanie-uzytkownikami")]
        [Route("User/Index")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("edytuj-uzytkownika")]
        [Route("User/EditUser")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUser(AspNetUser model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    AspNetUser aspNetUser = new AspNetUser();

                    aspNetUser.Id = model.Id;
                    aspNetUser.Email = model.Email;
                    aspNetUser.EmailConfirmed = model.EmailConfirmed;
                    aspNetUser.PasswordHash = model.PasswordHash;
                    aspNetUser.SecurityStamp = model.SecurityStamp;
                    aspNetUser.PhoneNumber = model.PhoneNumber;
                    aspNetUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    aspNetUser.TwoFactorEnabled = model.TwoFactorEnabled;
                    aspNetUser.LockoutEndDateUtc = model.LockoutEndDateUtc;
                    aspNetUser.LockoutEnabled = model.LockoutEnabled;
                    aspNetUser.AccessFailedCount = model.AccessFailedCount;
                    aspNetUser.UserName = model.Email;
                    aspNetUser.FirstName = model.FirstName;
                    aspNetUser.LastName = model.LastName;
                    aspNetUser.NickName = model.NickName;

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    this.AddNotification($"Użytkownik, edytowany pomyślnie", NotificationType.SUCCESS);
                    return View("Edit");
                }
                this.AddNotification($"Błąd!, Błędne dane", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return View("Edit");
            }
            return View("Edit");
        }

        [Route("lista-uzytkownikow")]
        [Route("User/UserList")]
        [Authorize(Roles = "Admin")]
        public ActionResult UserList()
        {
            var userList = db.AspNetUsers.ToList();
            return View(userList);
        }

        [Route("usuwanie-uzytkownika")]
        [Route("User/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string idUser, string nick)
        {
            var id = db.AspNetUsers.Where(m => m.Id == idUser).FirstOrDefault();
            if (id != null)
            {
                db.AspNetUsers.Remove(id);
                db.SaveChanges();

                this.AddNotification("Użytkownik \""+ nick + "\" został pomyślnie usunięty!", NotificationType.SUCCESS);
            }
            var userList = db.AspNetUsers.ToList();
            return View("UserList", userList);
        }

        [Route("edytowanie-uzytkownika")]
        [Route("User/Edit")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(AspNetUser obj)
        {
            if(obj !=null)
            {
                return View(obj);
            }
            else
            {
                return View();
            }
        }


        [Route("zablokuj-uzytkownika")]
        [Route("User/BanUser")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult BanUser(AspNetUser model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool ban = true;
                    AspNetUser aspNetUser = new AspNetUser();

                    var adminName = User.Identity.Name;
                    double addDays = (double)model.LastBanDays;                

                    aspNetUser.Id = model.Id;
                    aspNetUser.Email = model.Email;
                    aspNetUser.EmailConfirmed = model.EmailConfirmed;
                    aspNetUser.PasswordHash = model.PasswordHash;
                    aspNetUser.SecurityStamp = model.SecurityStamp;
                    aspNetUser.PhoneNumber = model.PhoneNumber;
                    aspNetUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    aspNetUser.TwoFactorEnabled = model.TwoFactorEnabled;
                    aspNetUser.LockoutEnabled = model.LockoutEnabled;
                    aspNetUser.AccessFailedCount = model.AccessFailedCount;
                    aspNetUser.UserName = model.Email;
                    aspNetUser.FirstName = model.FirstName;
                    aspNetUser.LastName = model.LastName;
                    aspNetUser.NickName = model.NickName;
                    aspNetUser.LockoutEndDateUtc = DateTime.Now.AddDays(addDays);
                    aspNetUser.LastBanDays = model.LastBanDays;
                    aspNetUser.BanComment = model.BanComment;
                    aspNetUser.WhoGaveBan = adminName;

                    double date = ((addDays * 24 * 60)+60);
                    DateTime? updatedEndBanDate = DateTime.Now.AddMinutes(date);

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SendEmail(model.Email, model.LastBanDays, model.BanComment, updatedEndBanDate, ban);
                    this.AddNotification($"Użytkownik, zablokowany pomyślnie", NotificationType.SUCCESS);
                    return View("Ban");
                }
                this.AddNotification($"Błąd!, Błędne dane", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return View("Ban");
            }
            return View("Ban");
        }

        [Route("blokowanie-uzytkownika")]
        [Route("User/Ban")]
        [Authorize(Roles = "Admin")]
        public ActionResult Ban(AspNetUser obj)
        {
            if (obj != null)
            {
                return View(obj);
            }
            else
            {
                return View();
            }
        }

        [Route("odblokuj-uzytkownika")]
        [Route("User/UnBanUser")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UnBanUser(AspNetUser model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool unban = false;
                    int result = 0;
                    AspNetUser aspNetUser = new AspNetUser();
                    
                    aspNetUser.Id = model.Id;
                    aspNetUser.Email = model.Email;
                    aspNetUser.EmailConfirmed = model.EmailConfirmed;
                    aspNetUser.PasswordHash = model.PasswordHash;
                    aspNetUser.SecurityStamp = model.SecurityStamp;
                    aspNetUser.PhoneNumber = model.PhoneNumber;
                    aspNetUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    aspNetUser.TwoFactorEnabled = model.TwoFactorEnabled;
                    aspNetUser.LockoutEnabled = model.LockoutEnabled;
                    aspNetUser.AccessFailedCount = model.AccessFailedCount;
                    aspNetUser.UserName = model.Email;
                    aspNetUser.FirstName = model.FirstName;
                    aspNetUser.LastName = model.LastName;
                    aspNetUser.NickName = model.NickName;
                    aspNetUser.LockoutEndDateUtc = model.LockoutEndDateUtc;
                    aspNetUser.LastBanDays = aspNetUser.LastBanDays;
                    aspNetUser.BanComment = aspNetUser.BanComment;
                    aspNetUser.WhoGaveBan = aspNetUser.WhoGaveBan;
                    
                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    SendEmail(model.Email, model.LastBanDays, model.BanComment, model.LockoutEndDateUtc, unban);

                    this.AddNotification($"Użytkownik, odblokowany pomyślnie", NotificationType.SUCCESS);
                    return View("UnBan");
                }
                this.AddNotification($"Błąd!, Błędne dane", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return View("UnBan");
            }
            return View("UnBan");
        }

        [Route("odblokowanie-uzytkownika")]
        [Route("User/UnBan")]
        [Authorize(Roles = "Admin")]
        public ActionResult UnBan(AspNetUser obj)
        {
            if (obj != null)
            {
                return View(obj);
            }
            else
            {
                return View();
            }
        }

        public void SendEmail(string DoEmail, int? IleDnie, string PowodBlokady, DateTime? DoKiedy, bool Ban)
        {
            if(Ban == true)
            {
                var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();
 
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(wiadomosc);
                msg.To.Add(DoEmail);
                msg.Subject = "Twoje konto zostało zablokowane - CzyDobre.pl";
                msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Na twoje konto została nałożona blokada na: " + IleDnie + " dni" + "\n" + "Za: " + PowodBlokady + "\n" + 
                    "Twoje konto zostanie odblowane " + DoKiedy + "\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n" + "kontakt@czydobre.pl";

                SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                    ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                    ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
            }
            else if(Ban == false)
            {
                var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(wiadomosc);
                msg.To.Add(DoEmail);
                msg.Subject = "Twoje konto zostało odblokowane - CzyDobre.pl";
                msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Twoje konto zostało odblokowane."+ "\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n" + "kontakt@czydobre.pl";

                SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                    ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                    ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);

                //if (aspNetUser.LockoutEndDateUtc != null)
                //{
                //    DateTime dateCheck = (DateTime)aspNetUser.LockoutEndDateUtc;
                //    result = DateTime.Compare(DateTime.Now, dateCheck);
                //}
            }
        }

    }
}