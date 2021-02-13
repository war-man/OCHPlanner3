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
        
        public VehicleController(IHttpContextAccessor httpContextAccessor,
             IUserService userService) : base(httpContextAccessor, userService)
        {
            
        }

        public IActionResult Index()
        {
            var model = new VehicleViewModel()
            {
                RootUrl = BaseRootUrl
            };

            return View(model);
        }
    }
}
