using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Ent.ViewModel
{
    public class UsersViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public decimal Salary { get; set; }

    }
}
