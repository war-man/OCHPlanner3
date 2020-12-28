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
        public string UnitValueChoice2 { get; set; }

        public int SelectedAppointmentId { get; set; }
        public IEnumerable<SelectListItem> AppointmentList { get; set; }
        public int SelectedMileageChoice3 { get; set; }

        public IEnumerable<SelectListItem> CarMakeList { get; set; }
        public string SelectedCarMake { get; set; }

        public IEnumerable<SelectListItem> CarModelList { get; set; }
        public string SelectedCarModel { get; set; }

        public IEnumerable<SelectListItem> CarColorList { get; set; }
        public string SelectedCarColor { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        public string Note { get; set; }
    }
}
