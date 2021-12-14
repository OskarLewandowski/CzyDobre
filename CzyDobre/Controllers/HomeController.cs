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
        //CzyDobre.pl/
        [AllowAnonymous]
        public ActionResult Index()
        {
            DBEntities db = new DBEntities();
            RattingViewModel rateImage = new RattingViewModel();
            string link = "https://res.cloudinary.com/czydobre-pl/image/upload/v1636896902/CzyDobre-images/";

            var top1 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").FirstOrDefault();
            var top2 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(1).FirstOrDefault();
            var top3 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(2).FirstOrDefault();
            var top4 = db.AspNetProducts.SqlQuery("select * from AspNetProducts order by Opinion_Counter DESC").Skip(3).FirstOrDefault();


            if (top1 != null && top1.CzyDobre ==true)
            {
                var top1image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top1.Id_Product + "')").FirstOrDefault();
                if (top1image != null)
                {
                    string fullLink1 = link + top1image.Url;
                    rateImage.Image1 = fullLink1;
                }
            }
            if (top2 != null && top2.CzyDobre == true)
            {
                var top2image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top2.Id_Product + "')").FirstOrDefault();
                if (top2image != null)
                {
                    string fulLink2 = link + top2image.Url;
                    rateImage.Image2 = fulLink2;
                }
            }
            if (top3 != null && top3.CzyDobre == true)
            {
                var top3image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top3.Id_Product + "')").FirstOrDefault();
                if (top3image != null)
                {
                    string fulLink3 = link + top3image.Url;
                    rateImage.Image3 = fulLink3;
                }
            }
            if (top4 != null && top4.CzyDobre == true)
            {
                var top4image = db.AspNetImages.SqlQuery("select * from AspNetImages where Id_Product IN ('" + top4.Id_Product + "')").FirstOrDefault();
                if (top4image != null)
                {
                    string fulLink4 = link + top4image.Url;
                    rateImage.Image4 = fulLink4;
                }
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

            if (top1.Opinion_Counter >= 0 && top1.CzyDobre == true)
            {
                rate.ProductNameTop1 = top1.ProductName;
                rate.OpinionCounterTop1 = top1.Opinion_Counter;
                rate.TasteRateTop1 = top1.AvarageTaste / top1.Opinion_Counter;
                rate.ServiceRateTop1 = top1.AvarageService / top1.Opinion_Counter;
                rate.IngredientsRateTop1 = top1.AvarageIngredients / top1.Opinion_Counter;
            }
            if (top2.Opinion_Counter >= 0 && top2.CzyDobre == true)
            {
                rate.ProductNameTop2 = top2.ProductName;
                rate.OpinionCounterTop2 = top2.Opinion_Counter;
                rate.TasteRateTop2 = top2.AvarageTaste / top2.Opinion_Counter;
                rate.ServiceRateTop2 = top2.AvarageService / top2.Opinion_Counter;
                rate.IngredientsRateTop2 = top2.AvarageIngredients / top2.Opinion_Counter;
            }
            if (top3.Opinion_Counter >= 0 && top3.CzyDobre == true)
            {
                rate.ProductNameTop3 = top3.ProductName;
                rate.OpinionCounterTop3 = top3.Opinion_Counter;
                rate.TasteRateTop3 = top3.AvarageTaste / top3.Opinion_Counter;
                rate.ServiceRateTop3 = top3.AvarageService / top3.Opinion_Counter;
                rate.IngredientsRateTop3 = top3.AvarageIngredients / top3.Opinion_Counter;
            }
            if (top4.Opinion_Counter >= 0 && top4.CzyDobre == true)
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
        public ActionResult Opinion(int id = 0, int filtr1 = 0, int filtr2 = 0, int filtr3 = 0, string query = "", int page = 0, int pageSize = 18)
        {
            if (page < 0)
            {
                page = 0;
            }
            List<OpinionViewModels> opinionViewModels = new List<OpinionViewModels>();
            try
            {
                // Tworzenie łącza do bazy Entity Framework
                DBEntities db = new DBEntities();

                // Deklaracja listy typu IQueryable do przechowywania rekordów; Dołączenie do niej Tabeli AspNetRatings i AspNetProducts.
                IQueryable<AspNetRating> SQLresult = db.AspNetRatings;
                SQLresult.Join(db.AspNetProducts,
                    a => a.Id_Product,
                    b => b.Id_Product,
                    (a, b) => a.Id_Product);

                // Wyszukiwanie.
                if (!String.IsNullOrEmpty(query))
                {
                    SQLresult = SQLresult.Where(c => c.AspNetProduct.AspNetCategory.CategoryName.Contains(query));
                }
                // Aplikowanie filtrów.
                if (filtr1 != 0)
                {
                    SQLresult = SQLresult.Where(c => c.RateTaste >= filtr1 + 1);
                }
                if (filtr2 != 0)
                {
                    SQLresult = SQLresult.Where(c => c.RateService >= filtr2 + 1);
                }
                if (filtr3 != 0)
                {
                    SQLresult = SQLresult.Where(c => c.RateIngredients >= filtr3 + 1);
                }

                // Ustawianie sortowania.
                switch (id)
                {
                    case 1:
                        SQLresult = SQLresult.OrderBy(x => x.AspNetProduct.ProductName);
                        break;
                    case 2:
                        SQLresult = SQLresult.OrderByDescending(x => x.AspNetProduct.ProductName);
                        break;
                    case 3:
                        SQLresult = SQLresult.OrderByDescending(x => x.AspNetProduct.Opinion_Counter);
                        break;
                    case 4:
                        SQLresult = SQLresult.OrderByDescending(x => x.Id_Rating);
                        break;
                    default:
                        SQLresult = SQLresult.OrderByDescending(x => x.Id_Rating);
                        break;
                }

                // Wybieranie danych parametrów.
                SQLresult.Select(a => new {
                    a.AspNetProduct.ProductName,
                    a.Id_Rating,
                    a.RateService,
                    a.RateTaste,
                    a.RateIngredients
                });

                // Ustawienia zmiennych ViewBag dla stronicowania.
                // Ilość Stron (Liczone od 0).
                ViewBag.NumberOfPages = 0;
                ViewBag.NumberOfPages = (SQLresult.Count() - 1) / pageSize + 1;
                // Aktualna strona.
                ViewBag.ActivePage = page;
                // Aktualne filtry.
                ViewBag.Filter1 = filtr1;
                ViewBag.Filter2 = filtr2;
                ViewBag.Filter3 = filtr3;
                // Aktualne sortowanie.
                ViewBag.SortBy = id;

                // Wybieranie pageSize rekordów, omijając rekordy poprzednich stron.
                SQLresult = SQLresult.Skip(page * pageSize).Take(pageSize);

                //Uzupełnianie listy modeli danymi z zapytania.
                foreach (var row in SQLresult)
                {
                    opinionViewModels.Add(new OpinionViewModels()
                    {
                        ProductName = row.AspNetProduct.ProductName,
                        RatingId = row.Id_Rating,
                        RateService = row.RateService.ToString(),
                        RateTaste = row.RateTaste.ToString(),
                        RateIngredients = row.RateIngredients.ToString(),
                        ImageUrls = (from AspNetRatingPicture in db.AspNetRatingPictures
                                     where AspNetRatingPicture.Id_Rating == row.Id_Rating
                                     select AspNetRatingPicture.Url).ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        [Route("opinia")]
        [Route("Home/opinia/{id?}")]
        [AllowAnonymous]
        public ActionResult OpinionDescription(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int opinionId = Convert.ToInt32(id);
                ViewBag.id = opinionId;
                DBEntities db = new DBEntities();
                OpinionViewModels opinionViewModel = new OpinionViewModels();
                opinionViewModel.RatingId = opinionId;
                var opinion = (from AspNetRating in db.AspNetRatings
                               join AspNetProduct in db.AspNetProducts on AspNetRating.Id_Product equals AspNetProduct.Id_Product
                               join AspNetUser in db.AspNetUsers on AspNetRating.Who equals AspNetUser.Id
                               where AspNetRating.Id_Rating == opinionId
                               select new
                               {
                                   RateService = AspNetRating.RateService,
                                   RateTaste = AspNetRating.RateTaste,
                                   RateIngredients = AspNetRating.RateIngredients,
                                   Comment = AspNetRating.Comment,
                                   AddedBy = AspNetUser.NickName,
                                   AddedDate = AspNetRating.Date,
                                   ProductName = AspNetProduct.ProductName
                               }
                                   ).FirstOrDefault();
                opinionViewModel.ImageUrls = (from AspNetRatingPicture in db.AspNetRatingPictures
                                              where AspNetRatingPicture.Id_Rating == opinionId
                                              select AspNetRatingPicture.Url).ToList();
                opinionViewModel.ProductName = opinion.ProductName.ToString();
                opinionViewModel.RateService = opinion.RateService.ToString();
                opinionViewModel.RateTaste = opinion.RateTaste.ToString();
                opinionViewModel.RateIngredients = opinion.RateIngredients.ToString();
                opinionViewModel.AddedBy = opinion.AddedBy.ToString();
                opinionViewModel.AddedDate = Convert.ToDateTime(opinion.AddedDate);
                if (!String.IsNullOrEmpty(opinion.Comment))
                {
                    opinionViewModel.Comment = opinion.Comment.ToString();
                }
                return View(opinionViewModel);
            }
            else
            {
                return RedirectToAction("Opinion", "Home");
            }
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
                    //ModelState.Clear();
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
        [Authorize]
        public ActionResult AddProducts(ProductFormModels prd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DBEntities db = new DBEntities();
                    List<string> zapisz = new List<string>();
                    zapisz = SaveIconToProduct(prd);
                    AspNetProduct product = new AspNetProduct();
                    AspNetPlace loc = new AspNetPlace();
                    AspNetImage image = new AspNetImage();


                    var query = db.AspNetCategories.Where(s => s.CategoryName == prd.CategoryName).Select(s => s.Id_Category).FirstOrDefault();

                    if (query != 0)
                    {
                        var queryl = db.AspNetPlaces.Where(s => s.PlaceName == prd.LocName.ToString()).Select(s => s.Id_Place).FirstOrDefault();

                        int querynp = db.AspNetProducts.Where(s => s.ProductName == prd.ProductName).Count();


                        prd.n = db.AspNetProducts.Count() + 1;

                        //this.AddNotification(n.ToString(), NotificationType.ERROR);


                        if (queryl != 0)
                        {
                            db.AspNetProducts.Add(product);
                            string uniq = prd.ProductName + querynp.ToString();
                            
                            product.Id_Place = queryl;
                            product.ProductName = prd.ProductName;
                            product.UniqName = uniq;
                            product.ProductDescription = prd.ProductDescription;
                            product.Id_Category = query;

                            var queru = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(s => s.Id).FirstOrDefault();
                            product.Who = queru;

                            db.SaveChanges();

                            var queryp = db.AspNetProducts.Where(s => s.UniqName == uniq).Select(s => s.Id_Product).FirstOrDefault();

                            foreach (var item in zapisz)
                            {
                                image.Url = item;
                                image.Id_Product = queryp;
                                db.AspNetImages.Add(image);
                                db.SaveChanges();
                            }

                            var wiadomosc = ConfigurationManager.AppSettings["EmailContactUs"].ToString();
                            bool OK = false;
                            int allSize = 0;

                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress(wiadomosc);
                            msg.To.Add(new MailAddress(wiadomosc));
                            msg.Subject = "Nowy produkt nr:" + prd.n;
                            msg.Body = "Do bazy został dodany nowy produkt ! " + "\n" + "Email: " + ConfigurationManager.AppSettings["EmailContactUs"].ToString() + "\n" + "Wiadomość: " + "Produkt dodany! Sprawdz baze";



                            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
                            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                                ConfigurationManager.AppSettings["EmailContactUs"].ToString(),
                                ConfigurationManager.AppSettings["PasswordContactUS"].ToString());
                            smtpClient.Credentials = credentials;
                            smtpClient.EnableSsl = true;
                            smtpClient.Send(msg);


                            this.AddNotification("Wiadomość została wysłana, dziękujemy za kontakt.", NotificationType.SUCCESS);

                            ModelState.Clear();

                            this.AddNotification("Produkt został dodany pomyślnie.", NotificationType.SUCCESS);
                        }
                        else
                        {
                            var ql = db.AspNetPlaces.Count() + 1;

                            //this.AddNotification(queryl.ToString(), NotificationType.INFO);
                            loc.Id_Place = ql;
                            loc.PlaceName = prd.LocName;
                            db.AspNetPlaces.Add(loc);
                            db.SaveChanges();

                            string uniq = prd.ProductName + querynp.ToString();
                            
                            product.Id_Place = ql;
                            product.ProductName = prd.ProductName;
                            product.UniqName = uniq;
                            product.ProductDescription = prd.ProductDescription;
                            product.Id_Category = query;
                            var queru = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(s => s.Id).FirstOrDefault();
                            product.Who = queru;
                            db.AspNetProducts.Add(product);
                            db.SaveChanges();

                            var queryp = db.AspNetProducts.Where(s => s.UniqName == uniq).Select(s => s.Id_Product).FirstOrDefault();

                            foreach (var item in zapisz)
                            {

                                image.Url = item;
                                image.Id_Product = queryp;

                                db.AspNetImages.Add(image);
                                db.SaveChanges();
                            }

                            var wiadomosc = ConfigurationManager.AppSettings["EmailContactUs"].ToString();
                            bool OK = false;
                            int allSize = 0;

                            MailMessage msg = new MailMessage();
                            msg.From = new MailAddress(wiadomosc);
                            msg.To.Add(new MailAddress(wiadomosc));
                            msg.Subject = "Nowy produkt nr:" + prd.n;
                            msg.Body = "Do bazy został dodany nowy produkt ! " + "\n" + "Email: " + ConfigurationManager.AppSettings["EmailContactUs"].ToString() + "\n" + "Wiadomość: " + "Produkt dodany! Sprawdz baze";



                            SmtpClient smtpClient = new SmtpClient("smtp.webio.pl", Convert.ToInt32(587));
                            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                                ConfigurationManager.AppSettings["EmailContactUs"].ToString(),
                                ConfigurationManager.AppSettings["PasswordContactUS"].ToString());
                            smtpClient.Credentials = credentials;
                            smtpClient.EnableSsl = true;
                            smtpClient.Send(msg);

                            ModelState.Clear();
                            this.AddNotification("Wiadomość została wysłana, dziękujemy za kontakt.", NotificationType.SUCCESS);


                            ModelState.Clear();
                            this.AddNotification("Produkt został dodany pomyślnie.", NotificationType.SUCCESS);


                        }

                    }

                    else
                    {
                        this.AddNotification("Przepraszamy ,nie ma takiej kategorii w naszej bazie !", NotificationType.ERROR);

                    }

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
                        //ModelState.Clear();
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }
                }
                catch (Exception ex)
                {
                    //ModelState.Clear();
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
                        //ModelState.Clear();
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }
                }
                catch (Exception)
                {
                    //ModelState.Clear();
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
        [AllowAnonymous]
        public JsonResult AutoComplete(string prefix)
        {
            DBEntities db = new DBEntities();
            var Products = (from AspNetProduct in db.AspNetProducts
                            join AspNetImage in db.AspNetImages on AspNetProduct.Id_Product equals AspNetImage.Id_Product
                            join AspNetPlaces in db.AspNetPlaces on AspNetProduct.Id_Place equals AspNetPlaces.Id_Place
                            where AspNetProduct.ProductName.Contains(prefix)

                            select new
                            {
                                label = AspNetProduct.ProductName,
                                img = AspNetImage.Url,
                                city = AspNetPlaces.PlaceName,
                                val = AspNetProduct.Id_Product


                            }).ToList();

            return Json(Products);
        }
        [AllowAnonymous]
        public JsonResult AutoCompleteCategory(string prefix)
        {
            DBEntities db = new DBEntities();
            var Products = (from AspNetCategory in db.AspNetCategories
                            where AspNetCategory.CategoryName.Contains(prefix)
                            select new
                            {
                                label = AspNetCategory.CategoryName,
                                val = AspNetCategory.Id_Category
                            }).ToList();

            return Json(Products);
        }
        public JsonResult AutoCompleteCity(string prefix)
        {
            DBEntities db = new DBEntities();
            var Products = (from AspNetCity in db.AspNetCities
                            where AspNetCity.LocalizationCity.Contains(prefix)
                            select new
                            {
                                label = AspNetCity.LocalizationCity,
                                val = AspNetCity.Id_City
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
            if (ModelState.IsValid)
            {
                try
                {
                    DBEntities db = new DBEntities();

                    //Console.WriteLine(zapisz);
                    int querynp = db.AspNetProducts.Where(s => s.ProductName == opn.PName).Count();
                    var verify = db.AspNetProducts.Where(s => s.ProductName == opn.PName).FirstOrDefault();
                    AspNetRating rate = new AspNetRating();


                    var query = db.AspNetProducts.Where(s => s.UniqName == opn.PName + (querynp - 1).ToString()).Select(s => s.Id_Product).FirstOrDefault();
                    //this.AddNotification(opn.PName + querynp.ToString(), NotificationType.INFO);


                    if (query != 0 && verify.CzyDobre == true)
                    {
                        List<string> zapisz = new List<string>();
                        var user = db.AspNetProducts.Where(u => u.UniqName == opn.PName + (querynp - 1).ToString()).FirstOrDefault();
                        var queru = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).Select(s => s.Id).FirstOrDefault();
                        var uni = db.AspNetRatings.Where(u => u.Id_Product == query && u.Who == queru).Count();
                        //this.AddNotification(uni.ToString(), NotificationType.ERROR);

                        if (uni == 0)
                        {
                            if (opn.RateIngredients != 0 && opn.RateService != 0 && opn.RateTaste != 0)
                            {
                                if (user.Opinion_Counter == 0)
                                {
                                    user.AvarageService = 0;
                                    user.AvarageTaste = 0;
                                    user.AvarageIngredients = 0;
                                }

                                user.Opinion_Counter += 1;
                                user.AvarageTaste += opn.RateTaste;
                                user.AvarageService += opn.RateService;
                                user.AvarageIngredients += opn.RateIngredients;

                                

                                zapisz = SaveImagesToOpinion(opn);
                                
                                rate.Id_Product = query;

                                rate.RateIngredients = opn.RateIngredients;
                                rate.RateService = opn.RateService;
                                rate.RateTaste = opn.RateTaste;
                                rate.Comment = opn.Review;

                                rate.Who = queru;
                                rate.Date = DateTime.Now;
                                rate.RateTotal = (rate.RateIngredients + rate.RateService + rate.RateTaste) / 3;
                                db.AspNetRatings.Add(rate);
                                db.SaveChanges();


                                AspNetRatingPicture image = new AspNetRatingPicture();
                                foreach (var item in zapisz)
                                {

                                    image.Url = item;
                                    image.Id_Rating = rate.Id_Rating;
                                    db.AspNetRatingPictures.Add(image);
                                    db.SaveChanges();
                                }
                                ModelState.Clear();
                                this.AddNotification("Opinia została wysłana, dziękujemy za opinię.", NotificationType.SUCCESS);
                            }
                            else
                            {
                                this.AddNotification("Jedna z ocen pozostała pusta ,uzupełnij ją!", NotificationType.ERROR);
                            }



                        }

                        else
                        {
                            this.AddNotification("Opinia została już wystawiona! Każdy użytkownik może ocenić dany produkt tylko raz", NotificationType.WARNING);
                        }


                    }
                    else if (verify.CzyDobre == false)
                    {
                        this.AddNotification("Produkt nie został jeszcze zweryfikowany!", NotificationType.ERROR);
                    }
                    else
                    {
                        this.AddNotification("Nie ma takiego produktu w naszej bazie ! Wprowadź ponownie nazwę produktu lub dodaj nowy!", NotificationType.ERROR);

                    }






                }
                catch (Exception ex)
                {
                    //ModelState.Clear();
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
                        //ModelState.Clear();
                        //this.AddNotification("Pliki zostały pomyślnie przesłane", NotificationType.SUCCESS);
                    }
                }
                catch (Exception ex)
                {
                    // ModelState.Clear();
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
        [Route("losuj-danie")]
        [Route("Home/GetDish")]
        [HttpGet]
        public ActionResult GetDish()
        {





            return View();
        }

        [Route("losuj-danie")]
        [Route("Home/GetDish")]
        [HttpPost]
        public ActionResult GetDish(OpinionViewModels mod)
        {
            List<OpinionViewModels> opinionViewModels = new List<OpinionViewModels>();
            if (ModelState.IsValid)
            {
                try
                {
                    DBEntities db = new DBEntities();

                    AspNetProduct prod = new AspNetProduct();
                    AspNetRating rate = new AspNetRating();



                    List<int> numbers = new List<int>();

                   

                   
                    for (int i=0;i<=2;i++)
                    {
                        numbers = db.AspNetRatings.Select(u => u.Id_Product).Distinct().ToList();
                        Random r = new Random();
                        int m = r.Next(numbers.Count());
                        int n = numbers[m];

                        var product = db.AspNetProducts.Where(u => u.Id_Product == n).Select(u => u.ProductName).FirstOrDefault();
                        var rateS = db.AspNetRatings.Where(u => u.Id_Product == n).Select(u => u.RateService).FirstOrDefault();
                        var rateP = db.AspNetRatings.Where(u => u.Id_Product == n).Select(u => u.RateIngredients).FirstOrDefault();
                        var rateT = db.AspNetRatings.Where(u => u.Id_Product == n).Select(u => u.RateTaste).FirstOrDefault();

                        string result = product.ToString() + " Smak: " + rateT.ToString() + " Cena: " + rateP.ToString() + " Obsługa: " + rateS.ToString() + " " + n.ToString();

                        this.AddNotification(result.ToString(), NotificationType.INFO);



                        IQueryable<AspNetRating> SQLresult = db.AspNetRatings;
                        SQLresult.Join(db.AspNetProducts,
                            a => a.Id_Product,
                            b => b.Id_Product,
                            (a, b) => a.Id_Product);

                        SQLresult.Select(a => new {
                            a.AspNetProduct.ProductName,
                            a.Id_Rating,
                            a.RateService,
                            a.RateTaste,
                            a.RateIngredients
                        });
                        SQLresult.ToList();

                        var qur = db.AspNetRatings.Where(u => u.Id_Product == n).Select(u => u.Id_Rating).FirstOrDefault();


                        opinionViewModels.Add(new OpinionViewModels()
                        {
                            ProductName = product.ToString(),
                            RatingId = qur,
                            RateService = rateS.ToString(),
                            RateTaste = rateT.ToString(),
                            RateIngredients = rateP.ToString(),
                            ImageUrls = (from AspNetRatingPicture in db.AspNetRatingPictures
                                         where AspNetRatingPicture.Id_Rating == qur
                                         select AspNetRatingPicture.Url).ToList()
                        });
                    }
                    




                    




                }
                catch (Exception ex)
                {
                    //ModelState.Clear();
                    this.AddNotification($"Przepraszamy, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                }
            }

            return View("Dish", opinionViewModels);
        }
    }

}