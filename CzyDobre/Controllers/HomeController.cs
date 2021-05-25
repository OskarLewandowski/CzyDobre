using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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