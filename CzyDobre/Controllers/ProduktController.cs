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

        // GET: Produkt
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult ProduktList()
        {
            var produktList = db.AspNetProducts.ToList();
            return View(produktList);
        }


        [Route("usuwanie-produktu")]
        [Route("Produkt/Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int idProdukt, string nazwaProduktu)
        {
            var idProduktu = db.AspNetProducts.Where(m => m.Id_Product == idProdukt).FirstOrDefault();
            var idZdjecia = db.AspNetImages.Where(m => m.Id_Product == idProdukt).FirstOrDefault();

            if (idProduktu != null)
            {
                db.AspNetProducts.Remove(idProduktu);
                if(idZdjecia != null)
                {
                    db.AspNetImages.Remove(idZdjecia);
                }
                db.SaveChanges();
                this.AddNotification("Produkt \"" + nazwaProduktu + "\" został pomyślnie usunięty!", NotificationType.SUCCESS);
            }
            var produktList = db.AspNetProducts.ToList();
            return View("ProduktList", produktList);
        }

    }
}