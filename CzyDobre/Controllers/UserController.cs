using CzyDobre.Extensions;
using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


        [Route("dodaj-uzytkownika")]
        [Route("User/AddUser")]
        [Authorize(Roles = "Admin")]
        public ActionResult AddUser()
        {
            return View();
        }

        [Route("dodaj-uzytkownika")]
        [Route("User/AddUser")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddUser(AspNetUser model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    AspNetUser aspNetUser = new AspNetUser();

                    aspNetUser.Email = model.Email;
                    aspNetUser.UserName = model.Email;
                    aspNetUser.FirstName = model.FirstName;
                    aspNetUser.LastName = model.LastName;
                    aspNetUser.NickName = model.NickName;

                    db.AspNetUsers.Add(aspNetUser);
                    db.SaveChanges();
                    this.AddNotification($"Użytkownik, dodany pomyślnie", NotificationType.ERROR);
                    return RedirectToAction("AddUser", "User");
                }
                this.AddNotification($"Ups!, Proszę uzupełnic brakujące pola", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("AddUser", "User");
            }
            return View();
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
            //var id = _context.Users.Where(m => m.Id == idUser).FirstOrDefault();
            var id = db.AspNetUsers.Where(m => m.Id == idUser).FirstOrDefault();
            if (id != null)
            {
                //_context.Users.Remove(id);
                //_context.SaveChanges();
                db.AspNetUsers.Remove(id);
                db.SaveChanges();

                this.AddNotification("Użytkownik \""+ nick + "\" został pomyślnie usunięty!", NotificationType.SUCCESS);
            }
            var userList = db.AspNetUsers.ToList();
            return View("UserList", userList);
        }

    }
}