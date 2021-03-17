using Exceptionless;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OCHPlanner3.Controllers
{
    public class MaintenancePlanController : BaseController
    {
        public readonly IMaintenancePlanService _maintenancePlanService;

        public MaintenancePlanController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IMaintenancePlanService maintenancePlanService,
            IOptionService optionService) : base(httpContextAccessor, userService)
        {
            _maintenancePlanService = maintenancePlanService;
        }

        [Route("/{lang:lang}/MaintenancePlan/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var model = new MaintenancePlanManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    SelectedGarageId = id,
                    MaintenancePlanList = await _maintenancePlanService.GetMaintenancePlans(id)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Route("/{lang:lang}/MaintenancePlan/Create/{id}")]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var model = new MaintenancePlanViewModel()
                {
                    RootUrl = BaseRootUrl,
                    GarageId = id,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}
