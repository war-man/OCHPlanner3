using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class RoleListViewModel : BaseViewModel
    {
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}
