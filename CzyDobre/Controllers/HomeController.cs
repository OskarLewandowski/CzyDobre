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
using Newtonsoft.Json;

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
                com.CommandText = "SELECT TOP 1000 dbo.AspNetProducts.ProductName ,RateService, RateTaste ,RateComposition ,RateIngredients ,RateTotal ,RateAdcompliance FROM dbo.AspNetRating JOIN dbo.AspNetProducts ON dbo.AspNetProducts.Id_Product = dbo.AspNetRating.Id_Product";
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
                        RateTotal = dr["RateTotal"].ToString(),
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
            DBEntities db = new DBEntities();
            RattingViewModel rateImage = new RattingViewModel();
            var top1 = (from top1p in db.AspNetProducts
                        join top1i in db.AspNetImages
                        on top1p.Id_Product equals top1i.Id_Product
                        orderby top1p.Opinion_Counter descending
                        select new
                        {
                            Url1 = top1i.Url
                        }).FirstOrDefault();
            var top2 = (from top2p in db.AspNetProducts
                        join top2i in db.AspNetImages
                        on top2p.Id_Product equals top2i.Id_Product
                        orderby top2p.Opinion_Counter descending
                        select new
                        {
                            Url2 = top2i.Url
                        }).Skip(1).FirstOrDefault();
            var top3 = (from top3p in db.AspNetProducts
                        join top3i in db.AspNetImages
                        on top3p.Id_Product equals top3i.Id_Product
                        orderby top3p.Opinion_Counter descending
                        select new
                        {
                            Url3 = top3i.Url
                        }).Skip(2).FirstOrDefault();
            var top4 = (from top4p in db.AspNetProducts
                        join top4i in db.AspNetImages
                        on top4p.Id_Product equals top4i.Id_Product
                        orderby top4p.Opinion_Counter descending
                        select new
                        {
                            Url4 = top4i.Url
                        }).Skip(3).FirstOrDefault();
            string link = "https://res.cloudinary.com/czydobre-pl/image/upload/v1636896902/CzyDobre-images/";
            string fullLink1 = link + top1.Url1;
            string fulLink2 = link + top2.Url2;
            string fulLink3 = link + top3.Url3;
            string fulLink4 = link + top4.Url4;
            rateImage.Image1 = fullLink1;
            rateImage.Image2 = fulLink2;
            rateImage.Image3 = fulLink3;
            rateImage.Image4 = fulLink4;
            return View(rateImage);
            }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetRate()
        {
            DBEntities db = new DBEntities();
            RattingViewModel rate = new RattingViewModel();

            var top1 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").FirstOrDefault();
            var top2 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(1).FirstOrDefault();
            var top3 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(2).FirstOrDefault();
            var top4 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(3).FirstOrDefault();

            rate.ProductNameTop1 = top1.ProductName;
            rate.OpinionCounterTop1 = top1.Opinion_Counter;
            rate.TasteRateTop1 = top1.AvarageTaste;
            rate.ServiceRateTop1 = top1.AvarageService;
            rate.IngredientsRateTop1 = top1.AvarageIngredients;
            rate.ProductNameTop2 = top2.ProductName;
            rate.OpinionCounterTop2 = top2.Opinion_Counter;
            rate.TasteRateTop2 = top2.AvarageTaste;
            rate.ServiceRateTop2 = top2.AvarageService;
            rate.IngredientsRateTop2 = top2.AvarageIngredients;
            rate.ProductNameTop3 = top3.ProductName;
            rate.OpinionCounterTop3 = top3.Opinion_Counter;
            rate.TasteRateTop3 = top3.AvarageTaste;
            rate.ServiceRateTop3 = top3.AvarageService;
            rate.IngredientsRateTop3 = top3.AvarageIngredients;
            rate.ProductNameTop4 = top4.ProductName;
            rate.OpinionCounterTop4 = top4.Opinion_Counter;
            rate.TasteRateTop4 = top4.AvarageTaste;
            rate.ServiceRateTop4 = top4.AvarageService;
            rate.IngredientsRateTop4 = top4.AvarageIngredients;
 
      



            var json = JsonConvert.SerializeObject(rate);
            return Json(json, JsonRequestBehavior.AllowGet);
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
        [Authorize]

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidator(ErrorMessage = "Nieprawidłowe roziązanie pola Captcha", RequiredMessage = "Pole Captcha jest wymagane.")]
        [Authorize]
        public ActionResult AddProducts(ProductFormModels prd,bool captchaValid)
        {
            if (ModelState.IsValid && captchaValid == true)
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

                    List<string> zapisz = new List<string>();

                    zapisz = SaveImagesToProduct(prd);

                    //Console.WriteLine(zapisz);

                    AspNetProduct product = new AspNetProduct();
                    product.ProductName = prd.ProductName;
                    product.ProductDescription = prd.ProductDescription;
                    product.Id_Category = prd.Id_Category;
                    product.Id_Localization = prd.Id_Localization;                    

                    db.AspNetProducts.Add(product);

                    db.SaveChanges();
                    int latestId = product.Id_Product;
                    AspNetImage image = new AspNetImage();
                    foreach (var item in zapisz)
                    {
                        
                        image.Url = item;
                        image.Id_Product = product.Id_Product;
                        db.AspNetImages.Add(image);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                this.AddNotification("Brak danych!", NotificationType.ERROR);
                return RedirectToAction("AddProducts");            
            }
        }

        private List<string> SaveImagesToProduct(ProductFormModels model)
        {
            bool OK = false;
            int allSize = 0;
            string ext = null;
            //nazwy plikow do zapisania do bazy
            List<string> imagesData = new List<string>();
            //zewryfikowane zdjecia do wyslania 
            List<HttpPostedFileBase> checkedFiles = new List<HttpPostedFileBase>();

            if (ModelState.IsValid)
            {
                try
                {
                    Account account = new Account(
                        ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                    Cloudinary cloudinary = new Cloudinary(account);

                    //Weryfikacja plików
                    foreach (HttpPostedFileBase item in model.Image)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            ext = Path.GetExtension(item.FileName.ToLower());

                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg")
                            {
                                checkedFiles.Add(item);
                                var byteCount = item.ContentLength;

                                allSize = allSize + byteCount;
                                if (allSize < 5242880)
                                {
                                    OK = true;
                                }
                                else
                                {
                                    OK = false;
                                    this.AddNotification("Zdjęcia ważą za dużo! Maksymalna wartość zdjęć wynosi 5MB", NotificationType.WARNING);
                                    return null;
                                }
                            }
                            else
                            {
                                this.AddNotification("Plik nieprawidłowy: " + item.FileName, NotificationType.INFO);
                            }
                        }
                        else
                        {
                            this.AddNotification("Nie wybrano pliku", NotificationType.INFO);
                        }
                    }

                    //Po weryfikacji
                    if (OK == true)
                    {
                        foreach (HttpPostedFileBase item in checkedFiles)
                        {
                            var filename = UniqueNumber() + item.FileName;
                            imagesData.Add(filename);

                            var uploadParams = new ImageUploadParams()
                            {
                                UseFilename = true,
                                UniqueFilename = false,
                                File = new FileDescription(filename, item.InputStream),
                                Folder = "CzyDobre-images"
                            };
                            var uploadResult = cloudinary.Upload(uploadParams);
                            //this.AddNotification(filename, NotificationType.SUCCESS);
                        }
                        ModelState.Clear();
                        this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }
           
            return imagesData;
        }


        //CzyDobre.pl/dodaj-opinie
        [Route("dodaj-opinie")]
        [Route("Home/AddOpinion")]
        [Authorize]
        public ActionResult AddOpinion()
        {
            DBEntities db = new DBEntities();
            List<AspNetProduct> prod = db.AspNetProducts.ToList();
            ViewBag.ProductsList = new SelectList(db.AspNetProducts.ToList(), "Id_Product", "ProductName");

           

            return View();
        }

        //CzyDobre.pl/dodaj-opinie
        [Route("dodaj-opinie")]
        [Route("Home/AddOpinion")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AddOpinion(AddOpinionViewModels opn)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    DBEntities db = new DBEntities();

                    List<AspNetProduct> prod = db.AspNetProducts.ToList();
                    ViewBag.ProductsList = new SelectList(db.AspNetProducts.ToList(), "Id_Product", "ProductName");

                    


                    List<string> zapisz = new List<string>();

                    zapisz = SaveImagesToOpinion(opn);

                    //Console.WriteLine(zapisz);

                    AspNetRating rate = new AspNetRating();
                    rate.Id_Product = opn.Id_Product;
                    rate.RateComposition = opn.RateComposition;
                    rate.RateIngredients = opn.RateIngredients;
                    rate.RateService = opn.RateService;
                    rate.RateTaste= opn.RateTaste;
                    rate.Comment = opn.Review;
                    rate.RateTotal = (rate.RateComposition + rate.RateIngredients + rate.RateService + rate.RateTaste) / 4 ;

                    db.AspNetRatings.Add(rate);
                    db.SaveChanges();

                    
                    AspNetImage image = new AspNetImage();
                    foreach (var item in zapisz)
                    {
                        
                        image.Url = item;
                        image.Id_Product = rate.Id_Product;
                        db.AspNetImages.Add(image);
                        db.SaveChanges();
                    }
                    ModelState.Clear();
                    this.AddNotification("Opinia została wysłana, dziękujemy za opinię.", NotificationType.SUCCESS);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }
            else
            {
                this.AddNotification("Brak danych !", NotificationType.ERROR);
                return RedirectToAction("AddOpinion");

            }
            return View();
        }

        private List<string> SaveImagesToOpinion(AddOpinionViewModels model)
        {
            bool OK = false;
            int allSize = 0;
            string ext = null;
            //nazwy plikow do zapisania do bazy
            List<string> imagesData = new List<string>();
            //zewryfikowane zdjecia do wyslania 
            List<HttpPostedFileBase> checkedFiles = new List<HttpPostedFileBase>();

            if (ModelState.IsValid)
            {              
                try
                {
                    Account account = new Account(
                        ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                        ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                    Cloudinary cloudinary = new Cloudinary(account);
                    
                    //Weryfikacja plików
                    foreach (HttpPostedFileBase item in model.Photo)
                    {
                        if(item != null && item.ContentLength > 0)
                        {
                            ext = Path.GetExtension(item.FileName.ToLower());

                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg")
                            {
                                checkedFiles.Add(item);
                                var byteCount = item.ContentLength;

                                allSize = allSize + byteCount;
                                if (allSize < 5242880)
                                {
                                    OK = true;
                                }
                                else
                                {
                                    OK = false;
                                    this.AddNotification("Zdjęcia ważą za dużo! Maksymalna wartość zdjęć wynosi 5MB", NotificationType.WARNING);
                                    return null;
                                }
                            }
                            else
                            {
                                this.AddNotification("Plik nieprawidłowy: " + item.FileName, NotificationType.INFO);
                            }
                        }
                        else
                        {
                            this.AddNotification("Nie wybrano pliku", NotificationType.INFO);
                        }
                    }

                    //Po weryfikacji
                    if(OK == true)
                    {
                        foreach (HttpPostedFileBase item in checkedFiles)
                        {
                            var filename = UniqueNumber() + item.FileName;
                            imagesData.Add(filename);

                            var uploadParams = new ImageUploadParams()
                            {
                                UseFilename = true,
                                UniqueFilename = false,
                                File = new FileDescription(filename, item.InputStream),
                                Folder = "CzyDobre-images"
                            };
                            var uploadResult = cloudinary.Upload(uploadParams);
                            //this.AddNotification(filename, NotificationType.SUCCESS);
                        }
                        ModelState.Clear();
                        this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }                
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }        
            }
            return imagesData;
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
            var timeNuber = DateTimeOffset.Now.ToUnixTimeSeconds();
            return s + timeNuber.ToString() + "_";
        }
    }
}