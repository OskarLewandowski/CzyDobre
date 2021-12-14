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
    }
}