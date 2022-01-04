﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CzyDobre.Models
{
    public class ProductFormModels
    {

        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int n { get; set; }



        [Required(ErrorMessage = "Nazwa Produktu jest wymagana!")]
        [StringLength(100)]
        [Display(Name = "Nazwa Produktu")]
        public string ProductName { get; set; }

        [StringLength(250)]
        [Display(Name = "Opis Produktu")]
        public string ProductDescription { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Kategoria jest wymagana!")]
        [Display(Name = "Kategoria")]
        public string CategoryName { get; set; }

        [StringLength(300)]
        [Required(ErrorMessage = "Lokalizacja jest wymagana!")]
        [Display(Name = "Lokalizacja")]
        public string LocName { get; set; }

        [Required]
        [ImageFileCheck]
        public List<HttpPostedFileBase> Icon { get; set; }

        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }




        public class ImageFileCheck : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                bool OK = false;
                int allSize = 0;
                var file = (ProductFormModels)validationContext.ObjectInstance;
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
}