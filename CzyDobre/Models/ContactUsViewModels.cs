using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class ContactUsViewModels
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(32)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        [Display(Name = "Temat")]
        public string Subject { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Treść wiadomości")]
        public string Message { get; set; }

        public HttpPostedFileBase Attachment { get; set; }


    }
}