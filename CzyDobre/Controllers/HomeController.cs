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
                com.CommandText = "SELECT TOP 1000 dbo.AspNetProducts.ProductName, Id_Rating,RateService, RateTaste ,RateComposition ,RateIngredients ,RateTotal ,RateAdcompliance, img.ImageURL FROM dbo.AspNetRating JOIN dbo.AspNetProducts ON dbo.AspNetProducts.Id_Product = dbo.AspNetRating.Id_Product JOIN (SELECT [novum_czydobre.pl].[novum_Andzej].[AspNetImages].[Id_Product] , MAX(cast([novum_czydobre.pl].[novum_Andzej].[AspNetImages].[Url] as varchar(max))) AS ImageURL FROM [novum_czydobre.pl].[novum_Andzej].[AspNetImages] GROUP BY [Id_Product]) img ON img.Id_Product = dbo.AspNetRating.Id_Product";
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
                        RateAdcompliance = dr["RateAdcompliance"].ToString(),
                        ImageUrl = dr["ImageURL"].ToString()
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

            var top1 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").FirstOrDefault();
            var top2 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(1).FirstOrDefault();
            var top3 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(2).FirstOrDefault();
            var top4 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(3).FirstOrDefault();

            var top1image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top1.Id_Product + "')").FirstOrDefault();
            var top2image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top2.Id_Product + "')").FirstOrDefault();
            var top3image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top3.Id_Product + "')").FirstOrDefault();
            var top4image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top4.Id_Product + "')").FirstOrDefault();

            RattingViewModel rateImage = new RattingViewModel();

            string link = "https://res.cloudinary.com/czydobre-pl/image/upload/v1636896902/CzyDobre-images/";
            if (top1image != null)
            {
                string fullLink1 = link + top1image.Url;
                rateImage.Image1 = fullLink1;
            }
            if (top2image != null)
            {
                string fulLink2 = link + top2image.Url;
                rateImage.Image2 = fulLink2;
            }
            if (top3image != null)
            {
                string fulLink3 = link + top3image.Url;
                rateImage.Image3 = fulLink3;
            }
            if (top4image != null)
            {
                string fulLink4 = link + top4image.Url;
                rateImage.Image4 = fulLink4;
            }
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

            if (top1.Opinion_Counter > 0)
            {
                rate.ProductNameTop1 = top1.ProductName;
                rate.OpinionCounterTop1 = top1.Opinion_Counter;
                rate.TasteRateTop1 = top1.AvarageTaste / top1.Opinion_Counter;
                rate.ServiceRateTop1 = top1.AvarageService / top1.Opinion_Counter;
                rate.IngredientsRateTop1 = top1.AvarageIngredients / top1.Opinion_Counter;
            }
            if (top2.Opinion_Counter > 0)
            {
                rate.ProductNameTop2 = top2.ProductName;
                rate.OpinionCounterTop2 = top2.Opinion_Counter;
                rate.TasteRateTop2 = top2.AvarageTaste / top2.Opinion_Counter;
                rate.ServiceRateTop2 = top2.AvarageService / top2.Opinion_Counter;
                rate.IngredientsRateTop2 = top2.AvarageIngredients / top2.Opinion_Counter;
            }
            if (top3.Opinion_Counter > 0)
            {
                rate.ProductNameTop3 = top3.ProductName;
                rate.OpinionCounterTop3 = top3.Opinion_Counter;
                rate.TasteRateTop3 = top3.AvarageTaste / top3.Opinion_Counter;
                rate.ServiceRateTop3 = top3.AvarageService / top3.Opinion_Counter;
                rate.IngredientsRateTop3 = top3.AvarageIngredients / top3.Opinion_Counter;
            }
            if (top4.Opinion_Counter > 0)
            {
                rate.ProductNameTop4 = top4.ProductName;
                rate.OpinionCounterTop4 = top4.Opinion_Counter;
                rate.TasteRateTop4 = top4.AvarageTaste / top4.Opinion_Counter;
                rate.ServiceRateTop4 = top4.AvarageService / top4.Opinion_Counter;
                rate.IngredientsRateTop4 = top4.AvarageIngredients / top4.Opinion_Counter;
            }



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

            return View();
        }

        [Route("dodaj-produkt")]
        [Route("Home/AddProducts")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidator(ErrorMessage = "Nieprawidłowe rozwiązanie pola Captcha", RequiredMessage = "Pole Captcha jest wymagane.")]
        [Authorize]
        public ActionResult AddProducts(ProductFormModels prd,bool captchaValid)
        {
            if (ModelState.IsValid && captchaValid == true)
            {
                try
                {
                    DBEntities db = new DBEntities();

                    

                    List<string> zapisz = new List<string>();
                    
                    
                    zapisz = SaveIconToProduct(prd);
                    


                    AspNetProduct product = new AspNetProduct();
                    AspNetLocalization loc = new AspNetLocalization();
                    AspNetImage image = new AspNetImage();


                    var query = db.AspNetCategories.Where(s => s.CategoryName == prd.CategoryName).Select(s => s.Id_Category).First();


                    
                    loc.LocalizationCity = prd.LocName;
                    db.AspNetLocalizations.Add(loc);
                    db.SaveChanges();

                    
                    
                    var queryl = db.AspNetLocalizations.Where(s => s.LocalizationCity == prd.LocName).Select(s => s.Id_Localization).First();
                    product.Id_Localization = queryl;
                    product.ProductName = prd.ProductName;
                    product.ProductDescription = prd.ProductDescription;
                    product.Id_Category = query;
                    db.AspNetProducts.Add(product);
                    db.SaveChanges();


                    
                    var queryp = db.AspNetProducts.Where(s => s.ProductName == prd.ProductName).Select(s => s.Id_Product).First();
                    
                    //image.Url = zapiszIc;
                    //image.Id_Product = queryp;
                   // image.Icon = true;
                    //db.AspNetImages.Add(image);
                    //db.SaveChanges();


                      
                    foreach (var item in zapisz)
                    {
                        
                        image.Url = item;
                        image.Id_Product = queryp;
                        image.Icon = true; 
                        db.AspNetImages.Add(image);
                        db.SaveChanges();
                    }
                    




                    ModelState.Clear();
                    this.AddNotification("Produkt został dodany pomyślnie.", NotificationType.SUCCESS);

                }

                catch (Exception ex)
                {
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                    throw ex;
                }
            }
            /*else
            {
                this.AddNotification("Brak danych!", NotificationType.ERROR);
                return RedirectToAction("AddProducts");            
            }
            */
            return View();
        }
        private List<string> SaveIconToProduct(ProductFormModels model)
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
                    foreach (HttpPostedFileBase item in model.Icon)
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
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
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
                    foreach (HttpPostedFileBase item in model.Icon)
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
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }
                }
                catch (Exception)
                {
                    ModelState.Clear();
                    //this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }
           
            return imagesData;
        }
        /*
        public JsonResult GetMapMarker()
        {
            DBEntities db = new DBEntities();
            var ListOfAddress = db.AspNetLocalizations1.ToList();

            return Json(ListOfAddress, JsonRequestBehavior.AllowGet);
        }
        */
       
        //CzyDobre.pl/dodaj-opinie
        [Route("dodaj-opinie")]
        [Route("Home/AddOpinion")]
        [Authorize]
        public ActionResult AddOpinion()
        {
            DBEntities db = new DBEntities();
            

           

            return View();
        }
        public JsonResult AutoComplete(string prefix)
        {
            DBEntities db = new DBEntities();
            var  Products = (from AspNetProduct in db.AspNetProducts
                             where AspNetProduct.ProductName.StartsWith(prefix)
                             select new
                             {
                                 label = AspNetProduct.ProductName,
                                 val = AspNetProduct.Id_Product
                             }).ToList();

            return Json(Products);
        }
        public JsonResult AutoCompleteCategory(string prefix)
        {
            DBEntities db = new DBEntities();
            var Products = (from AspNetCategory in db.AspNetCategories
                            where AspNetCategory.CategoryName.StartsWith(prefix)
                            select new
                            {
                                label = AspNetCategory.CategoryName,
                                val = AspNetCategory.Id_Category
                            }).ToList();

            return Json(Products);
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

                    List<string> zapisz = new List<string>();

                    zapisz = SaveImagesToOpinion(opn);

                    //Console.WriteLine(zapisz);

                    //var user = db.AspNetProducts.Where(u => u.Id_Product == opn.Id_Product).FirstOrDefault();
                   
                   //user.Opinion_Counter += 1;

                    //user.AvarageTaste += opn.RateTaste;
                    //user.AvarageService += opn.RateService;
                   // user.AvarageIngredients += opn.RateIngredients;

                    AspNetRating rate = new AspNetRating();

                    
                    var query = db.AspNetProducts.Where(s => s.ProductName == opn.PName).Select(s => s.Id_Product).FirstOrDefault();
                    int q = query;
                    

                    if (query!=0)
                    {
                        rate.Id_Product = query;
                        rate.RateComposition = opn.RateComposition;
                        rate.RateIngredients = opn.RateIngredients;
                        rate.RateService = opn.RateService;
                        rate.RateTaste = opn.RateTaste;
                        rate.Comment = opn.Review;
                        rate.RateTotal = (rate.RateComposition + rate.RateIngredients + rate.RateService + rate.RateTaste) / 4;
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

                    }
                    else
                    {
                        this.AddNotification("Nie ma takiego produktu w naszej bazie ! Wprowadź ponownie nazwę produktu lub dodaj nowy!", NotificationType.ERROR);
                        return View();
                    }
                    

                  

                   ;
                    
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }
            /*
            else
            {
                this.AddNotification("Brak danych !", NotificationType.ERROR);
                return RedirectToAction("AddOpinion");

            }
            */
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
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
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