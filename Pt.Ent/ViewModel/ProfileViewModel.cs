using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Ent.ViewModel
{
    public class ProfilePasswordViewModel
    {
        public ProfileViewModel profileModel { get; set; } = new ProfileViewModel();
        public PasswordViewModel passwordModel { get; set; } = new PasswordViewModel();
    }
    public class ProfileViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Ad")]
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(35)]
        [Required]
        [Display(Name = "Soyad")]
        public string SurName { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    
    }
    public class PasswordViewModel
    {

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 5 karakter olmalıdır!")]
        [Display(Name = "Eski Şifre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 5 karakter olmalıdır!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Şifre uyuşmuyor")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 5 karakter olmalıdır!")]
        public string NewPasswordConfirm { get; set; }
    }
}
