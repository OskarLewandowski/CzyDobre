using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CzyDobre.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Pole Adres e-mail jest wymagane")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole Nazwa użytkownika jest wymagane")]
        [Display(Name = "Nazwa użytkownika")]
        public string NickName { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Pamiętasz tę przeglądarkę?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Pole Adres e-mail jest wymagane")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required (ErrorMessage = "Pole Email jest wymagane")]
        [Display(Name = "E-mail")]
        [EmailAddress (ErrorMessage = "Wymagany jest poprawny adres Email")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Pole Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać Cię?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Name { get; set; }

        [Required (ErrorMessage = "Pole Nazwa użytkownika jest wymagane")]
        [Display(Name = "Nazwa użytkownika")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Pole E-mail jest wymagane")]
        [EmailAddress(ErrorMessage = "Wymagany jest poprawny adres Email")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole Hasło jest wymagane")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi zawierać co najmniej 6 znaków")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie nie są takie same - Hasła są niezgodne")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Pole Adres e-mail jest wymagane")]
        [EmailAddress(ErrorMessage = "Wymagany jest poprawny adres email")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie nie są takie same - Hasła są niezgodne")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Pole Adres e-mail jest wymagane")]
        [EmailAddress(ErrorMessage ="Wymagany jest poprawny adres Email")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
    }
}
