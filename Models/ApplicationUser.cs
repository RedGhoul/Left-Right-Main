using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.DateCreated = DateTime.UtcNow;
        }
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
