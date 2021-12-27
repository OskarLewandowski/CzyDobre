using CzyDobre.Extensions;
using CzyDobre.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleV2Controller : Controller
    {
        DBEntities db = new DBEntities();

        ApplicationDbContext _context;

        public RoleV2Controller()
        {
            _context = new ApplicationDbContext();
        }

        [Route("lista-roli")]
        [Route("RoleV2/RoleList")]
        [Authorize(Roles = "Admin")]
        public ActionResult RoleList()
        {
            var userList = db.AspNetUsers.ToList();
            return View(userList);
        }

        [Route("tworzenie-nowej-roli")]
        [Route("RoleV2/Create")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [Route("tworzenie-nowej-roli")]
        [Route("RoleV2/Create")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            var nameRole = Role.Name;
            bool uniq = false;
            var wynik = db.AspNetRoles.Where(u => u.Name == nameRole).Select(u => u.Name).FirstOrDefault();

            if (wynik == "" || wynik == null)
            {
                uniq = true;
            }
            else
            {
                uniq = false;
                this.AddNotification("Rola już istnieje!", NotificationType.ERROR);
                return RedirectToAction("Create", "RoleV2");
            }

            if (nameRole != null && uniq == true)
            {
                _context.Roles.Add(Role);
                _context.SaveChanges();
                this.AddNotification("Rola \"" + nameRole + "\" została pomyślnie dodana!", NotificationType.SUCCESS);
                return RedirectToAction("Create", "RoleV2");
            }
            else if(nameRole == null)
            {
                this.AddNotification("Nazwa roli jest wymagana, pole nie może być puste!", NotificationType.ERROR);
                return RedirectToAction("Create", "RoleV2");
            }
            else
            {
                this.AddNotification("Ups! Coś poszło nie tak", NotificationType.ERROR);
                return RedirectToAction("Create", "RoleV2");
            }
        }

        [Route("usuwanie-stworzonej-roli")]
        [Route("RoleV2/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete()
        {
            return View();
        }

        [Route("usuwanie-stworzonej-roli")]
        [Route("RoleV2/Delete")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(RoleViewModels model)
        {
            try
            {   
                if(model.RoleName == null)
                {
                    this.AddNotification("Nazwa roli jest wymagana, pole nie może być puste!", NotificationType.ERROR);
                    return RedirectToAction("Delete", "RoleV2");
                }

                var nameRole = model.RoleName;
                bool uniq = false;
                var wynik = db.AspNetRoles.Where(u => u.Name == nameRole).Select(u => u.Name).FirstOrDefault();

                if (wynik == "" || wynik == null)
                {
                    uniq = false;
                    this.AddNotification("Rola nie istnieje!", NotificationType.ERROR);
                    return RedirectToAction("Delete", "RoleV2");            
                }
                else
                {
                    uniq = true;
                }

                if (ModelState.IsValid && model.RoleName != null && uniq == true)
                {
                    var role = _context.Roles.Where(d => d.Name == model.RoleName).FirstOrDefault();
                    _context.Roles.Remove(role);
                    _context.SaveChanges();
                    this.AddNotification("Rola \"" + nameRole + "\" została pomyślnie usunięta!", NotificationType.SUCCESS);
                    return RedirectToAction("Delete", "RoleV2");
                }
            }
            catch (Exception ex)
            {
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("Delete", "RoleV2");
            }
            return View();
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Route("przypisanie-roli-uzytkownikowi")]
        [Route("RoleV2/AssignRole")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AssignRole(string idUser, string emailUser)
        {
            ViewBag.Name = new SelectList(_context.Roles.ToList(), "Name", "Name");
            ViewBag.UserName = emailUser;
 
            return View();
        }

        [Route("przypisanie-roli-uzytkownikowi")]
        [Route("RoleV2/AssignRole")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRole(RegisterViewModel model, ApplicationUser user)
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
                this.AddNotification("Rola \"" + model.Name + "\" została pomyślnie przypisana użytkownikowi \"" + user.UserName + "\"", NotificationType.SUCCESS);
                return RedirectToAction("RoleList", "RoleV2");
            }
            catch (Exception ex)
            {
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("RoleList", "RoleV2");
            }
        }

    }
}