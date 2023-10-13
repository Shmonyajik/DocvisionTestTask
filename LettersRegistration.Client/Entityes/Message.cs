using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LettersRegistration.Client
{
    public class Message
    {
        public int Id {get;set;}
        public string Name { get; set; } 
        public DateTime RegistrationTime { get; set; }
        public string Sender { get; set; }

        public string Addressee { get; set; }
        public string Content { get; set; }
    }
}
