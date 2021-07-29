using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CzyDobre.Extensions;

namespace CzyDobre.Controllers
{
    
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        { 
            this.AddNotification("Strona jest w budowie, jeżeli masz pomysł na nową funkcję i chcesz się tym z nami podzielić napisz do nas na kontakt@czydobre.pl ", NotificationType.WARNING);
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Pomysł oraz zamysł strony jest naszym projektem inżynierskim nad którym cały czas prężnie pracujemy.";  

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {         
            return View();
        }

        [AllowAnonymous]
        public ActionResult Opinion()
        {
            ViewBag.Message = "Tu wszytkie opinie + filtrowanie";

            return View();
        }

        [Authorize]
        public ActionResult AddOpinion()
        {
            ViewBag.Message = "To dodawanie nowych opini";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Results()
        {
            ViewBag.Message = "Wyniki wyszukiwania";

            return View();
        }
    }
}