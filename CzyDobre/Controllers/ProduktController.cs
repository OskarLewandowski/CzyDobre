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
        public ActionResult EditProdukt(AspNetProduct obj)
        {
            return View();
        }

        [Route("usuwanie-produktu")]
        [Route("Produkt/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int idProdukt, string nazwaProduktu)
        {
            try
            {
                var idProduktu = db.AspNetProducts.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
                var idZdjecia = db.AspNetImages.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
                if (idProduktu != null && idZdjecia != null)
                {
                    db.AspNetProducts.Remove(idProduktu);
                    db.AspNetImages.Remove(idZdjecia);
                    db.SaveChanges();
                    this.AddNotification("Produkt \"" + nazwaProduktu + "\" został pomyślnie usunięty!", NotificationType.SUCCESS);
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