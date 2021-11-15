using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CzyDobre.Extensions;
using CzyDobre.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Role
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();

            return View(Roles);
        }

        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if(Role.Name != null)
            {
                context.Roles.Add(Role);
                context.SaveChanges();
                this.AddNotification("Rola \"" + Role.Name + "\" dodana!", NotificationType.SUCCESS);
                return RedirectToAction("Create", "Role");
            }
            else
            {
                this.AddNotification("Ups! Coś poszło nie tak", NotificationType.ERROR);
                return RedirectToAction("Create", "Role");
            }
            this.AddNotification("Ups! Coś poszło nie tak", NotificationType.ERROR);
            return RedirectToAction("Create", "Role");
        }
        //TODO usuniecie stworzonej roli Delete
    }
}