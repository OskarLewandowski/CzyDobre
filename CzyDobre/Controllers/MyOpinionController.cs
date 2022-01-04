using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CzyDobre.Extensions;
using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin, Moderator,User")]
    public class MyOpinionController : Controller
    {
        DBEntities db = new DBEntities();

        [Route("moje-opinie")]
        [Route("MyOpinion/MojaOpinia")]
        [Authorize(Roles = "Admin, Moderator,User")]
        public ActionResult MojaOpinia()
        {
            var produktList = db.AspNetRatings.ToList();
            return View(produktList);
        }

        [Route("usuwanie-mopjej-opinii")]
        [Route("MyOpinion/DeleteO")]
        [Authorize(Roles = "Admin, Moderator,User")]
        public ActionResult DeleteO(int idRating, int idProduct, string nazwaProduktu)
        {
            try
            {
                List<AspNetRatingPicture> lista = new List<AspNetRatingPicture>();

                Account account = new Account(
                ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                Cloudinary cloudinary = new Cloudinary(account);

                var Opinia = db.AspNetRatings.Where(m => m.Id_Rating == idRating).ToList();


                foreach (AspNetRating r in Opinia)
                {
                    var opiniaDecres = db.AspNetProducts.Where(m => m.Id_Product == r.Id_Product).FirstOrDefault();
                    opiniaDecres.Opinion_Counter -=1;
                    lista.AddRange(db.AspNetRatingPictures.Where(m => m.Id_Rating == r.Id_Rating).ToList());
                }

                if (Opinia != null)
                {
                    if (lista != null)
                    {
                        db.AspNetRatingPictures.RemoveRange(lista);
                    }

                    if (Opinia != null)
                    {
                        db.AspNetRatings.RemoveRange(Opinia);
                    }


                    db.SaveChanges();

                    var listaZdjecUrl = db.AspNetRatingPictures.Where(u => u.Id_Rating == idRating).Select(u => u.Url).ToList();

                    if (listaZdjecUrl != null)
                    {
                        for (int i = 0; i < listaZdjecUrl.Count; i++)
                        {
                            //usuwan rozszezenia .jpg 4 i .jpng 5
                            var ID4 = listaZdjecUrl[i].Remove(listaZdjecUrl[i].Length - 4);
                            var ID5 = listaZdjecUrl[i].Remove(listaZdjecUrl[i].Length - 5);
                            var nameId4 = "CzyDobre-images/" + ID4;
                            var nameId5 = "CzyDobre-images/" + ID5;

                            var deletionParams4 = new DeletionParams(ID4)
                            {
                                PublicId = nameId4
                            };
                            var deletionResult = cloudinary.Destroy(deletionParams4);

                            var deletionParams5 = new DeletionParams(ID5)
                            {
                                PublicId = nameId5
                            };
                            var deletionResult4 = cloudinary.Destroy(deletionParams5);
                        }
                    }

                    this.AddNotification("Opinia \"" + nazwaProduktu + "\" została pomyślnie usunięta! ", NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotification("Produkt nie został usunięty!", NotificationType.ERROR);
                }
                var opiniaList = db.AspNetRatings.ToList();
                return View("MojaOpinia", opiniaList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("MojaOpinia");
            }
        }


        [Route("edytor-opinii")]
        [Route("MyOpinion/Editor")]
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator,User")]
        public ActionResult Editor(EditOpinionViewModels edit, int idRating)
        {
            try
            {
                DBEntities db = new DBEntities();

                var queru= db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.Who).FirstOrDefault();
                var quern = db.AspNetUsers.Where(m => m.Id == queru).Select(m=>m.UserName).FirstOrDefault();

                

                if(User.Identity.Name == quern)
                {

                    int idProduktu = db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.Id_Product).FirstOrDefault();
                    string nazwaProduktu = db.AspNetProducts.Where(m => m.Id_Product == idProduktu).Select(m => m.ProductName).FirstOrDefault().ToString();
                    short rateT = db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.RateTaste).FirstOrDefault();
                    short rateS = db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.RateService).FirstOrDefault();
                    short rateP = db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.RateIngredients).FirstOrDefault();
                    string comm = db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.Comment).FirstOrDefault();

                    List<string> photos = new List<string>();

                    photos = db.AspNetRatingPictures.Where(m => m.Id_Rating == idRating).Select(m => m.Url).ToList();



                    edit.PName = nazwaProduktu;
                    edit.RateTaste = rateT;
                    edit.RateService = rateS;
                    edit.RateIngredients = rateP;
                    edit.Review = comm;
                    // edit.Photo = photos;
                    // ViewData["PN"] = nazwaProduktu;
                    // ViewData["RT"] = rateT;
                    // ViewData["RS"] = rateS;
                    // ViewData["RP"] = rateP;

                    Session["idRating"] = idRating;
                    Session["Photos"] = photos;
                    

                    // this.AddNotification(" "+idRating+" "+ idProduktu + " " + nazwaProduktu + " " + rateT + " " + rateP + " " + rateS + " " + comm,NotificationType.INFO);
                    //this.AddNotification(TempData["idRating"].ToString(), NotificationType.INFO);







                    return View(edit);
                }
                else
                {
                    this.AddNotification($"Brak dostępu! ", NotificationType.ERROR);
                    return RedirectToAction("MojaOpinia");
                }
                
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("MojaOpinia");
            }


           
        }


        [HttpPost]
        [Route("edytor-opinii")]
        [Route("MyOpinion/Editor")]
        [Authorize(Roles = "Admin, Moderator,User")]
        public ActionResult Editor(EditOpinionViewModels edit)
        {
            DBEntities db = new DBEntities();

            var id = Session["idRating"];

           
                var entity = db.AspNetRatings.FirstOrDefault(item => item.Id_Rating == (int)id);

                // Validate entity is not null
                if (entity != null)
                {
                    // Answer for question #2

                    // Make changes on entity
                    entity.RateIngredients = edit.RateIngredients;
                    entity.RateService = edit.RateService;
                    entity.RateTaste = edit.RateTaste;
                    entity.Comment = edit.Review;
                    entity.CzyDobre = false;
                    // Update entity in DbSet
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                    // Save changes in database
                    db.SaveChanges();
                    
                }



           



            //this.AddNotification(id.ToString(), NotificationType.INFO);





            return View(edit);
        }


        [Route("zarzadzaj-zdjeciami")]
        [Route("MyOpinion/ManageIMG")]
        [Authorize(Roles = "Admin, Moderator,User")]
        public ActionResult ManageIMG()
        {

            var produktList = db.AspNetRatings.ToList();
            return View(produktList);

            


        }


        [Route("usuwanie-zdjecia")]
        [Route("MyOpinion/ManageIMG")]
        [Authorize(Roles = "Admin, Moderator,User")]

        public ActionResult DeleteIMG(int id)
        {
            try
            {
                

                Account account = new Account(
                ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                Cloudinary cloudinary = new Cloudinary(account);

                AspNetRatingPicture pic;

                
                 using (DBEntities db = new DBEntities())
                 {
                     
                     pic = db.AspNetRatingPictures.Where(d => d.Id_Picture == id).FirstOrDefault();
                     db.AspNetRatingPictures.Remove(pic);
                     db.SaveChanges();

                 }

                var listaZdjecUrl = db.AspNetRatingPictures.Where(u => u.Id_Picture == id).Select(u => u.Url).ToList();

                this.AddNotification(listaZdjecUrl.Count.ToString(), NotificationType.INFO);

                if (listaZdjecUrl != null)
                {
                    for (int i = 0; i < listaZdjecUrl.Count; i++)
                    {
                        //usuwan rozszezenia .jpg 4 i .jpng 5
                        var ID4 = listaZdjecUrl[i].Remove(listaZdjecUrl[i].Length - 4);
                        var ID5 = listaZdjecUrl[i].Remove(listaZdjecUrl[i].Length - 5);
                        var nameId4 = "CzyDobre-images/" + ID4;
                        var nameId5 = "CzyDobre-images/" + ID5;

                        var deletionParams4 = new DeletionParams(ID4)
                        {
                            PublicId = nameId4
                        };
                        var deletionResult = cloudinary.Destroy(deletionParams4);

                        var deletionParams5 = new DeletionParams(ID5)
                        {
                            PublicId = nameId5
                        };
                        var deletionResult4 = cloudinary.Destroy(deletionParams5);
                    }
                }



                this.AddNotification("Zdjęcie usunięto pomyślnie", NotificationType.INFO);
                var opiniaList = db.AspNetRatings.ToList();
                return View("ManageIMG", opiniaList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("ManageIMG");
            }

        }



        [Route("zmiana-zdjecia")]
        [Route("MyOpinion/ChangeIMG")]
        [Authorize]
        public ActionResult ChangeIMG(int id, int idRating)
        {
            var queru = db.AspNetRatings.Where(m => m.Id_Rating == idRating).Select(m => m.Who).FirstOrDefault();
            var quern = db.AspNetUsers.Where(m => m.Id == queru).Select(m => m.UserName).FirstOrDefault();

            if (User.Identity.Name == quern)
            {

                Session["idPicture"] = id;
                Session["idRat"] = idRating;
            }                 
            return View();



        }



        [HttpPost]
        [Route("zmiana-zdjecia")]
        [Route("MyOpinion/ChangeIMG")]
        public ActionResult ChangeIMG(ChangeIMGViewModels img)
        {
            try
            {
                int id = (int)Session["idPicture"];
                int idRating = (int)Session["idRat"];

                var rate = db.AspNetRatingPictures.Where(d => d.Id_Picture == id).Select(d => d.Id_Rating).FirstOrDefault();

                List<string> zapisz = new List<string>();

                zapisz = SaveIconToProduct(img);

                AspNetRatingPicture image = new AspNetRatingPicture();
                
                foreach (var item in zapisz)
                {

                    image.Url = item;
                    image.Id_Rating = rate;
                    db.AspNetRatingPictures.Add(image);
                    db.SaveChanges();
                }
                
                var entity = db.AspNetRatings.FirstOrDefault(item => item.Id_Rating == (int)idRating);

                // Validate entity is not null
                if (entity != null)
                {
                    // Answer for question #2

                    // Make changes on entity
                   
                    entity.CzyDobre = false;
                    // Update entity in DbSet
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                    // Save changes in database
                    db.SaveChanges();

                }

                Account account = new Account(
                ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                Cloudinary cloudinary = new Cloudinary(account);

                AspNetRatingPicture pic;

                
                

                var listaZdjecUrl = db.AspNetRatingPictures.Where(u => u.Id_Picture == id).Select(u => u.Url).ToList();

                this.AddNotification(id.ToString(), NotificationType.INFO);

                if (listaZdjecUrl != null)
                {
                    for (int i = 0; i < listaZdjecUrl.Count; i++)
                    {
                        //usuwan rozszezenia .jpg 4 i .jpng 5
                        var ID4 = listaZdjecUrl[i].Remove(listaZdjecUrl[i].Length - 4);
                        var ID5 = listaZdjecUrl[i].Remove(listaZdjecUrl[i].Length - 5);
                        var nameId4 = "CzyDobre-images/" + ID4;
                        var nameId5 = "CzyDobre-images/" + ID5;

                        var deletionParams4 = new DeletionParams(ID4)
                        {
                            PublicId = nameId4
                        };
                        var deletionResult = cloudinary.Destroy(deletionParams4);

                        var deletionParams5 = new DeletionParams(ID5)
                        {
                            PublicId = nameId5
                        };
                        var deletionResult4 = cloudinary.Destroy(deletionParams5);
                    }
                }

                using (DBEntities db = new DBEntities())
                {

                    pic = db.AspNetRatingPictures.Where(d => d.Id_Picture == id).FirstOrDefault();
                    db.AspNetRatingPictures.Remove(pic);
                    db.SaveChanges();

                }

                this.AddNotification("Zdjęcie zostało zamienione !" , NotificationType.SUCCESS);
                var opiniaList = db.AspNetRatings.ToList();
                return View("ManageIMG", opiniaList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("ManageIMG");
            }

        }
        private List<string> SaveIconToProduct(ChangeIMGViewModels model)
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