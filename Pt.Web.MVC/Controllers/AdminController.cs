using Microsoft.AspNet.Identity;
using Pt.BL.AccountRepository;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var roles = MembershipTools.NewRoleManager().Roles.ToList();
            var userManager = MembershipTools.NewUserManager();
            var users = userManager.Users.ToList().Select(x => new UsersViewModel
            {//users.tolist yaptık
                UserId = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Salary = x.Salary,
                Name = x.Name,
                Surname = x.SurName,
                RegisterDate = x.RegisterDate,
                RoleId = x.Roles.FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(y => y.Id == userManager.FindById(x.Id).Roles.FirstOrDefault().RoleId).Name//userManager.FindById ile önce userı buldurduk - bu sorun çoka çok ilişki olduğundan dolayı oluştu
            }).ToList();


            return View(users);
        }

        public ActionResult EditUser(string id)
        {
            if (id == null)
            { return RedirectToAction("Index"); }
            var userManager = MembershipTools.NewUserManager();
            var user = userManager.FindById(id);

            if (user == null)
            { return RedirectToAction("Index"); }

            var roles = MembershipTools.NewRoleManager().Roles.ToList();
            List<SelectListItem> roleList = new List<SelectListItem>();
            roles.ForEach(x => roleList.Add(new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.Id.ToString()

            }));

            ViewBag.roles = roleList;

            var model = new UsersViewModel()
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.SurName,
                RegisterDate = user.RegisterDate,
                Salary = user.Salary,
                RoleId = user.Roles.FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(y => y.Id == userManager.FindById(user.Id).Roles.FirstOrDefault().RoleId).Name
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UsersViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var roles = MembershipTools.NewRoleManager().Roles.ToList();
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(model.UserId);
            if (user == null) return View("Index");
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.SurName = model.Surname;
            user.Salary = model.Salary;
            user.Email = model.Email;
            if (model.RoleId != user.Roles.ToList().FirstOrDefault().RoleId)
            {
                var yenirol = roles.First(x => x.Id == model.RoleId).Name;
                userManager.AddToRole(model.UserId, yenirol);
                var eskirol = roles.First(x => x.Id == user.Roles.ToList().FirstOrDefault().RoleId).Name;
                userManager.RemoveFromRole(model.UserId, eskirol);
            }

            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("EditUser", new { id = model.UserId });
        }




    }
}

