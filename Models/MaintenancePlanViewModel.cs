using System;

namespace OCHPlanner3.Models
{
    public class MaintenancePlanViewModel : BaseViewModel
    {
        public int LastServiceMileage { get; set; }
        public DateTime LastServiceDate { get; set; }
       
        public VehicleViewModel Vehicle { get; set; }
       
    }
}
