using Microsoft.AspNet.Identity;
using Pt.BL.AccountRepository;
using Pt.Ent.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Register(RegisterViewModel model )
        {
            if (!ModelState.IsValid)
                return View(model);

            var userManager = MembershipTools.NewUserManager();
            var checkUser = userManager.FindByName(model.UserName);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı zaten kayıtlı");
                return View(model);
            }
            return View();
        }
    }
}