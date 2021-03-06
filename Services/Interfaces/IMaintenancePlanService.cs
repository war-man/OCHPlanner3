using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IMaintenancePlanService
    {
        Task<IEnumerable<MaintenancePlanViewModel>> GetMaintenancePlans(int id);
    }
}
