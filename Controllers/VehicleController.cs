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
        public readonly IProgramService _programService;

        public VehicleController(IHttpContextAccessor httpContextAccessor,
             IUserService userService,
             IReferenceService referenceService,
             IProgramService programService,
             IVINQueryService vinQueryService,
             IVehicleService vehicleService) : base(httpContextAccessor, userService)
        {
            _vehicleService = vehicleService;
            _referenceService = referenceService;
            _vinQueryService = vinQueryService;
            _programService = programService;
        }

        [HttpGet("/{lang:lang}/Vehicle/{vin}")]
        public async Task<IActionResult> Index(string? vin = "")
        {
            var model = new VehicleViewModel();

            if (!string.IsNullOrEmpty(vin))
            {
                //get vehicle info by VIN
                model = await _vehicleService.GetVehicleByVIN(vin, CurrentUser.GarageId);
            }

            model.RootUrl = BaseRootUrl;
            model.OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId);
            if(model.Programs == null)
            {
                model.Programs = await _programService.GetPrograms(CurrentUser.GarageId);
            }
            // model.MaintenancePlanList = TODO

            if (model.Owner == null)
            {
                model.Owner = new OwnerViewModel() { IsReadOnly = false };
            }

            if (model.Driver == null)
            {
                model.Driver = new DriverViewModel() { IsReadOnly = false };
            }


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
            var vehicle = await _vehicleService.GetVehicleByVIN(vin, CurrentUser.GarageId);

            //Get oilType
            if (vehicle.OilTypeId != 0)
            {
                var oillist = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId);
                var item = oillist.FirstOrDefault(p => p.Value == vehicle.OilTypeId.ToString());
                vehicle.SelectedOilDisplay = item != null ? item.Text : string.Empty;
            }

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

        [HttpGet("/{lang:lang}/Vehicle/Programs")]
        public async Task<IActionResult> GetProgramList(int vehicleId, bool displayOnlySelected = false)
        {
            try
            {
                var model = await _vehicleService.GetVehiclePrograms(vehicleId, CurrentUser.GarageId);
                if(displayOnlySelected)
                {
                    return PartialView("_programSelected", model);
                }
                else
                {
                    return PartialView("_programs", model);
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}
