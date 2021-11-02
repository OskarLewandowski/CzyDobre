using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        public Nullable<int> Id_Category { get; set; }

        [Required]
        [Display(Name = "Składniki")]
        public Nullable<int> Id_Ingredients { get; set; }

        [Required]
        [Display(Name = "Lokalizacja")]
        public Nullable<int> Id_Localization { get; set; }

        public List<HttpPostedFileBase> ProductImage { get; set; }
    }
}