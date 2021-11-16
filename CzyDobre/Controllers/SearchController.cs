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
        DBEntities db = new DBEntities();
        // GET: Search
        public JsonResult GetCustomers(string term)

        {
            
            List<string> Customers = db.AspNetProducts.Where(s => s.ProductName.StartsWith(term))

                .Select(x => x.ProductName).ToList();

            return Json(Customers, JsonRequestBehavior.AllowGet);
        }
    }
}