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

        [Route("/{lang:lang}/Garage/Oil")]
        public async Task<IActionResult> OilManagement()
        {
            var model = new OilManagementViewModel()
            {
                RootUrl = BaseRootUrl,
                OilList = await _garageService.GetOilList(CurrentUser.GarageId),
                GarageSelector = new GarageSelectorViewModel
                {
                    Garages = await _garageService.GetGaragesSelectList(),
                    SelectedGarageId = HttpContext.User.IsInRole("SuperAdmin") ? 0 : CurrentUser.GarageId,
                    disabled = HttpContext.User.IsInRole("Administrator")
                },
            };

            return View(model);
        }

        [HttpGet("/{lang:lang}/Garage/Oil/{id}")]
        public async Task<IActionResult> OilManagementList(int id)
        {
            if (id == 0)
                throw new ApplicationException("OilManagementList - Id should ne be set to 0");

            var model = new OilManagementViewModel() { OilList = await _garageService.GetOilList(id) };
            return PartialView("_oils", model);
        }

        [HttpGet("/{lang:lang}/Garage/List")]
        public async Task<IActionResult> GetGarageList()
        {
            var model = new GarageListViewModel() { Garages = await _garageService.GetGarages() };
            return PartialView("_garages", model);
        }

        [HttpGet("/{lang:lang}/Garage/Oil/List")]
        public async Task<IActionResult> GetOilList()
        {
            var model = new OilManagementViewModel() { OilList = await _garageService.GetOilList(CurrentUser.GarageId) };
            return PartialView("_oils", model);
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

        [HttpGet("/{lang:lang}/Garage/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            DateTime activationDate;
            var model = await _garageService.GetGarage(id);

            DateTime.TryParse(model.ActivationDate, out activationDate);

            model.RootUrl = BaseRootUrl;
            if (activationDate != null)
            {
                model.ActivationDate = activationDate.ToString("yyyy-MM-dd");
            }
            model.BannerList = await _referenceService.GetBannerSelectListItem();
            model.LanguageList = await _referenceService.GetLanguageSelectList(CurrentUser.GarageSetting.Language);
            model.DateFormatList = await _referenceService.GetDateFormatSelectList();

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
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Garage/[action]")]
        public async Task<IActionResult> CreateOil(int selectedGarageId, string name)
        {
            try
            {
                var result = await _garageService.CreateOil(selectedGarageId, name);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Garage/IncrementPrintCounter")]
        public async Task<IActionResult> IncrementPrintCounter()
        {
            try
            {
                await _garageService.IncrementPrintCounter(CurrentUser.GarageId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("/{lang:lang}/Garage/Edit")]
        public async Task<IActionResult> Edit(GarageViewModel model)
        {
            try
            {
                var result = await _garageService.Update(model);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Garage/[action]")]
        public async Task<IActionResult> UpdateOil(int id, string name)
        {
            try
            {
                var result = await _garageService.UpdateOil(id, name);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("/{lang:lang}/Garage/Delete")]
        public async Task<IActionResult> Delete(int garageId)
        {
            try
            {
                var result = await _garageService.Delete(garageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpDelete("/{lang:lang}/Garage/Oil/Delete")]
        public async Task<IActionResult> DeleteOil(int id)
        {
            try
            {
                var result = await _garageService.DeleteOil(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return BadRequest();
        }

    }
}