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
        private readonly IReferenceService _referenceService;

        public GarageController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IReferenceService referenceService,
            IGarageService garageService) : base(httpContextAccessor, userService)
        {
            _garageService = garageService;
            _referenceService = referenceService;
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

        [HttpGet("/{lang:lang}/Garage/List")]
        public async Task<IActionResult> GetGarageList()
        {
            var model = new GarageListViewModel() { Garages = await _garageService.GetGarages() };
            return PartialView("_garages", model);
        }

        [HttpGet("/{lang:lang}/Garage/Create")]
        public async Task<IActionResult> Create()
        {
            var model = new GarageViewModel()
            {
                ActivationDate = DateTime.Now.ToString("yyyy-MM-dd"),
                BannerList = await _referenceService.GetBannerSelectListItem(),
                LanguageList = await _referenceService.GetLanguageSelectList(CurrentUser.GarageSetting.Language),
                DateFormatList = await _referenceService.GetDateFormatSelectList(),
                RootUrl = BaseRootUrl
            };

           return View(model);
        }

        [HttpPost("/{lang:lang}/Garage/Create")]
        public async Task<IActionResult> Create(GarageViewModel model)
        {
            try
            {
                var result = await _garageService.Create(model);
                return Ok(result);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpDelete("/{lang:lang}/Garage/Delete")]
        public async Task<IActionResult> Delete(int garageId)
        {
            try
            {
                var result = await _garageService.Delete(garageId);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return BadRequest();
        }

    }
}