using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProduktController : Controller
    {
        DBEntities db = new DBEntities();

        // GET: Produkt
        public ActionResult ProduktList()
        {
            var produktList = db.AspNetProducts.ToList();
            return View(produktList);
        }
    }
}