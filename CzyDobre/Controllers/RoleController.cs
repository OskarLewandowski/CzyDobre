﻿using System;
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
        ApplicationDbContext _context;

        public RoleController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Admin")]
        // GET: Role
        public ActionResult Index()
        {
            var Roles = _context.Roles.ToList();

            return View(Roles);
        }

        [Route("tworzenie-roli")]
        [Route("Role/Create")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [Route("tworzenie-roli")]
        [Route("Role/Create")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if(Role.Name != null)
            {
                _context.Roles.Add(Role);
                _context.SaveChanges();
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

        [Route("usuwanie-roli")]
        [Route("Role/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete()
        {
            return View();
        }

        [Route("usuwanie-roli")]
        [Route("Role/Delete")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(RoleViewModels model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = _context.Roles.Where(d => d.Name == model.RoleName).FirstOrDefault();
                    _context.Roles.Remove(role);
                    _context.SaveChanges();
                    this.AddNotification("Operacja wykonana pomyślnie!", NotificationType.SUCCESS);
                    return RedirectToAction("Delete", "Role");
                }

            }
            catch (Exception ex)
            {
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return RedirectToAction("Delete", "Role");
            }
            return View();
        }
    }
}