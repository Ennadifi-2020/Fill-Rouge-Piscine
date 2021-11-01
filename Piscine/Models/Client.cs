using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piscine.Models
{
    public class Client: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    
        public string Adress{ get; set; }
     
    }
}
