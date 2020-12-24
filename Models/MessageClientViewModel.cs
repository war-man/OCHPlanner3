using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class MessageClientViewModel : BaseViewModel
    {
        public string SelectedUnit { get; set; }
        public string UnitValue { get; set; }
        public string Comment { get; set; }

        public IEnumerable<SelectListItem> MileageList { get; set; }


        public int SelectedVerificationId { get; set; }
        public IEnumerable<SelectListItem> VerificationList { get; set; }
        public int SelectedMileageChoice1 { get; set; }

        public int SelectedMaintenanceId { get; set; }
        public IEnumerable<SelectListItem> MaintenanceList { get; set; }
        public int SelectedMileageChoice2 { get; set; }


    }
}
