using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class VerificationManagementViewModel : BaseViewModel
    {
        public IEnumerable<OptionViewModel> VerificationList { get; set; }
        public GarageSelectorViewModel GarageSelector { get; set; }
    }
}
