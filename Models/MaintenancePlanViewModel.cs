using System;

namespace OCHPlanner3.Models
{
    public class MaintenancePlanViewModel : BaseViewModel
    {
        public int LastServiceMileage { get; set; }
        public DateTime LastServiceDate { get; set; }
        public int MonthlyMileage { get; set; }
        public DateTime EntryDate { get; set; }
       
        public VehicleViewModel Vehicle { get; set; }
        public OwnerViewModel Owner { get; set; }
        public DriverViewModel Driver { get; set; }
    }
}
