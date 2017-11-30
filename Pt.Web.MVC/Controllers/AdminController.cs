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
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var roles = MembershipTools.NewRoleManager().Roles.ToList();
            var userManager = MembershipTools.NewUserManager();
            IEnumerable<UsersViewModel> users = userManager.Users.Select(x => new UsersViewModel
            {
                UserId = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Salary = x.Salary,
                Name = x.Name,
                Surname = x.SurName,
                RegisterDate = x.RegisterDate,
                RoleId = x.Roles.FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(y=>y.Id==x.Roles.FirstOrDefault().RoleId).Name
            }).ToList();
           
            List<SelectListItem> roleList = new List<SelectListItem>();
            roles.ForEach(x => new SelectListItem(){
                Text = x.Name,
                Value = x.Id
            });
            ViewBag.roles = roleList;
            return View(users);
        }
    }
}