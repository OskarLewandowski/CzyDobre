using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class ContactUsViewModels
    {
        [Required(ErrorMessage = "Pole Nazwa jest wymagane")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Maksymalna dozwolona ilość znaków 32")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Pole Email jest wymagane")]
        [EmailAddress (ErrorMessage = "Wymagany jest poprawny adres Email")]
        [StringLength(64)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Pole Temat jest wymagane")]
        [StringLength(32 ,ErrorMessage = "Maksymalna dozwolona ilość znaków to 32")]
        [Display(Name = "Temat")]
        public string Subject { get; set; }

        [Required (ErrorMessage = "Pole Treść wiadomości jest wymagane")]
        [StringLength(255 , ErrorMessage ="Maksymalna dozwolona ilość znaków to 255")]
        [Display(Name = "Treść wiadomości")]
        public string Message { get; set; }

        [AttachmentCheck]
        public List<HttpPostedFileBase> Attachment { get; set; }
    }

    public class AttachmentCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool OK = false;
            int allSize = 0;
            var file = (ContactUsViewModels)validationContext.ObjectInstance;

            foreach (HttpPostedFileBase item in file.Attachment)
            {
                if (item != null && item.ContentLength > 0)
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
                        return new ValidationResult("Pliki ważą za dużo! Maksymalna wartość załącznika wynosi 5MB");
                    }
                }
            }

            if (OK == true)
            {
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}