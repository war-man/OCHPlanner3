using System;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public string CostPrice { get; set; }
        public string RetailPrice { get; set; }
        public int GarageId { get; set; }
    }

    public class MaintenanceTypeProductGroup
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
