using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IMaintenancePlanFactory
    {
        Task<IEnumerable<MaintenancePlanModel>> GetMaintenancePlans(int garageId);
    }
}
