using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IMaintenanceTypeService
    {
        Task<int> CreateMaintenanceType(MaintenanceTypeViewModel maintenanceType);
        Task<IEnumerable<MaintenanceTypeViewModel>> GetMaintenanceTypes(int garageId);
        Task<int> Delete(int id);
    }
}
