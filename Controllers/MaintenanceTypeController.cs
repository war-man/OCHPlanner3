using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class MaintenanceTypeController : BaseController
    {
        public readonly IOptionService _optionService;

        public MaintenanceTypeController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IOptionService optionService) : base(httpContextAccessor, userService)
        {
            _optionService = optionService;
        }

        [Route("/{lang:lang}/MaintenanceType/{id}")]
        public IActionResult Index(int id)
        {
            try
            {
                var model = new MaintenanceTypeManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    SelectedGarageId = id,
                    MaintenanceTypeList = new List<MaintenanceTypeViewModel>()
                };


                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Route("/{lang:lang}/MaintenanceType/Create/{id}")]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var model = new MaintenanceTypeViewModel()
                {
                    RootUrl = BaseRootUrl,
                    GarageId = id,
                    ProductList = await _optionService.GetProductSelectListItem(id)
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
