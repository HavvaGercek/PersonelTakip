using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Pt.BL.AccountRepository;
using Pt.BL.Settings;
using Pt.Ent.IdentityModel;
using Pt.Ent.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Pt.Web.MVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //kayıt olmadan önce kontrol ediliyor
            if (!ModelState.IsValid)
                return View(model);
            var userManager = MembershipTools.NewUserManager();
            var checkUser = userManager.FindByName(model.UserName);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı zaten kayıtlı!");
                return View(model);
            }

            checkUser = userManager.FindByEmail(model.Email);
            if(checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu eposta adersi kullanılmakta");
                return View(model);
            }
            
            // register işlemi yapılır
            var activationCode = Guid.NewGuid().ToString();
            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                SurName = model.SurName,
                Email = model.Email,
                UserName = model.UserName,
                ActivationCode = activationCode
            };

            var response = userManager.Create(user, model.Password);

            if (response.Succeeded)
            {
                string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    await SiteSettings.SendMail(new MailModel
                    {
                        To = user.Email,
                        Subject = "Hoşgeldin Sahip",
                        Message = "Sitemizi yöneteceğin için çok mutluyuz ^^"
                    });
                }
                else
                {
                    userManager.AddToRole(user.Id, "Passive");
                    await SiteSettings.SendMail(new MailModel
                    {
                        To = user.Email,
                        Subject = "Personel Yönetimi - Aktivasyon",
                        Message =
                            $"Merhaba {user.Name} {user.SurName} <br/>Hesabınızı aktifleştirmek için <a href='{siteUrl}/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a>"
                    });
                }

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt İşleminde bir hata oluştu");
                return View(model);
            }
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MembershipTools.NewUserManager();
            var user = await userManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "kullanıcı yok");
                return View(model);
            }
            var authManager = HttpContext.GetOwinContext().Authentication;
            var userIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignIn(new AuthenticationProperties
            {
                IsPersistent=model.RememberMe
            }, userIdentity);
            return RedirectToAction("Index","Home");
        }

        [Authorize]//signin yapılmış mı ona bakar yapılmışsa altındakini çalıştırır
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public async Task<ActionResult> Activation(string code)
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.ActivationCode == code);
            if (sonuc == null)
            {
                ViewBag.sonuc = "Aktivasyon işlemi başarısız";
                return View();
            }
            sonuc.EmailConfirmed = true;
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();

            userManager.RemoveFromRole(sonuc.Id, "Passive");
            userManager.AddToRole(sonuc.Id, "User");

            ViewBag.sonuc = $"Merhaba{sonuc.Name} {sonuc.SurName} kayıt aktivaston işleminiz başarılı olmuştur.";

            await SiteSettings.SendMail(new MailModel()
            {
                To = sonuc.Email,
                Message = ViewBag.sonuc.ToString(),
                Subject = "Aktivasyon",
                Bcc="havva.gercek@gmail.com"

            });
            return View();
        }

        public ActionResult RecoverPassword()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoverPassword(string email)
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.Email == email);
            if (sonuc == null)
            {
                ViewBag.sonuc = "email adresiniz kayıtlı değil";
                return View();
            }
            var randomPas = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            await userStore.SetPasswordHashAsync(sonuc, userManager.PasswordHasher.HashPassword(randomPas));
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();

            await SiteSettings.SendMail(new MailModel()
            {
                To = sonuc.Email,
                Subject = "Şifreniz değiştirildi",
                Message = $"Merhaba {sonuc.Name} {sonuc.SurName} yeni şifreniz:<b>{randomPas}</b> olmuştur"
            });
            ViewBag.sonuc = "Email adresinize yeni şifre gönderilmiştir";
            return View();

        }

        [Authorize]
        public ActionResult Profile()
        {
            var userManager = MembershipTools.NewUserManager();
            var user = userManager.FindById(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId());
            var model = new ProfileViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name=user.Name,
                SurName=user.SurName,
                UserName=user.UserName
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(model.Id);
            user.Name = model.Name;
            user.SurName = model.SurName;
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                if (HttpContext.User.IsInRole("Admin"))
                {
                    userManager.RemoveFromRole(user.Id, "Admin");
                }
                else if(HttpContext.User.IsInRole("User"))
                {
                    userManager.RemoveFromRole(user.Id, "User");
                }
                userManager.AddToRole(user.Id, "Passive");
                
                user.ActivationCode = Guid.NewGuid().ToString().Replace("-", "");
                string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
                                (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                await SiteSettings.SendMail(new MailModel()
                {
                    To = user.Email,
                    Subject = "Personel Yönetimi - Aktivasyon",
                    Message = $"Merhaba {user.Name} {user.SurName} email adresinizi değiştirdiğiniz için tekrar aktive etmelisiniz: <a href='{siteUrl}/Account/Activation?code={user.ActivationCode}'>Aktivasyon Kodu</a>",
                    
                });
            }
          
            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            var model1 = new ProfileViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                SurName = user.SurName,
                UserName = user.UserName
            };
            ViewBag.sonuc = "Bilgileriniz güncellenmiştir";
            return View(model1);

        }
    }
}