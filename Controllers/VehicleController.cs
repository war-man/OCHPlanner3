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
        public readonly IVINQueryService _vinQueryService;

        public VehicleController(IHttpContextAccessor httpContextAccessor,
             IUserService userService,
             IReferenceService referenceService,
             IVINQueryService vinQueryService,
             IVehicleService vehicleService) : base(httpContextAccessor, userService)
        {
            _vehicleService = vehicleService;
            _referenceService = referenceService;
            _vinQueryService = vinQueryService;
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

        public async Task<VehicleViewModel> GetVehicleByVIN(string vin)
        {
            //Get vehicle from datatabse
            var vehicle = await _vehicleService.GetVehicleByVIN(vin);

            //If no vehicle in DB, Get it from VIN Decode
            if (vehicle == null)
            {
                var vinResult = await _vinQueryService.GetVINDecode(vin);

                if (vinResult != null && !string.IsNullOrWhiteSpace(vinResult.VIN))
                {
                    //Increment VINDecode counter
                    //var _garageService = new GarageService();
                    //_garageService.IncrementVINDecodeCounter(UserManagerHelper.GetCurrentUser().GarageID);
                }

                vehicle = new VehicleViewModel();
            }
            return vehicle;
        }
    }
}
