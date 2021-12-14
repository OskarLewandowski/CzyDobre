using CzyDobre.Extensions;
using CzyDobre.Models;
using System;
using System.Collections.Generic;
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
        [Route("Opinia/AddObjectionsOpinia")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult AddObjectionsOpinia(AspNetRating model)
        {
            try
            {
                if (model.Objections == null)
                {
                    this.AddNotification($"Należy wprowadzić treść zastrzeżenia", NotificationType.ERROR);
                    return View("Objections");
                }

                if (ModelState.IsValid)
                {
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
                    aspNetRating.Date = model.Date;

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

    }
}