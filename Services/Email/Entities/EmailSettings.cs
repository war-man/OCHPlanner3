using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Email.Entities
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DebugEmail { get; set; }
    }
}
