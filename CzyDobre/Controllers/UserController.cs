using CzyDobre.Extensions;
using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}