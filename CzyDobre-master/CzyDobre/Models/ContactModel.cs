using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class ContactModel
    {
        [Required, Display(Name = "Twój Nick:")]
        public string SenderName { get; set; }
        [Required, Display(Name = "Twój Email:"), EmailAddress]
        public string SenderEmail { get; set; }
        [Required, Display(Name = "Treść wiadomości:")]
        public string Message { get; set; }
    }
}