using OCHPlanner3.Models;

namespace OCHPlanner3.Data.Models
{
    public class MaintenanceTypeProductGroupModel
    {
        public int Id { get; set; }
        public int MaintenanceTypeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
