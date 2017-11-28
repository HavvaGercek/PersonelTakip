using Microsoft.AspNet.Identity;
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
        [ValidateAntiForgeryToken]//form post yaparken (cfrs) sessionda bir kod(token) üretiyor ve işlemleri yaparken kod ile sessiondan kontrol ediyor.
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //kayıt olmadan önce kontrol ediliyor
            if (!ModelState.IsValid)
                return View(model);

            var userManager = MembershipTools.NewUserManager();
            var checkUser = userManager.FindByName(model.UserName);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı zaten kayıtlı");
                return View(model);
            }

            //register işlemi yapılır
            var activationCode = Guid.NewGuid().ToString();
            ApplicationUser user = new ApplicationUser() {
                Name=model.Name,
                SurName=model.SurName,
                UserName=model.UserName,
                Email=model.Email,
                ActivationCode=activationCode
            };
            var response = userManager.Create(user, model.Password);
            if (response.Succeeded)
            {
                //heryerde çalışabilecek bir url oluşturuyoruz
                string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    await SiteSettings.SendMail(new MailModel {
                        To=user.Email,
                        Subject="Welcome",
                        Message="Admin'e mesajjj",
                    });
                }
                else
                {
                    userManager.AddToRole(user.Id, "Passive");
                    await SiteSettings.SendMail(new MailModel
                    {
                        To = user.Email,
                        Subject = "Personel Yönetimi Aktivasyon",
                        Message = $"Merhaba {user.Name} {user.SurName}<br/> Sistemi kullanabilmeniz için <a href='{siteUrl}/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a>"
                    });
                }
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt işleminde bir hata oluştu");
                return View(model);
            }

            
        }
    }
}