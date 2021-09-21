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
        [StringLength(24, MinimumLength = 3)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Temat")]
        public string Subject { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Treść wiadomości")]
        public string Message { get; set; }

        public HttpPostedFile Attachment { get; set; }


    }
}