using Microsoft.AspNet.Identity.EntityFramework;
using Pt.Ent.IdentityModel;
using Pt.Ent.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.DL
{
    public class MyContext:IdentityDbContext<ApplicationUser>
    {
        public MyContext()
        :base("name=MyCon"){ }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<LaborLog> LaborLogs { get; set; }
        public virtual DbSet<SalaryLog> SalaryLogs { get; set; }
    }
}
