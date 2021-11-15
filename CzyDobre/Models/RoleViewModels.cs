using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class RoleViewModels
    {
        [StringLength(24, MinimumLength = 1, ErrorMessage = "Pole jest wymagane! Minimalna długość 1 znak, a maksymalna 24 znaki")]
        [Required(ErrorMessage = "Pole jest wymagane!")]
        public string RoleName { get; set; }
    }
}