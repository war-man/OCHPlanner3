using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class MaintenancePlanManagementViewModel : BaseViewModel
    {
        public int SelectedGarageId { get; set; }

        public IEnumerable<MaintenancePlanViewModel> MaintenancePlanList { get; set; }
    }
}
