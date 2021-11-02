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
using System.Data.SqlClient;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CzyDobre.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        List<OpinionViewModels> opinionViewModels = new List<OpinionViewModels>();

        private void DisplayDataOpinion()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            con.ConnectionString = mySetting.ConnectionString;

            if (opinionViewModels.Count > 0)
            {
                opinionViewModels.Clear();
            }

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP 1000 dbo.AspNetProducts.ProductName ,RateService, RateTaste ,RateComposition ,RateIngredients ,RateAdcompliance FROM dbo.AspNetRating JOIN dbo.AspNetProducts ON dbo.AspNetProducts.Id_Product = dbo.AspNetRating.Id_Product";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    opinionViewModels.Add(new OpinionViewModels()
                    {
                        ProductName = dr["ProductName"].ToString(),
                        RateService = dr["RateService"].ToString(),
                        RateTaste = dr["RateTaste"].ToString(),
                        RateComposition = dr["RateComposition"].ToString(),
                        RateIngredients = dr["RateIngredients"].ToString(),
                        RateAdcompliance = dr["RateAdcompliance"].ToString()
                    });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //CzyDobre.pl/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //CzyDobre.pl/o-nas
        [Route("o-nas")]
        [Route("Home/About")]
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        

        //CzyDobre.pl/opinie
        [Route("opinie")]
        [Route("Home/Opinion")]
        [AllowAnonymous]
        public ActionResult Opinion()
        {
            DisplayDataOpinion();
            return View(opinionViewModels);
        }

        //CzyDobre.pl/wyniki
        [Route("wyniki")]
        [Route("Home/Results")]
        [AllowAnonymous]
        public ActionResult Results()
        {
            this.AddNotification("Funkcja wyszukiwania jest niedostępna", NotificationType.ERROR);
            return View();
        }

        //CzyDobre.pl/faq
        [Route("faq")]
        [Route("Home/FAQ")]
        [AllowAnonymous]
        public ActionResult FAQ()
        {
            return View();
        }

        //CzyDobre.pl/regulamin
        [Route("regulamin")]
        [Route("Home/Policies")]
        [AllowAnonymous]
        public ActionResult Policies()
        {
            return View();
        }

        //CzyDobre.pl/RODO
        [Route("RODO")]
        [Route("Home/RODO")]
        [AllowAnonymous]
        public ActionResult RODO()
        {
            return View();
        }

        [Route("privacy")]
        [Route("Home/Privacy")]
        [AllowAnonymous]
        public ActionResult Privacy()
        {
            return View();
        }

        //CzyDobre.pl/kontakt
        [Route("kontakt")]
        [Route("Home/Contact")]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        //CzyDobre.pl/kontakt
        [Route("kontakt")]
        [Route("Home/Contact")]
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
                    bool OK = false;
                    int allSize = 0;

                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(wiadomosc);
                    msg.To.Add(new MailAddress(wiadomosc));
                    msg.Subject = model.Subject;
                    msg.Body = "Nazwa: " + model.Name + "\n" + "Email: " + model.Email + "\n" + "Wiadomość: " + model.Message;


                    //Sprawdzenie rozmiaru zalacznika
                    foreach (HttpPostedFileBase item in model.Attachment)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            var byteCount = item.ContentLength;

                            allSize = allSize + byteCount;
                            if (allSize < 5242880)
                            {
                                OK = true;
                            }
                            else
                            {
                                this.AddNotification("Załącznik jest za duży! Maksymalna wartość załącznika wynosi 5MB", NotificationType.WARNING);
                                return View();
                            }
                        }
                    }

                    //zalaczniki po sprawdzeniu
                    if (OK == true)
                    {
                        foreach (HttpPostedFileBase attachment in model.Attachment)
                        {
                            if (attachment != null && attachment.ContentLength > 0)
                            {
                                string fileName = Path.GetFileName(attachment.FileName);
                                msg.Attachments.Add(new Attachment(attachment.InputStream, fileName));
                            }
                        }
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

        //CzyDobre.pl/dodaj-produkt
        [Route("dodaj-produkt")]
        [Route("Home/AddProducts")]
        [AllowAnonymous]
        
        public ActionResult AddProducts()
        {
            DBEntities db = new DBEntities();
            List<AspNetCategory> cats = db.AspNetCategories.ToList();
            ViewBag.CategoryList = new SelectList(cats, "Id_Category", "CategoryName");

            List<AspNetIngredient> ing = db.AspNetIngredients.ToList();
            ViewBag.IngredientsList = new SelectList(ing, "Id_Ingredients", "IngredientsName");

            List<AspNetLocalization> loc = db.AspNetLocalizations.ToList();
            ViewBag.LocalizationsList = new SelectList(loc, "Id_Localization", "LocalizationCity");



            return View();
        }

        [Route("dodaj-produkt")]
        [Route("Home/AddProducts")]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidator(ErrorMessage = "Nieprawidłowe roziązanie pola Captcha", RequiredMessage = "Pole Captcha jest wymagane.")]
        public ActionResult AddProducts(ProductFormModels prd,string Ostre)
        {
            try
            {
                DBEntities db = new DBEntities();

                List<AspNetCategory> cats = db.AspNetCategories.ToList();
                ViewBag.CategoryList = new SelectList(cats, "Id_Category", "CategoryName");

                List<AspNetIngredient> ing = db.AspNetIngredients.ToList();
                ViewBag.IngredientsList = new SelectList(ing, "Id_Ingredients", "IngredientsName");

                List<AspNetLocalization> loc = db.AspNetLocalizations.ToList();
                ViewBag.LocalizationsList = new SelectList(loc, "Id_Localization", "LocalizationCity");

                AspNetProduct product = new AspNetProduct();
                product.ProductName = prd.ProductName;
                product.ProductDescription = prd.ProductDescription;
                product.Id_Category = prd.Id_Category;
                product.Id_Localization = prd.Id_Localization;
                product.Id_Ingredients = prd.Id_Ingredients;
                


                db.AspNetProducts.Add(product);

                db.SaveChanges();
                int latestEmpId = product.Id_Product;
                return RedirectToAction("Index");

            }

            catch (Exception ex)
            {
                throw ex;

            }


            return View();
        }



        //CzyDobre.pl/dodaj-opinie
        [Route("dodaj-opinie")]
        [Route("Home/AddOpinion")]
        [Authorize]
        public ActionResult AddOpinion()
        {
            return View();
        }

        //CzyDobre.pl/dodaj-opinie
        [Route("dodaj-opinie")]
        [Route("Home/AddOpinion")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AddOpinion(AddOpinionViewModels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account account = new Account(
                        ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());

                    Cloudinary cloudinary = new Cloudinary(account);

                    bool OK = false;
                    int allSize = 0;
                    string ext = "null";
                    //nazwy plikow do zapisania w bazie
                    List<string> photoPathToDataBase = new List<string>();

                    //Sprawdzenie rozmiaru oraz typ zalacznika
                    foreach (HttpPostedFileBase item in model.Photo)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            ext = Path.GetExtension(item.FileName.ToLower());
                        }
                        else
                        {
                            ext = "null";
                        }

                        if (item != null && item.ContentLength > 0 && ext == ".png" || ext ==".jpeg" || ext == ".jpg")
                        {
                            var byteCount = item.ContentLength;

                            allSize = allSize + byteCount;
                            if (allSize < 5242880)
                            {
                                OK = true;
                            }
                            else
                            {
                                this.AddNotification("Zdjęcia ważą za dużo! Maksymalna wartość zdjęć wynosi 5MB", NotificationType.WARNING);
                                return View();
                            }
                        }
                        else
                        {
                            if (OK == true)
                            {
                                this.AddNotification("Plik nie został wysłany: " + item.FileName, NotificationType.INFO);

                            }
                            else
                            {
                                this.AddNotification("Nie wybrano pliku", NotificationType.INFO);

                            }
                        }
                    }

                    //zalaczniki po sprawdzeniu
                    if (OK == true)
                    {
                        foreach (HttpPostedFileBase file in model.Photo)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                var filename = UniqueNumber() + file.FileName;
                                photoPathToDataBase.Add(filename);

                                var uploadParams = new ImageUploadParams()
                                {
                                    UseFilename = true,
                                    UniqueFilename = false,
                                    File = new FileDescription(filename, file.InputStream),
                                    Folder = "CzyDobre-images"
                                };
                                var uploadResult = cloudinary.Upload(uploadParams);
                                //this.AddNotification(filename, NotificationType.SUCCESS);
                            }
                        }
                        ModelState.Clear();
                        this.AddNotification("Opinia została przesłana.", NotificationType.SUCCESS);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }
            return View();
        }

        private String characters = "abcdeCDEfghijkzMABFHIJKLNOlmnopqrPQRSTstuvwxyUVWXYZ";

        private string UniqueNumber()
        {
            Random rnd = new Random();
            string s = "CzyDobre_";
            int unique;
            int n = 0;
            while (n < 10)
            {
                if (n % 2 == 0)
                {
                    s += rnd.Next(10).ToString();

                }
                else
                {
                    unique = rnd.Next(52);
                    if (unique < this.characters.Length)
                        s = String.Concat(s, this.characters[unique]);
                }
                n++;
            }
            var timeNuber = DateTimeOffset.Now.ToUnixTimeSeconds() - rnd.Next(1, 100);
            return s + timeNuber.ToString() + "_";
        }
    }
}