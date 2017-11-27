using Pt.Ent.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Ent.Model
{
    [Table("Departments")]
    public class Department:BaseModel
    {
        [Required]
        [StringLength(55,ErrorMessage ="Bu alan zorunludur",MinimumLength =5)]
        [Index(IsUnique = true)]
        public string  DepartmentName { get; set; }

        public virtual List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
