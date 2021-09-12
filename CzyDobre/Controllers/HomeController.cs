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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                var mail = new MailMessage();
                mail.To.Add(new MailAddress(model.SenderEmail));
                mail.Subject = "Your Email Subject";
                mail.Body = string.Format("<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>", model.SenderName, model.SenderEmail, model.Message);
                mail.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(mail);
                    return RedirectToAction("SuccessMessage");
                }
            }
            return View(model);
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