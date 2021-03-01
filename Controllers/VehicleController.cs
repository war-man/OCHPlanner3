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

        public VehicleController(IHttpContextAccessor httpContextAccessor,
             IUserService userService,
             IVehicleService vehicleService) : base(httpContextAccessor, userService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("/{lang:lang}/Vehicle/{vin}")]
        public IActionResult Index(string? vin = "")
        {
            if(!string.IsNullOrEmpty(vin))
            {
                //get vehicle info by VIN
                var vehicle = _vehicleService.GetVehicleByVIN(vin);
            }

            var model = new VehicleViewModel()
            {
                RootUrl = BaseRootUrl
            };

            return View(model);
        }
    }
}
