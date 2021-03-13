using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class PreventiveMaintenanceController : BaseController
    {
        public readonly IReferenceService _referenceService;
        public readonly IProgramService _programService;
        public readonly IVINQueryService _vinQueryService;
        public readonly IVehicleService _vehicleService;

        public PreventiveMaintenanceController(IHttpContextAccessor httpContextAccessor,
            IVINQueryService vinQueryService,
            IReferenceService referenceService,
            IProgramService programService,
            IVehicleService vehicleService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _referenceService = referenceService;
            _vinQueryService = vinQueryService;
            _programService = programService;
            _vehicleService = vehicleService;
        }

        [Route("/{lang:lang}/PreventiveMaintenance/{vin?}")]
        public async Task<IActionResult> Index(string? vin)
        {
            try
            {
                var model = new PreventiveMaintenanceViewModel()
                {
                    Vehicle = new VehicleViewModel()
                    {
                        OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId),
                        MaintenancePlanList = new List<SelectListItem>(),
                        Owner = new OwnerViewModel() { IsReadOnly = true },
                        Driver = new DriverViewModel() { IsReadOnly = true },
                        Programs = await _programService.GetPrograms(CurrentUser.GarageId)
                    },
                    RootUrl = BaseRootUrl
                };

                if(!string.IsNullOrWhiteSpace(vin))
                {
                    model.Vehicle = await _vehicleService.GetVehicleByVIN(vin, CurrentUser.GarageId);
                }

                if(model.Vehicle.Owner != null)
                {
                    model.Vehicle.Owner.IsReadOnly = true;
                }

                if (model.Vehicle.Driver != null)
                {
                    model.Vehicle.Driver.IsReadOnly = true;
                }

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