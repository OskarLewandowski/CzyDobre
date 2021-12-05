using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class GetDishViewModels
    {
        [StringLength(100)]
        
        [Display(Name = "Produkt")]
        public string ProductName { get; set; }

       
        [Display(Name = "Cena")]
        public short RateIngredients { get; set; }

        
        [Display(Name = "Smak")]
        public short RateTaste { get; set; }
        
        [Display(Name = "Obsługa")]
        public short RateService { get; set; }

        
        [Display(Name = "Recenzja")]
        public string Review { get; set; }

        
    }

    

}