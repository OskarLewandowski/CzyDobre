using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CzyDobre.Models;
using reCAPTCHA.MVC;
using CzyDobre.Extensions;
using System.Collections.Generic;

namespace CzyDobre.Controllers
{
    public class SearchController : Controller
    {
        
        // GET: Search
        
        [HttpPost]
        public JsonResult GetCountries(string Prefix)
        {
            DBEntities db = new DBEntities();
            var Countries = (from c in db.AspNetProducts
                             where c.ProductName.StartsWith(Prefix)
                             select new { c.ProductName, c.Id_Product });
            return Json(Countries, JsonRequestBehavior.AllowGet);
        }
    }
}