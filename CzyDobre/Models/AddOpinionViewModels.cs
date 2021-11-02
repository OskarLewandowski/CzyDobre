using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class AddOpinionViewModels
    {
        public List<HttpPostedFileBase> Photo { get; set; }
    }
}