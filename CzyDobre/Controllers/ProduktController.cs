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

        [Route("edytowanie-produktu")]
        [Route("Produkt/Edit")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit(AspNetProduct obj)
        {
            if (obj != null)
            {
                return View(obj);
            }
            else
            {
                return View();
            }
            return View();
        }

        [Route("edytuj-produkt")]
        [Route("Produkt/EditProdukt")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult EditProdukt(AspNetProduct model)
        {
            try
            {
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

                    db.Entry(aspNetProduct).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    this.AddNotification($"Produkt, edytowany pomyślnie", NotificationType.SUCCESS);
                    return View("Edit");
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
                Account account = new Account(
                ConfigurationManager.AppSettings["CloudinaryName"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiKey"].ToString(),
                ConfigurationManager.AppSettings["CloudinaryApiSecret"].ToString());
                Cloudinary cloudinary = new Cloudinary(account);

                var idProduktu = db.AspNetProducts.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
                var idZdjecia = db.AspNetImages.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
                if (idProduktu != null && idZdjecia != null)
                {
                    db.AspNetProducts.Remove(idProduktu);
                    db.AspNetImages.Remove(idZdjecia);

                    db.SaveChanges();
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

                    this.AddNotification("Produkt \"" + nazwaProduktu + "\" został pomyślnie usunięty! " + ID4 + ID5, NotificationType.SUCCESS);
                }
                else
                {
                    this.AddNotification("Produkt nie został usunięty!", NotificationType.INFO);
                }
                var produktList = db.AspNetProducts.ToList();
                return View("ProduktList", produktList);
            }
            catch (Exception ex)
            {
                this.AddNotification($"Nie można usunać produktu, który posiada opinię", NotificationType.ERROR);
                return RedirectToAction("ProduktList");
            }
            var produktListt = db.AspNetProducts.ToList();
            return View("ProduktList", produktListt);
        }
    }
}