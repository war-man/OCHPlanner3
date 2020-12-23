using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class AppointmentManagementViewModel : BaseViewModel
    {
        public IEnumerable<OptionViewModel> AppointmentList { get; set; }
        public GarageSelectorViewModel GarageSelector { get; set; }
    }
}
