using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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

        //CzyDobre.pl/kontakt
        [Route("kontakt")]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        //CzyDobre.pl/kontakt
        [Route("kontakt")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Contact(ContactUsViewModels m)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var wiadomosc = ConfigurationManager.AppSettings["EmailContactUs"].ToString();

                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(wiadomosc);
                    msg.To.Add(new MailAddress(wiadomosc));
                    msg.Subject = m.Subject;
                    msg.Body = "Nazwa: " + m.Name + "\n" + "Email: " + m.Email + "\n" + "Wiadomość: " + m.Message;
    
                    SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["EmailContactUs"].ToString(),
                        ConfigurationManager.AppSettings["PasswordContactUS"].ToString());
                    smtpClient.Credentials = credentials;
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(msg);

                    ModelState.Clear();
                    ViewBag.Message = "Dziękujemy za kontakt!";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Wystąpił błąd! {ex.Message}";
                }
            }
            return View();
        }
    }
}