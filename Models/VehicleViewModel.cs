using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class VehicleViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string VinCode { get; set; }
        public string Description { get; set; }
        public int Odometer { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string Propulsion { get; set; }
        public string BrakeSystem { get; set; }
        public string Steering { get; set; }
        public string Seating { get; set; }
        public int SelectedOil { get; set; }
        public IEnumerable<SelectListItem> OilList { get; set; }
        public int SelectedMaintenancePlan { get; set; }
        public IEnumerable<SelectListItem> MaintenancePlanList { get; set; }
        public string UnitNo { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public string SelectedUnit { get; set; }
        public string EntryDate { get; set; }
        public int MonthlyMileage { get; set; }

        public OwnerViewModel Owner { get; set; }
        public DriverViewModel Driver { get; set; }

        //For POST
        public string OwnerCompany { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }

        public string DriverName { get; set; }
        public string DriverCellPhone { get; set; }
        public string DriverPhone { get; set; }
        public string DriverEmail { get; set; }
        public string DriverAutorization { get; set; }
    }
}
