using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class MaintenanceTypeManagementViewModel : BaseViewModel
    {
        public int SelectedGarageId { get; set; }

        public IEnumerable<MaintenanceTypeViewModel> MaintenanceTypeList { get; set; }
    }
}
