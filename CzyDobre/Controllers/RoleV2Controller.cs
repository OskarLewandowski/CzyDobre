using CzyDobre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleV2Controller : Controller
    {
        DBEntities db = new DBEntities();

        [Route("lista-roli")]
        [Route("RoleV2/RoleList")]
        [Authorize(Roles = "Admin")]
        public ActionResult RoleList()
        {
            var userList = db.AspNetUsers.ToList();
            return View(userList);
        }
    }
}