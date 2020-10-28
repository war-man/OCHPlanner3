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

namespace OCHPlanner3.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class GarageController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IGarageService _garageService;

        public GarageController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IGarageService garageService) : base(httpContextAccessor, userService)
        {
            _garageService = garageService;
        }

        [Route("/{lang:lang}/Garages")]
        public async Task<IActionResult> Index()
        {
            var model = new GarageListViewModel()
            {
                RootUrl = BaseRootUrl,
                Garages = await _garageService.GetGarages()
            };

            return View(model);
        }

        [Route("/{lang:lang}/Garage/Create")]
        public async Task<IActionResult> Create()
        {
           return View(new GarageViewModel());
        }
    }
}