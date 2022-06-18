using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        public ActionResult EditUser(AspNetUsers model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Account account = new Account(
                        ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                    Cloudinary cloudinary = new Cloudinary(account);
                    //AspNetUser aspNetUser = new AspNetUser();
                    var aspNetUser = db.AspNetUsers.FirstOrDefault(m => m.Id == model.Id);
                    var avatarUrlOld = aspNetUser.AvatarUrl;
                    //DateTime? date = null;
                    //if (model.LockoutEndDateUtc != null)
                    //{
                    //    DateTime boxDate = (DateTime)model.LockoutEndDateUtc;
                    //    string porawnaData = boxDate.ToString("MM.dd.yyyy HH:mm:ss");
                    //    date = Convert.ToDateTime(porawnaData);
                    //}

                    //aspNetUser.Id = model.Id;
                    aspNetUser.Email = model.Email;
                    aspNetUser.EmailConfirmed = model.EmailConfirmed;
                    //aspNetUser.PasswordHash = model.PasswordHash;
                    //aspNetUser.SecurityStamp = model.SecurityStamp;
                    //aspNetUser.PhoneNumber = model.PhoneNumber;
                    //aspNetUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    //aspNetUser.TwoFactorEnabled = model.TwoFactorEnabled;
                    //aspNetUser.LockoutEnabled = model.LockoutEnabled;
                    //aspNetUser.AccessFailedCount = model.AccessFailedCount;
                    aspNetUser.UserName = model.Email;
                    aspNetUser.FirstName = model.FirstName;
                    aspNetUser.LastName = model.LastName;
                    aspNetUser.NickName = model.NickName;
                    aspNetUser.AvatarUrl = model.AvatarUrl;
                    //aspNetUser.LockoutEndDateUtc = date;
                    //aspNetUser.LastBanDays = model.LastBanDays;
                    //aspNetUser.BanComment = model.BanComment;
                    //aspNetUser.WhoGaveBan = model.WhoGaveBan;

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    if (avatarUrlOld != null && model.AvatarUrl == null)
                    {
                        //usuwan rozszezenia .jpg 4 i .jpng 5
                        var ID4 = avatarUrlOld.Remove(avatarUrlOld.Length - 4);
                        var ID5 = avatarUrlOld.Remove(avatarUrlOld.Length - 5);
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
        public ActionResult Delete(string idUser, string nick, string email)
        {
            var id = db.AspNetUsers.Where(m => m.Id == idUser).FirstOrDefault();
            if (id != null)
            {
                db.AspNetUsers.Remove(id);
                db.SaveChanges();
                DeleteAccountEmail(email);
                this.AddNotification("Użytkownik \""+ nick + "\" został pomyślnie usunięty!", NotificationType.SUCCESS);
            }
            var userList = db.AspNetUsers.ToList();
            return View("UserList", userList);
        }

        [Route("edytowanie-uzytkownika")]
        [Route("User/Edit")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(AspNetUsers obj)
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
        public ActionResult BanUser(AspNetUsers model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //AspNetUser aspNetUser = new AspNetUser();
                    var aspNetUser = db.AspNetUsers.FirstOrDefault(m => m.Id == model.Id);


                    var adminName = User.Identity.Name;
                    double addDays = (double)model.LastBanDays;                

                    //aspNetUser.Id = model.Id;
                    //aspNetUser.Email = model.Email;
                    //aspNetUser.EmailConfirmed = model.EmailConfirmed;
                    //aspNetUser.PasswordHash = model.PasswordHash;
                    //aspNetUser.SecurityStamp = model.SecurityStamp;
                    //aspNetUser.PhoneNumber = model.PhoneNumber;
                    //aspNetUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    //aspNetUser.TwoFactorEnabled = model.TwoFactorEnabled;
                    //aspNetUser.LockoutEnabled = model.LockoutEnabled;
                    //aspNetUser.AccessFailedCount = model.AccessFailedCount;
                    //aspNetUser.UserName = model.Email;
                    //aspNetUser.FirstName = model.FirstName;
                    //aspNetUser.LastName = model.LastName;
                    //aspNetUser.NickName = model.NickName;
                    aspNetUser.LockoutEndDateUtc = DateTime.Now.AddDays(addDays);
                    aspNetUser.LastBanDays = model.LastBanDays;
                    aspNetUser.BanComment = model.BanComment;
                    aspNetUser.WhoGaveBan = adminName;

                    double date = ((addDays * 24 * 60)+60);
                    DateTime? updatedEndBanDate = DateTime.Now.AddMinutes(date);

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    SendBanEmail(model.Email, model.LastBanDays, model.BanComment, updatedEndBanDate);
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
        public ActionResult Ban(AspNetUsers obj)
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
        public ActionResult UnBanUser(string itemId, string itemEmail, bool itemEmailConfirmed, string itemPasswordHash,string itemSecurityStamp, string itemPhoneNumber, bool itemPhoneNumberConfirmed, bool itemTwoFactorEnabled, DateTime? itemLockoutEndDateUtc, bool itemLockoutEnabled, int itemAccessFailedCount, string itemUserName, string itemFirstName, string itemLastName, string itemNickName, int itemLastBanDays, string itemBanComment, string itemWhoGaveBan )
        {
            if (itemId != null)
            {
                //AspNetUser aspNetUser = new AspNetUser();
                var aspNetUser = db.AspNetUsers.FirstOrDefault(m => m.Id == itemId);

                //aspNetUser.Id = itemId;
                //aspNetUser.Email = itemEmail;
                //aspNetUser.EmailConfirmed = itemEmailConfirmed;
                //aspNetUser.PasswordHash = itemPasswordHash;
                //aspNetUser.SecurityStamp = itemSecurityStamp;
                //aspNetUser.PhoneNumber = itemPhoneNumber;
                //aspNetUser.PhoneNumberConfirmed = itemPhoneNumberConfirmed;
                //aspNetUser.TwoFactorEnabled = itemTwoFactorEnabled;
                //aspNetUser.LockoutEnabled = itemLockoutEnabled;
                //aspNetUser.AccessFailedCount = itemAccessFailedCount;
                //aspNetUser.UserName = itemUserName;
                //aspNetUser.FirstName = itemFirstName;
                //aspNetUser.LastName = itemLastName;
                //aspNetUser.NickName = itemNickName;
                aspNetUser.LockoutEndDateUtc = null;
                aspNetUser.LastBanDays = null;
                aspNetUser.BanComment = null;
                aspNetUser.WhoGaveBan = null;

                db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                SendUnBanEmail(itemEmail);
                this.AddNotification($"Użytkownik, \"" + itemNickName + "\" został odblokowany pomyślnie", NotificationType.SUCCESS);
            }
            var userList = db.AspNetUsers.ToList();
            return View("UserList", userList);
        }


        [Route("zmien-blokade-uzytkownika")]
        [Route("User/UnBanEditUser")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UnBanEditUser(AspNetUsers model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime? d1 = model.LockoutEndDateUtc;
                    DateTime d2 = DateTime.Now;
                    d2.AddDays(1);

                    if (model.LockoutEndDateUtc == null)
                    {
                        this.AddNotification($"Wymagane jest podanie daty w prawidłowym formacie DD/MM/RRRR GG:MM:SS", NotificationType.ERROR);
                        return View("UnBanEdit");
                    }
                    else if(d1 != null)
                    {
                        int result = DateTime.Compare((DateTime)d1, d2);

                        if (result < 0)
                        {
                            this.AddNotification($"Blokada musi trwać co najmniej jeden dzień!", NotificationType.ERROR);
                            return View("UnBanEdit");
                        }
                        else if (result == 0)
                        {
                            this.AddNotification($"Błędna data", NotificationType.WARNING);
                        }
                        else
                        {
                        }
                    }

                    //AspNetUser aspNetUser = new AspNetUser();
                    var aspNetUser = db.AspNetUsers.FirstOrDefault(m => m.Id == model.Id);

                    var adminName = User.Identity.Name;
                    DateTime? nowaData = null;

                    //aspNetUser.Id = model.Id;
                    //aspNetUser.Email = model.Email;
                    //aspNetUser.EmailConfirmed = model.EmailConfirmed;
                    //aspNetUser.PasswordHash = model.PasswordHash;
                    //aspNetUser.SecurityStamp = model.SecurityStamp;
                    //aspNetUser.PhoneNumber = model.PhoneNumber;
                    //aspNetUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    //aspNetUser.TwoFactorEnabled = model.TwoFactorEnabled;
                    //aspNetUser.LockoutEnabled = model.LockoutEnabled;
                    //aspNetUser.AccessFailedCount = model.AccessFailedCount;
                    //aspNetUser.UserName = model.Email;
                    //aspNetUser.FirstName = model.FirstName;
                    //aspNetUser.LastName = model.LastName;
                    //aspNetUser.NickName = model.NickName;
                    aspNetUser.LockoutEndDateUtc = model.LockoutEndDateUtc;
                    //aspNetUser.LastBanDays = model.LastBanDays;
                    aspNetUser.BanComment = model.BanComment;
                    aspNetUser.WhoGaveBan = adminName;

                    db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    if(model.LockoutEndDateUtc != null)
                    {
                        DateTime tempData = (DateTime)model.LockoutEndDateUtc;
                        nowaData = tempData.AddMinutes(60);                   
                    }

                    SendChangedBanEmail(model.Email, model.BanComment, nowaData);
                    this.AddNotification($"Blokada, została zmieniona pomyślnie", NotificationType.SUCCESS);
                    return View("UnBanEdit");
                }
                this.AddNotification($"Błąd!, Błędne dane. Prawidłowy format daty to: DD/MM/RRRR GG:MM:SS", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return View("UnBanEdit");
            }
            return View("UnBanEdit");
        }

        [Route("edytowanie-blokady-uzytkownika")]
        [Route("User/UnBanEdit")]
        [Authorize(Roles = "Admin")]
        public ActionResult UnBanEdit(AspNetUsers obj)
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

        [Route("odblokowanie-uzytkownika")]
        [Route("User/UnBan")]
        [Authorize(Roles = "Admin")]
        public ActionResult UnBan(AspNetUsers obj)
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
        public void SendBanEmail(string DoEmail, int? IleDnie, string PowodBlokady, DateTime? DoKiedy)
        {
            var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(wiadomosc);
            msg.To.Add(DoEmail);
            msg.Subject = "Twoje konto zostało zablokowane - CzyDobre.pl";
            if(IleDnie == 1)
            {
                msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Na twoje konto została nałożona blokada na: " + IleDnie + " dzień" + "\n" + "Powód blokady: " + PowodBlokady + "\n" +
                "Twoje konto zostanie odblokowane: " + DoKiedy + "\n\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";
            }
            else if(IleDnie == 99999)
            {
                msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Na twoje konto została nałożona permanentna blokada" + "\n" + "Powód blokady: " + PowodBlokady + "\n" +
                "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";
            }
            else
            {
                msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Na twoje konto została nałożona blokada na: " + IleDnie + " dni" + "\n" + "Powód blokady: " + PowodBlokady + "\n" +
                "Twoje konto zostanie odblokowane: " + DoKiedy + "\n\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";
            }
          
            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
        public void SendUnBanEmail(string DoEmail)
        {
            var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(wiadomosc);
            msg.To.Add(DoEmail);
            msg.Subject = "Twoje konto zostało odblokowane - CzyDobre.pl";
            msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Informujemy, że twoje konto zostało odblokowane." +"\n"+ "Zapraszamy ponownie na CzyDobre.pl" + "\n\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";

            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
        public void SendChangedBanEmail(string DoEmail, string PowodBlokady, DateTime? DoKiedy)
        {
            var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(wiadomosc);
            msg.To.Add(DoEmail);
            msg.Subject = "Blokada konta została zmieniona - CzyDobre.pl";
            msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Na twoim koncie została zmienniona blokada." + "\n" + "Twoje konto zostanie odblowane " + DoKiedy + "\n" + "Powód blokady: " + PowodBlokady + "\n" + "\n\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";
      
            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }

        public void DeleteAccountEmail(string DoEmail)
        {
            var wiadomosc = ConfigurationManager.AppSettings["EmailNoReply"].ToString();

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(wiadomosc);
            msg.To.Add(DoEmail);
            msg.Subject = "Twoje konto zostało usuniete - CzyDobre.pl";
            msg.Body = "Dzień dobry, " + DoEmail + "\n" + "Informujemy, że twoje konto zostało usunięte." + "\n\n" + "Pozdrawiamy zespół CzyDobre.pl" + "\n\n" + "W razie pytań prosimy o kontakt mejlowy: kontakt@czydobre.pl lub za pomocą formularza kontaktowego: https://www.czydobre.pl/kontakt";

            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailNoReply"].ToString(),
                ConfigurationManager.AppSettings["PasswordNoReply"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}