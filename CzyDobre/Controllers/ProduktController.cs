using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CzyDobre.Extensions;
using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class ProduktController : Controller
    {
        DBEntities db = new DBEntities();

        [Route("lista-produktow")]
        [Route("Produkt/ProduktList")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult ProduktList()
        {
            var produktList = db.AspNetProducts.ToList();
            return View(produktList);
        }

        [Route("dodawanie-zastrzezenia")]
        [Route("Produkt/Objections")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Objections(AspNetProduct obj)
        {
            if (obj != null)
            {
                return View(obj);
            }
            else
            {
                return View();
            }
        }

        public JsonResult AutoCompleteCategory(int id)
        {
            DBEntities db = new DBEntities();
            var Products = (from AspNetCategory in db.AspNetCategories
                            where AspNetCategory.Id_Category.Equals(id)
                            select new
                            {
                                label = AspNetCategory.Id_Category,
                                val = AspNetCategory.CategoryName
                            }).ToList();

            return Json(Products);
        }

        [Route("zatwierdz-produkt")]
        [Route("Produkt/ConfirmProdukt")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult ConfirmProdukt(int idProduct, int idCategory, int? idPlace, string productDescription, string productName, int opinionCounter, int? avarageTaste, int? avarageService, int? avarageIngredients, string who, string uniqName, bool czyDobre, string objections)
        {
            if (idProduct != 0)
            {
                AspNetProduct aspNetProduct = new AspNetProduct();

                aspNetProduct.Id_Product = idProduct;
                aspNetProduct.Id_Category = idCategory;
                aspNetProduct.ProductDescription = productDescription;
                aspNetProduct.ProductName = productName;
                aspNetProduct.Opinion_Counter = opinionCounter;
                aspNetProduct.AvarageTaste = avarageTaste;
                aspNetProduct.AvarageService = avarageService;
                aspNetProduct.AvarageIngredients = avarageIngredients;
                aspNetProduct.Who = who;
                aspNetProduct.UniqName = uniqName;
                aspNetProduct.Id_Place = idPlace;
                aspNetProduct.CzyDobre = true;
                aspNetProduct.Objections = null;

                db.Entry(aspNetProduct).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                this.AddNotification($"Produkt, zatwierdzony pomyślnie", NotificationType.SUCCESS);
            }

            var produktList = db.AspNetProducts.ToList();
            return View("ProduktList", produktList);
        }

        [Route("cofnij-zatwierdzenie-produktu")]
        [Route("Produkt/BackConfirmProdukt")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult BackConfirmProdukt(int idProduct, int idCategory, int? idPlace, string productDescription, string productName, int opinionCounter, int? avarageTaste, int? avarageService, int? avarageIngredients, string who, string uniqName, bool czyDobre, string objections)
        {
            if (idProduct != 0)
            {
                AspNetProduct aspNetProduct = new AspNetProduct();

                aspNetProduct.Id_Product = idProduct;
                aspNetProduct.Id_Category = idCategory;
                aspNetProduct.ProductDescription = productDescription;
                aspNetProduct.ProductName = productName;
                aspNetProduct.Opinion_Counter = opinionCounter;
                aspNetProduct.AvarageTaste = avarageTaste;
                aspNetProduct.AvarageService = avarageService;
                aspNetProduct.AvarageIngredients = avarageIngredients;
                aspNetProduct.Who = who;
                aspNetProduct.UniqName = uniqName;
                aspNetProduct.Id_Place = idPlace;
                aspNetProduct.CzyDobre = false;
                aspNetProduct.Objections = objections;

                db.Entry(aspNetProduct).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                this.AddNotification($"Zatwierdzenie produktu, cofnięte pomyślnie", NotificationType.SUCCESS);
            }

            var produktList = db.AspNetProducts.ToList();
            return View("ProduktList", produktList);
        }

        [Route("dodaj-zastrzezenie")]
        [Route("Produkt/AddObjections")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult AddObjections(AspNetProduct model)
        {
            try
            {
                if(model.Objections == null)
                {
                    this.AddNotification($"Należy wprowadżić treść zastrzeżenia", NotificationType.ERROR);
                    return View("Objections");
                }

                if (ModelState.IsValid)
                {
                    AspNetProduct aspNetProduct = new AspNetProduct();

                    aspNetProduct.Id_Product = model.Id_Product;
                    aspNetProduct.Id_Category = model.Id_Category;
                    aspNetProduct.ProductDescription = model.ProductDescription;
                    aspNetProduct.ProductName = model.ProductName;
                    aspNetProduct.Opinion_Counter = model.Opinion_Counter;
                    aspNetProduct.AvarageTaste = model.AvarageTaste;
                    aspNetProduct.AvarageService = model.AvarageService;
                    aspNetProduct.AvarageIngredients = model.AvarageIngredients;
                    aspNetProduct.Who = model.Who;
                    aspNetProduct.UniqName = model.UniqName;
                    aspNetProduct.Id_Place = model.Id_Place;
                    aspNetProduct.CzyDobre = model.CzyDobre;
                    aspNetProduct.Objections = model.Objections;

                    db.Entry(aspNetProduct).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    this.AddNotification($"Zastrzeżenie, dodane pomyślnie", NotificationType.SUCCESS);
                    return View("Objections");
                }
                this.AddNotification($"Błąd!, Błędne dane", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                this.AddNotification($"Ups!, napotkaliśmy pewien problem. {ex.Message}", NotificationType.ERROR);
                return View("Edit");
            }
            return View("Edit");
        }

        [Route("usuwanie-produktu")]
        [Route("Produkt/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int idProdukt, string nazwaProduktu, string imageUrl)
        {
            try
            {

                List<AspNetRatingPicture> lista = new List <AspNetRatingPicture>();

                Account account = new Account(
                ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                Cloudinary cloudinary = new Cloudinary(account);

                var Produkt = db.AspNetProducts.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
                var ZdjecieProduktu = db.AspNetImages.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
                var Opinia = db.AspNetRatings.Where(m => m.Id_Product == idProdukt).ToList();
                var idOpini = db.AspNetRatings.Where(m => m.Id_Product == idProdukt).Select(m => m.Id_Rating).FirstOrDefault();
                var idZdjecie = db.AspNetRatingPictures.Where(m => m.Id_Rating == idOpini).ToList();
                foreach (AspNetRating r in Opinia)
                {
                    lista.AddRange(db.AspNetRatingPictures.Where(m => m.Id_Rating == r.Id_Rating).ToList());
                }
                


                if (Produkt != null )
                {



                    if(lista != null)
                    {
                        db.AspNetRatingPictures.RemoveRange(lista);
                    }
                    
                    if(Opinia != null)
                    {
                        db.AspNetRatings.RemoveRange(Opinia);
                    }

                    if (ZdjecieProduktu != null)
                    {
                        db.AspNetImages.Remove(ZdjecieProduktu);
                    }
                        
                    
                    db.AspNetProducts.Remove(Produkt);
                    db.SaveChanges();


                    //this.AddNotification(" "+Produkt.ToString()+"  "+ZdjecieProduktu.ToString() + " "+ Opinia.ToString() + " "+idZdjecie.ToString() + " "+idOpini.ToString(), NotificationType.INFO);



                    if(idZdjecie != null)
                    {
                        //usuwan rozszezenia .jpg 4 i .jpng 5
                        var ID4 = imageUrl.Remove(imageUrl.Length - 4);
                        var ID5 = imageUrl.Remove(imageUrl.Length - 5);
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
                   

                    this.AddNotification("Produkt \"" + nazwaProduktu + "\" został pomyślnie usunięty! " , NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotification("Produkt nie został usunięty!", NotificationType.ERROR);
                }
                var produktList = db.AspNetProducts.ToList();
                return View("ProduktList", produktList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("ProduktList");
            }
        }
    }
}