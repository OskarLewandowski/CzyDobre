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
    [Authorize(Roles = "Admin, Moderator")]
    public class MyProductController : Controller
    {
        DBEntities db = new DBEntities();

        [Route("moje-produkty")]
        [Route("MyProduct/MojProdukt")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult MojProdukt()
        {
            var produktList = db.AspNetProducts.ToList();
            return View(produktList);
        }

        [Route("usuwanie-mojego-produktu")]
        [Route("MyProduct/DeleteP")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteP(int idProduct, string nazwaProduktu)
        {
            try
            {
                List<AspNetImage> lista = new List<AspNetImage>();

                Account account = new Account(
                ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                Cloudinary cloudinary = new Cloudinary(account);

                var Produkt = db.AspNetProducts.Where(m => m.Id_Product == idProduct).ToList();


                foreach (AspNetProduct r in Produkt)
                {
                    var opiniaDecres = db.AspNetProducts.Where(m => m.Id_Product == r.Id_Product).FirstOrDefault();
                    opiniaDecres.Opinion_Counter -=1;
                    lista.AddRange(db.AspNetImages.Where(m => m.Id_Product == r.Id_Product).ToList());
                }

                if (Produkt != null)
                {
                    if (lista != null)
                    {
                        db.AspNetImages.RemoveRange(lista);
                    }

                    if (Produkt != null)
                    {
                        db.AspNetProducts.RemoveRange(Produkt);
                    }


                    db.SaveChanges();

                    var listaZdjecUrl = db.AspNetImages.Where(u => u.Id_Product == idProduct).Select(u => u.Url).ToList();

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

                    this.AddNotification("Produkt \"" + nazwaProduktu + "\" została pomyślnie usunięta! ", NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotification("Produkt nie został usunięty!", NotificationType.ERROR);
                }
                var produktList = db.AspNetProducts.ToList();
                return View("MojProdukt", produktList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("MojProdukt");
            }
        }


        [Route("edytor-produktu")]
        [Route("MyProduct/EditorP")]
        [HttpGet]
        [Authorize]
        public ActionResult EditorP(EditProductViewModels prd, int idProduct)
        {
            try
            {
                DBEntities db = new DBEntities();

                var queru= db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.Who).FirstOrDefault();
                var quern = db.AspNetUsers.Where(m => m.Id == queru).Select(m=>m.UserName).FirstOrDefault();

                

                if(User.Identity.Name == quern)
                {

                    int idProduktu = db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.Id_Product).FirstOrDefault();
                    string nazwaProduktu = db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.ProductName).FirstOrDefault().ToString();
                    int idCat = db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.Id_Category).FirstOrDefault();
                    string Cat = db.AspNetCategories.Where(m => m.Id_Category == idCat).Select(m => m.CategoryName).FirstOrDefault();
                    int? idPlace = db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.Id_Place).FirstOrDefault();
                    string Place = db.AspNetPlaces.Where(m => m.Id_Place == idPlace).Select(m => m.PlaceName).FirstOrDefault();
                    string Desc = db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.ProductDescription).FirstOrDefault();
                    

                    List<string> photos = new List<string>();

                    photos = db.AspNetImages.Where(m => m.Id_Product == idProduct).Select(m => m.Url).ToList();
                    int img = db.AspNetImages.Where(m => m.Id_Product == idProduct).Select(m => m.Id_Image).FirstOrDefault();


                    prd.ProductName = nazwaProduktu;
                    prd.CategoryName = Cat;
                    prd.LocName = Place;
                    prd.ProductDescription = Desc;
                    
                    // edit.Photo = photos;
                    // ViewData["PN"] = nazwaProduktu;
                    // ViewData["RT"] = rateT;
                    // ViewData["RS"] = rateS;
                    // ViewData["RP"] = rateP;

                    Session["idProduct"] = idProduct;
                    Session["Icons"] = photos;
                    Session["Img"] = img;

                    // this.AddNotification(" "+idRating+" "+ idProduktu + " " + nazwaProduktu + " " + rateT + " " + rateP + " " + rateS + " " + comm,NotificationType.INFO);
                    //this.AddNotification(TempData["idRating"].ToString(), NotificationType.INFO);







                    return View(prd);
                }
                else
                {
                    this.AddNotification($"Brak dostępu! ", NotificationType.ERROR);
                    return RedirectToAction("MojProdukt");
                }
                
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("MojProdukt");
            }


           
        }


        [HttpPost]
        [Route("edytor-produktu")]
        [Route("MyProduct/EditorP")]
        [Authorize]
        public ActionResult EditorP(EditProductViewModels prd)
        {
            DBEntities db = new DBEntities();

            var id = Session["idProduct"];

           
                var entity = db.AspNetProducts.FirstOrDefault(item => item.Id_Product == (int)id);
                int qcat = db.AspNetCategories.Where(m => m.CategoryName == prd.CategoryName).Select(m => m.Id_Category).FirstOrDefault();
                int qpla = db.AspNetPlaces.Where(m => m.PlaceName == prd.LocName).Select(m => m.Id_Place).FirstOrDefault();
            // Validate entity is not null
            if (entity != null)
                {
                    // Answer for question #2

                    // Make changes on entity
                    entity.ProductName = prd.ProductName;
                    entity.Id_Category = qcat;
                    entity.Id_Place= qpla;
                    entity.ProductDescription = prd.ProductDescription;
                    entity.CzyDobre = false;
                    // Update entity in DbSet
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                    // Save changes in database
                    db.SaveChanges();
                    
                }



           



            //this.AddNotification(id.ToString(), NotificationType.INFO);





            return View(prd);
        }


        [Route("zmiana-zdjeciapr")]
        [Route("MyProduct/ChangeIMGpr")]
        [Authorize]
        public ActionResult ChangeIMGpr(int id ,int idProduct)
        {
            var queru = db.AspNetProducts.Where(m => m.Id_Product == idProduct).Select(m => m.Who).FirstOrDefault();
            var quern = db.AspNetUsers.Where(m => m.Id == queru).Select(m => m.UserName).FirstOrDefault();

            if (User.Identity.Name == quern)
            {

                Session["idImgage"] = id;
                Session["idPro"] = idProduct;
            }                 
            return View();



        }



        [HttpPost]
        [Route("zmiana-zdjeciapr")]
        [Route("MyProduct/ChangeIMGpr")]
        public ActionResult ChangeIMGpr(ChangeIMGViewModels img)
        {
            try
            {
                int id = (int)Session["idImgage"];
                int idProduct= (int)Session["idPro"];

                var prd = db.AspNetImages.Where(d => d.Id_Image == id).Select(d => d.Id_Product).FirstOrDefault();

                List<string> zapisz = new List<string>();

                


                zapisz = SaveIconToProduct(img);

                AspNetImage image = new AspNetImage();
                
                foreach (var item in zapisz)
                {

                    image.Url = item;
                    image.Id_Product = prd;
                    db.AspNetImages.Add(image);
                    db.SaveChanges();
                }
                
                var entity = db.AspNetProducts.FirstOrDefault(item => item.Id_Product == (int)idProduct);

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

                AspNetImage pic;


                var listaZdjecUrl = db.AspNetImages.Where(u => u.Id_Image == id).Select(u => u.Url).ToList();

                

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
                   
                    pic = db.AspNetImages.Where(d => d.Id_Image == id).FirstOrDefault();
                    db.AspNetImages.Remove(pic);
                    db.SaveChanges();

                }


                this.AddNotification("Zdjęcie zostało zamienione !" , NotificationType.SUCCESS);
                var produktList = db.AspNetProducts.ToList();
                return View("MojProdukt", produktList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("MojProdukt");
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