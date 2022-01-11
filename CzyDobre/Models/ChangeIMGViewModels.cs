using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class ChangeIMGViewModels
    {
        
        


        [Required(ErrorMessage = "Zdjecie jest wymagane!")]
        [Display(Name = "Zdjęcie Produktu")]
        [ImageFilesCheck]
        public List<HttpPostedFileBase> Icon { get; set; }
    }
    public class ImageFilesCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool OK = false;
            int allSize = 0;
            var file = (ChangeIMGViewModels)validationContext.ObjectInstance;
            string ext = null;

            foreach (HttpPostedFileBase item in file.Icon)
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
