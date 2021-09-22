using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CzyDobre.Extensions;
using CzyDobre.Models;
using reCAPTCHA.MVC;

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
        [ValidateAntiForgeryToken]
        [CaptchaValidator(ErrorMessage = "Nieprawidłowe roziązanie pola Captcha", RequiredMessage = "Pole Captcha jest wymagane.")]
        public ActionResult Contact(ContactUsViewModels model, bool captchaValid)
        {
            if (ModelState.IsValid && captchaValid == true)
            {
                try
                {
                    var wiadomosc = ConfigurationManager.AppSettings["EmailContactUs"].ToString();

                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(wiadomosc);
                    msg.To.Add(new MailAddress(wiadomosc));
                    msg.Subject = model.Subject;
                    msg.Body = "Nazwa: " + model.Name + "\n" + "Email: " + model.Email + "\n" + "Wiadomość: " + model.Message;

                    if (model.Attachment != null)
                    {
                        string fileName = Path.GetFileName(model.Attachment.FileName);
                        msg.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
                    }

                    SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["EmailContactUs"].ToString(),
                        ConfigurationManager.AppSettings["PasswordContactUS"].ToString());
                    smtpClient.Credentials = credentials;
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(msg);

                    ModelState.Clear();
                    this.AddNotification("Wiadomość została wysłana, dziękujemy za kontakt.", NotificationType.SUCCESS);
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }
            return View();
        }
    }
}