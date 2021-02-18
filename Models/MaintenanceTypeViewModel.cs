using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class MaintenanceTypeManagementViewModel : BaseViewModel
    {
        public int SelectedGarageId { get; set; }

        public IEnumerable<MaintenanceTypeViewModel> MaintenanceTypeList { get; set; }
    }

    public class MaintenanceTypeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public IList<MaintenanceTypeProductGroup> products { get; set; }

        public string Material { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal MaterialRetail { get; set; }

        public decimal WorkTime { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal WorkCost { get; set; }
        public decimal WorkTotal { get; set; }

        public decimal MaintenanceTotalCost { get; set; }
        public decimal MaintenanceTotalRetail { get; set; }
        public decimal MaintenanceTotalPrice { get; set; }

        public decimal ProfitPercentage { get; set; }
        public decimal ProfitAmount { get; set; }

        public int SelectedProduct { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }

        public int GarageId { get; set; }
    }
}
