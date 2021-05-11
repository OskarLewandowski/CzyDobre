using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Pomysł oraz zamysł strony jest naszym projektem inżynierskim nad którym cały czas prężnie pracujemy.";  

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Dane kontaktowe biuro UAM Piła";

            return View();
        }

        public ActionResult Opinion()
        {
            ViewBag.Message = "Tu wszytkie opinie + filtrowanie";

            return View();
        }

        public ActionResult AddOpinion()
        {
            ViewBag.Message = "To dodawanie nowych opini";

            return View();
        }
    }
}