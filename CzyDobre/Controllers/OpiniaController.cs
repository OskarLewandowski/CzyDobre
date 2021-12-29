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
    public class OpiniaController : Controller
    {
        DBEntities db = new DBEntities();

        [Route("lista-opinii")]
        [Route("Opinia/OpiniaList")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult OpiniaList()
        {
            var produktList = db.AspNetRatings.ToList();
            return View(produktList);
        }

        [Route("dodawanie-zastrzezenia-opinia")]
        [Route("Opinia/Objections")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Objections(AspNetRating obj)
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

        [Route("zatwierdz-opinie")]
        [Route("Opinia/ConfirmOpinie")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult ConfirmOpinie(int idRating, short rateService, short rateTaste, short rateIngriedients, short rateComposition, string comment, bool czyDobre, double? rateTotal, int idProduct, string who, string objections, DateTime? date)
        {
            if (idRating != 0)
            {
                AspNetRating aspNetRating = new AspNetRating();

                aspNetRating.Id_Rating = idRating;
                aspNetRating.RateService = rateService;
                aspNetRating.RateTaste = rateTaste;
                aspNetRating.RateIngredients = rateIngriedients;
                aspNetRating.RateComposition = rateComposition;
                aspNetRating.Comment = comment;
                aspNetRating.CzyDobre = true;
                aspNetRating.RateTotal = rateTotal;
                aspNetRating.Id_Product = idProduct;
                aspNetRating.Who = who;
                aspNetRating.Objections = null;
                aspNetRating.Date = date;
                
                db.Entry(aspNetRating).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                this.AddNotification($"Opinia, zatwierdzona pomyślnie", NotificationType.SUCCESS);
            }

            var opiniaList = db.AspNetRatings.ToList();
            return View("OpiniaList", opiniaList);
        }

        [Route("cofnij-zatwierdzenie-opinii")]
        [Route("Opinia/BackConfirmOpinie")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult BackConfirmOpinie(int idRating, short rateService, short rateTaste, short rateIngriedients, short rateComposition, string comment, bool czyDobre, double? rateTotal, int idProduct, string who, string objections, DateTime? date)
        {
            if (idRating != 0)
            {
                AspNetRating aspNetRating = new AspNetRating();

                aspNetRating.Id_Rating = idRating;
                aspNetRating.RateService = rateService;
                aspNetRating.RateTaste = rateTaste;
                aspNetRating.RateIngredients = rateIngriedients;
                aspNetRating.RateComposition = rateComposition;
                aspNetRating.Comment = comment;
                aspNetRating.CzyDobre = false;
                aspNetRating.RateTotal = rateTotal;
                aspNetRating.Id_Product = idProduct;
                aspNetRating.Who = who;
                aspNetRating.Objections = objections;
                aspNetRating.Date = date;

                db.Entry(aspNetRating).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                this.AddNotification($"Zatwierdzenie opinii, cofnięte pomyślnie", NotificationType.SUCCESS);
            }

            var opiniaList = db.AspNetRatings.ToList();
            return View("OpiniaList", opiniaList);
        }

        [Route("dodaj-zastrzezenie-opinii")]
        [Route("Opinia/AddObjections")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult AddObjections(AspNetRating model)
        {
            try
            {
                if (model.Objections == null)
                {
                    this.AddNotification($"Należy wprowadzić treść zastrzeżenia", NotificationType.ERROR);
                    return View("Objections");
                }

                if (model.Id_Rating != 0)
                {
                    var nowaData = db.AspNetRatings.Where(u => u.Id_Rating == model.Id_Rating).Select(u => u.Date).FirstOrDefault();

                    AspNetRating aspNetRating = new AspNetRating();

                    aspNetRating.Id_Rating = model.Id_Rating;
                    aspNetRating.RateService = model.RateService;
                    aspNetRating.RateTaste = model.RateTaste;
                    aspNetRating.RateIngredients = model.RateIngredients;
                    aspNetRating.RateComposition = model.RateComposition;
                    aspNetRating.Comment = model.Comment;
                    aspNetRating.CzyDobre = model.CzyDobre;
                    aspNetRating.RateTotal = model.RateTotal;
                    aspNetRating.Id_Product = model.Id_Product;
                    aspNetRating.Who = model.Who;
                    aspNetRating.Objections = model.Objections;
                    aspNetRating.Date = nowaData;

                    db.Entry(aspNetRating).State = System.Data.Entity.EntityState.Modified;
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
                return View("Objections");
            }
            return View("Objections");
        }


        [Route("usuwanie-opinii")]
        [Route("Opinia/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete( int idRating, int idProduct, string nazwaProduktu)
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
                    opiniaDecres.Opinion_Counter -= 1;
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
                return View("OpiniaList", opiniaList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Bład: " + ex, NotificationType.ERROR);
                return RedirectToAction("OpiniaList");
            }
        }
    }
}