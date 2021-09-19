using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CzyDobre.Extensions;
using CzyDobre.Models;




namespace CzyDobre.Controllers
{

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
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [Route("kontakt")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Contact(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("kontakt@czydobre.pl");//Where mail will be sent 
                    msz.Subject = vm.Subject;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.webio.pl";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("kontakt@czydobre.pl", "Esl-123");

                    
                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }

            return View();
        }


        // public ActionResult Contact()
        // {
        //     return View();
        // }

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