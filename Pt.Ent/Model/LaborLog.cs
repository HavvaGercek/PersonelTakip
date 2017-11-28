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
    [Table("LaborLogs")]
    public class LaborLog:BaseModel
    {
        public DateTime StartShift { get; set; } = DateTime.Now;
        public DateTime? EndShift { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User{ get; set; }
    }
}
