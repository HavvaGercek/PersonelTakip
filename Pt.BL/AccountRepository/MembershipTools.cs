using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Pt.DL;
using Pt.Ent.IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.BL.AccountRepository
{
    public class MembershipTools
    {
        public static UserStore<ApplicationUser> NewUserStore()//kullanıcı ekle sil güncelle işlemleri
            =>new UserStore<ApplicationUser>(new MyContext());
        public static UserManager<ApplicationUser> NewUserManager()
          => new UserManager<ApplicationUser>(NewUserStore());

        public static RoleStore<ApplicationRole> NewRoleStore()
         => new RoleStore<ApplicationRole>(new MyContext());
        public static RoleManager<ApplicationRole> NewRoleManager()
          => new RoleManager<ApplicationRole>(NewRoleStore());
    }
}
