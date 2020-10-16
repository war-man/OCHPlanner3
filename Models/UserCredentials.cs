using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OCHPlanner3.Models
{
    public class UserCredentials
    {
        public UserCredentials() { }

        public int GarageId { get; set; }
        public GarageViewModel GarageSetting { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
