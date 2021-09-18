using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CzyDobre.Extensions;
namespace CzyDobre.Controllers
{
    [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    public class HomeController : Controller
    {
        //CzyDobre.pl/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //CzyDobre.pl/o-nas
        [Route("o-nas")]
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        //CzyDobre.pl/kontakt
        [Route("kontakt")]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        //CzyDobre.pl/opinie
        [Route("opinie")]
        [AllowAnonymous]
        public ActionResult Opinion()
        {
            this.AddNotification("Funkcja wyświelania opinii jest niedostępna", NotificationType.ERROR);
            return View();
        }

        //CzyDobre.pl/dodaj-opinie
        [Route("dodaj-opinie")]
        [Authorize]
        public ActionResult AddOpinion()
        {
            this.AddNotification("Funkcja dodawania opinii jest niedostępna", NotificationType.ERROR);
            return View();
        }

        //CzyDobre.pl/wyniki
        [Route("wyniki")]
        [AllowAnonymous]
        public ActionResult Results()
        {
            return View();
        }

        //CzyDobre.pl/faq
        [Route("faq")]
        [AllowAnonymous]
        public ActionResult FAQ()
        {
            return View();
        }

        //CzyDobre.pl/regulamin
        [Route("regulamin")]
        [AllowAnonymous]
        public ActionResult Policies()
        {
            return View();
        }
    }
}