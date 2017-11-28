using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Ent.ViewModel
{
    public class MailModel
    {
        public string To { get; set; }
        public List<String> ToList { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    
    }
}
