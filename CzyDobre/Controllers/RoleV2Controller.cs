using CzyDobre.Extensions;
using CzyDobre.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
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
                this.AddNotification("Taka nazwa roli już istnieje!", NotificationType.ERROR);
                return RedirectToAction("Create", "RoleV2");
            }

            if (nameRole != null && uniq == true)
            {
                _context.Roles.Add(Role);
                _context.SaveChanges();
                this.AddNotification("Rola \"" + nameRole + "\" dodana!", NotificationType.SUCCESS);
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
                    this.AddNotification("Wprowadzona nazwa roli nie istnieje!", NotificationType.ERROR);
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
                    this.AddNotification("Operacja wykonana pomyślnie!", NotificationType.SUCCESS);
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

    }
}