using Mapster;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class MaintenancePlanService : IMaintenancePlanService
    {
        private readonly IMaintenancePlanFactory _maintenancePlanFactory;

        public MaintenancePlanService(IMaintenancePlanFactory maintenancePlanFactory,
            IOptionFactory optionFactory)
        {
            _maintenancePlanFactory = maintenancePlanFactory;
        }

        public async Task<IEnumerable<MaintenancePlanViewModel>> GetMaintenancePlans(int garageId)
        {
            var result = await _maintenancePlanFactory.GetMaintenancePlans(garageId);
            return result.Adapt<IEnumerable<MaintenancePlanViewModel>>();
        }
    }
}
