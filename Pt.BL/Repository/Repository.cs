using Pt.Ent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.BL.Repository
{
    public class Repository
    {
        public class DepartmentRepo : RepositoryBase<Department, int> { }
        public class SalaryLogRepo : RepositoryBase<SalaryLog, int> { }
        public class LaborLogRepo : RepositoryBase<LaborLog, int> { }

    }
}
