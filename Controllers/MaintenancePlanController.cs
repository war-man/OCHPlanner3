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

        public MaintenancePlanController(IHttpContextAccessor httpContextAccessor,
            IReferenceService referenceService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _referenceService = referenceService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new MaintenancePlanViewModel()
                {
                    OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId),
                    MaintenancePlanList = new List<SelectListItem>(),
                    Owner = new OwnerViewModel(),
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
    }
}