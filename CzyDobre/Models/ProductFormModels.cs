using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzyDobre.Models
{
    public class ProductFormModels
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa Produktu")]
        public string ProductName { get; set; }

        [StringLength(250)]
        [Display(Name = "Opis Produktu")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Kategoria")]
        public string CategoryName { get; set; }


        [Required]
        [Display(Name = "Lokalizacja")]
        public string LocName { get; set; }

       
        public HttpPostedFile Icon { get; set; }

        
        public List<HttpPostedFileBase> Image { get; set; }
    }
}