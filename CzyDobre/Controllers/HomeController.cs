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

        [Route("o-nas")]
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Pomysł oraz zamysł strony jest naszym projektem inżynierskim nad którym cały czas prężnie pracujemy.";
            return View();
        }

        [Route("kontakt")]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("opinie")]
        [AllowAnonymous]
        public ActionResult Opinion()
        {
            this.AddNotification("Funkcja wyświelania opinii jest zablokowana", NotificationType.ERROR);
            return View();
        }

        [Route("dodaj-opinie")]
        [Authorize]
        public ActionResult AddOpinion()
        {
            this.AddNotification("Funkcja dodawania opinii jest zablokowana", NotificationType.ERROR);
            return View();
        }

        [Route("wyniki")]
        [AllowAnonymous]
        public ActionResult Results()
        {
            ViewBag.Message = "Wyniki wyszukiwania";
            return View();
        }

        
        [AllowAnonymous]
        public ActionResult FAQ()
        {
            ViewBag.Message = "Często zadawane pytania";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Policies()
        {
            ViewBag.Message = "Regulamin";
            return View();
        }
    }
}