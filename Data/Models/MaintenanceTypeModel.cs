using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Models
{
    public class MaintenanceTypeModel
    {


        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductString { get; set; }
        public string Material { get; set; }
        public string MaterialCost { get; set; }
        public string MaterialRetail { get; set; }
        public string ProductCost { get; set; }
        public string ProductRetail { get; set; }
        public string WorkTime { get; set; }
        public string HourlyRateCost { get; set; }
        public string HourlyRateBillable { get; set; }
        public string WorkCost { get; set; }
        public string WorkTotal { get; set; }
        public string MaintenanceTotalCost { get; set; }
        public string MaintenanceTotalRetail { get; set; }
        public string MaintenanceTotalPrice { get; set; }
        public string ProfitPercentage { get; set; }
        public string ProfitAmount { get; set; }
        public int SelectedProduct { get; set; }
        public int GarageId { get; set; }
    }
}
