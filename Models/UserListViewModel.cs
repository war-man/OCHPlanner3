using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class UserListViewModel : BaseViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public GarageSelectorViewModel GarageSelector { get; set; }
        public int RemainingUsers { get; set; }
    }
}
