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
        
        public List<HttpPostedFileBase> Photo { get; set; }
        
    }
    
    public class ImageFilesCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool OK = false;
            int allSize = 0;
            var file = (EditOpinionViewModels)validationContext.ObjectInstance;
            string ext = null;

            foreach (HttpPostedFileBase item in file.Photo)
            {
                if (item != null && item.ContentLength > 0)
                {
                    ext = Path.GetExtension(item.FileName.ToLower());

                    if (ext == ".png" || ext == ".jpeg" || ext == ".jpg")
                    {
                        var byteCount = item.ContentLength;

                        allSize = allSize + byteCount;

                        if (allSize < 5242880)
                        {
                            OK = true;
                        }
                        else
                        {
                            OK = false;
                            return new ValidationResult("Zdjęcia ważą za dużo! Maksymalna wartość zdjęć wynosi 5MB");
                        }
                    }
                    else
                    {
                        return new ValidationResult("Dozwolony format zdjęć to: .png, .jpg, .jpeg ");
                    }
                }
                else
                {
                    return new ValidationResult("Wymagane jest zdjęcie");
                }
            }

            if (OK == true)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Wymagane jest zdjecie, o rozszerzeniu .png, .jpg, .jpeg");
        }
        
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