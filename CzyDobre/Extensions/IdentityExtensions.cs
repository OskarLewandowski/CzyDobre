using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace CzyDobre.IdentityExtensions
{
   public static class IdentityExtensions
    {
        public static string GetAvatarURL(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AvatarURL");
            if (claim != null)
            {
                return "https://res.cloudinary.com/czydobre-pl/image/upload/v1640790462/CzyDobre-awatary/" + claim.Value;
            }
            else
            {
                return "https://res.cloudinary.com/czydobre-pl/image/upload/v1640786753/CzyDobre-www/awatar-domyslny-ramka_u9xv4t.png";
            }
        }
    }
}