using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class EditOpinionViewModels
    {
        [StringLength(100)]
        
        [Display(Name = "Produkt")]
        public string  PName { get; set; }

        
        [Display(Name = "Wygląd")]
        public short RateComposition { get; set; }

        
        [Display(Name = "Cena")]
        public short RateIngredients { get; set; }

        
        [Display(Name = "Smak")]
        public short RateTaste { get; set; }
        
        [Display(Name = "Obsługa")]
        public short RateService { get; set; }

        [StringLength(250)]
        [Display(Name = "Recenzja")]
        public string Review { get; set; }

        
        [Display(Name = "Zdjęcie Produktu")]
        
        public List<string> Photo { get; set; }
        
    }
    
    
        
    
   



    /*
    public class CheckIdCategory : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (AddOpinionViewModels)validationContext.ObjectInstance;

            if(file.Id_Category == null)
            {
                return new ValidationResult("Wymagana jest kategoria");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
    public class CheckIdIngredients : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (AddOpinionViewModels)validationContext.ObjectInstance;

            if (file.Id_Ingredients == null)
            {
                return new ValidationResult("Wymagane są składniki");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class CheckIdLocalization : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (AddOpinionViewModels)validationContext.ObjectInstance;

            if (file.Id_Localization == null)
            {
                return new ValidationResult("Wymagana jest lokalizacja");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
    */


}