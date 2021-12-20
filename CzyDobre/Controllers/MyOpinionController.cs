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
    public class MyOpinionController : Controller
    {
        DBEntities db = new DBEntities();

        [Route("moje-opinie")]
        [Route("MyOpinion/MojaOpinia")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult MojaOpinia()
        {
            var produktList = db.AspNetRatings.ToList();
            return View(produktList);
        }

        [Route("usuwanie-mopjej-opinii")]
        [Route("MyOpinion/DeleteO")]
        [Authorize(Roles = "Admin")]
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
    }


}