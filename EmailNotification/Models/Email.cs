using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailNotification.Models
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
       
    }
}
