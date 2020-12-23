using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class MaintenanceManagementViewModel : BaseViewModel
    {
        public IEnumerable<OptionViewModel> MaintenanceList { get; set; }
        public GarageSelectorViewModel GarageSelector { get; set; }
    }
}
