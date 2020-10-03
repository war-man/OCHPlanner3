using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OCHPlanner3.Models
{
    public class UserCredentials
    {
        public UserCredentials() { }

        //public UserCredentials(string userName, string language)
        //{
        //    UserName = userName;
        //    Language = language;
        //}

        public int GarageId { get; set; }
        //public string UserId { get; set; }
        //public string UserName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string UserRoleForDisplay { get; set; }
        //public string Language { get; set; }
        //public string Email { get; set; }
       
        public IEnumerable<Claim> Claims { get; set; }

        //public string FullName
        //{
        //    get
        //    {
        //        return $"{FirstName} {LastName}";
        //    }
        //}
    }
}
