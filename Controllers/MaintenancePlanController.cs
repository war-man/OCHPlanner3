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
    public class MaintenancePlanController : BaseController
    {
        public readonly IReferenceService _referenceService;
        public readonly IVINQueryService _vinQueryService;

        public MaintenancePlanController(IHttpContextAccessor httpContextAccessor,
            IVINQueryService vinQueryService,
            IReferenceService referenceService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _referenceService = referenceService;
            _vinQueryService = vinQueryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new MaintenancePlanViewModel()
                {
                    Vehicle = new VehicleViewModel()
                    {
                        OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId),
                        MaintenancePlanList = new List<SelectListItem>(),
                        Owner = new OwnerViewModel(),
                        Driver = new DriverViewModel()
                    },
                    RootUrl = BaseRootUrl
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        public async Task<JsonResult> VINDecode(string vin)
        {
            var vinResult = await _vinQueryService.GetVINDecode(vin);

            if (vinResult != null && !string.IsNullOrWhiteSpace(vinResult.VIN))
            {
                //Increment VINDecode counter
                //var _garageService = new GarageService();
                //_garageService.IncrementVINDecodeCounter(UserManagerHelper.GetCurrentUser().GarageID);
            }

            return Json(vinResult);
        }
    }
}