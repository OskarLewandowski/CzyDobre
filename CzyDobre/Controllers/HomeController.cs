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
            this.AddNotification("Funkcja wyświelania opinii jest zablokowana", NotificationType.ERROR);
            return View();
        }

        [Authorize]
        public ActionResult AddOpinion()
        {
            this.AddNotification("Funkcja dodawania opinii jest zablokowana", NotificationType.ERROR);
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