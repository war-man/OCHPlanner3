using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IMaintenanceTypeFactory
    {
        Task<int> CreateMaintenanceType(MaintenanceTypeModel maintenanceType, IEnumerable<MaintenanceTypeProductGroupViewModel> products);
        Task<IEnumerable<MaintenanceTypeModel>> GetMaintenanceTypes(int garageId);
        Task<int> Delete(int id);
    }
}
