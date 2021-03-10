using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string VinCode { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string Propulsion { get; set; }
        public string BrakeSystem { get; set; }
        public string Steering { get; set; }
        public string Color { get; set; }
        public string UnitNo { get; set; }
        public string LicencePlate { get; set; }
        public string Seating { get; set; }
        public int Odometer { get; set; }
        public string SelectedUnit { get; set; }
        public string EntryDate { get; set; }
        public int MonthlyMileage { get; set; }
        public int OilTypeId { get; set; }
        public int MaintenancePlanId { get; set; }
        public int VehicleOwnerId { get; set; }
        public int VehicleDriverId { get; set; }
        public string OwnerCompany { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string DriverCellphone { get; set; }
        public string DriverEmail { get; set; }
        public string DriverNotes { get; set; }

        public IEnumerable<VehicleProgramModel> VehicleProgram { get; set; }
    }
}
