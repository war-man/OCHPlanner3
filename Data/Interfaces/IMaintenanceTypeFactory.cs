using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IMaintenanceTypeFactory
    {
        Task<int> CreateMaintenanceType(MaintenanceTypeViewModel maintenanceType, IEnumerable<MaintenanceTypeProductGroup> products);
    }
}
