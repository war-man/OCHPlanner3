using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class MaintenancePlanViewModel : BaseViewModel
    {
        public string VinCode { get; set; }
        public string Description { get; set; }
        public int LastServiceMileage { get; set; }
        public DateTime LastServiceDate { get; set; }
        public int MonthlyMileage { get; set; }
        public DateTime EntryDate { get; set; }
        public int SelectedOil { get; set; }
        public IEnumerable<SelectListItem> OilList { get; set; }
        public int SelectedMaintenancePlan { get; set; }
        public IEnumerable<SelectListItem> MaintenancePlanList { get; set; }
        public string UnitNo { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public int Odometer { get; set; }
        public OwnerViewModel Owner { get; set; }
        public DriverViewModel Driver { get; set; }
    }
}
