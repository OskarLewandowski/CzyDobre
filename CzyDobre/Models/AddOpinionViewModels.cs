using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class AddOpinionViewModels
    {
        [Required(ErrorMessage = "Nazwa produktu jest wymagana")]
        [StringLength(128, MinimumLength = 4, ErrorMessage = "Nazwa produktu musi być ciągiem o długości minimalnej 4 znaków i maksymalnej 128 znaków")]
        [Display(Name = "Nazwa Produktu")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Opis produktu jest wymagany")]
        [StringLength(255, MinimumLength = 16, ErrorMessage = "Opis produktu musi być ciągiem o długości minimalnej 4 znaków i maksymalnej 128 znaków")]
        [Display(Name = "Opis Produktu")]
        public string ProductDescription { get; set; }

        [CheckIdCategory]
        [Display(Name = "Kategoria")]
        public Nullable<int> Id_Category { get; set; }

        [CheckIdIngredients]
        [Display(Name = "Składniki")]
        public Nullable<int> Id_Ingredients { get; set; }

        [CheckIdLocalization]
        [Display(Name = "Lokalizacja")]
        public Nullable<int> Id_Localization { get; set; }

        [Required]
        [Display(Name = "Zdjęcie Produktu")]
        [ImageFileCheck]
        public List<HttpPostedFileBase> Photo { get; set; }
    }


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

            if (file.Id_Category == null)
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

            if (file.Id_Category == null)
            {
                return new ValidationResult("Wymagana jest lokalizacja");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class ImageFileCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool OK = false;
            int allSize = 0;
            var file = (AddOpinionViewModels)validationContext.ObjectInstance;
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
}