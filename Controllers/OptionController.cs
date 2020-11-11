using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class OptionController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IOptionService _optionService;

        public OptionController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IOptionService optionService) : base(httpContextAccessor, userService)
        {
            _optionService = optionService;
        }

        [Route("/{lang:lang}/Options/Printer")]
        public async Task<IActionResult> Printer()
        {
            //Get printer Configuration
            var printerConfiguration = await _optionService.GetPrinterConfiguration(CurrentUser.GarageId);

            var model = printerConfiguration;
            model.RootUrl = BaseRootUrl;

            return View(model);
        }

        [HttpPost("/{lang:lang}/Options/Printer/Save")]
        public async Task<int> SavePrinter(PrinterConfigurationViewModel printerConfig)
        {
            return await _optionService.SavePrinterConfiguration(printerConfig, base.CurrentUser.GarageId);
        }
    }
}
