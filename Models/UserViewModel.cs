using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCHPlanner3.Models
{
    public class UserViewModel
    {

        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName => $"{FirstName} {LastName}";
        public bool LockedOut { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Clients { get; set; }
        public string ClientsIdList { get; set; }

        public string Claims { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public int GarageId { get; set; }
        public string GarageName { get; set; }

        public string GarageToDisplay
        {
            get
            {
                if(Roles.Any(p => p == "SuperAdmin"))
                {
                    return "All";
                }

                return GarageName;
            }
        }
        public string RolesToDisplay
        {

            get
            {
                var builder = new StringBuilder();

                if (!Roles.Any()) return string.Empty;

                Roles.ToList().ForEach(c =>
                {
                    builder.Append(c);
                    builder.Append("<br />");
                });

                return builder.Remove(builder.Length - 6, 6).ToString();
            }
        }

    }
}
