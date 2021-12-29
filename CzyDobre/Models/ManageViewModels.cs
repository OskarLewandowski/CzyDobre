using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CzyDobre.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej następującą liczbę znaków: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Nowe hasło i potwierdzenia hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Bieżące hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej następującą liczbę znaków: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Nowe hasło i potwierdzenia hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }

    public class DeleteAccountViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Bieżące hasło")]
        public string Password { get; set; }
    }

    public class MyDataViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Pole Nick jest wymagane, nie może być puste")]
        [MyDataNickNameCheck]
        [Display(Name = "Nick")]
        public string NickName { get; set; }
    }

    public class AvatarViewModel
    {
        public string AvatarUrl { get; set; }
        [AvatarCheckFile]
        public List<HttpPostedFileBase> avatarAttachment { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Numer telefonu")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class MyDataNickNameCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DBEntities db = new DBEntities();

            var userMyData = (MyDataViewModel)validationContext.ObjectInstance;
            string nickName = userMyData.NickName;
            string userId = userMyData.Id;
            string checkUser = db.AspNetUsers.Where(u => u.NickName == nickName).Select(u => u.Id).FirstOrDefault();
            string checkCurrentNick = db.AspNetUsers.Where(u => u.Id == userId).Select(u => u.NickName).FirstOrDefault();

            if(nickName == checkCurrentNick)
            {
                return ValidationResult.Success;
            }

            if (checkUser == "" || checkUser == null)
            {
                return ValidationResult.Success;
            }

            if (checkUser != "" || checkUser != null)
            {
                return new ValidationResult("Ten Nick jest już zajęty");
            }

            return ValidationResult.Success;
        }
    }

    public class AvatarCheckFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool OK = false;
            int allSize = 0;
            var file = (AvatarViewModel)validationContext.ObjectInstance;
            string ext = null;

            foreach (HttpPostedFileBase item in file.avatarAttachment)
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