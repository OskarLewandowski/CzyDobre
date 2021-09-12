using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using CzyDobre.Extensions;
using CzyDobre.Models;
using MailMessage = System.Net.Mail.MailMessage;

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

        public ActionResult SuccessMessage()
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
    }
}