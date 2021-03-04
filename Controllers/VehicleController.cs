using Exceptionless;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Controllers
{
    public class VehicleController : BaseController
    {
        public readonly IUserService _userService;
        public readonly IVehicleService _vehicleService;
        public readonly IReferenceService _referenceService;

        public VehicleController(IHttpContextAccessor httpContextAccessor,
             IUserService userService,
             IReferenceService referenceService,
             IVehicleService vehicleService) : base(httpContextAccessor, userService)
        {
            _vehicleService = vehicleService;
            _referenceService = referenceService;
        }

        [HttpGet("/{lang:lang}/Vehicle/{vin}")]
        public async Task<IActionResult> Index(string? vin = "")
        {
            var model = new VehicleViewModel();

            if (!string.IsNullOrEmpty(vin))
            {
                //get vehicle info by VIN
                model = await _vehicleService.GetVehicleByVIN(vin);
            }

            model.RootUrl = BaseRootUrl;
            model.OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId);
            // model.MaintenancePlanList = TODO

            return View(model);
        }

        [HttpPost("/{lang:lang}/Vehicle/Save")]
        public async Task<IActionResult> Create(VehicleViewModel vehicle)
        {
            try
            {
                var result = await _vehicleService.SaveVehicle(vehicle);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}
