using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System.IO;
using Exceptionless;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class GarageController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IGarageService _garageService;
        private readonly IReferenceService _referenceService;
        private readonly IBlobStorageService _blobStorageService;

        public GarageController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IReferenceService referenceService,
            IBlobStorageService blobStorageService,
            IGarageService garageService) : base(httpContextAccessor, userService)
        {
            _garageService = garageService;
            _referenceService = referenceService;
            _blobStorageService = blobStorageService;
        }

        [Route("/{lang:lang}/Garages")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new GarageListViewModel()
                {
                    RootUrl = BaseRootUrl,
                    Garages = await _garageService.GetGarages()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("/{lang:lang}/Garage/List")]
        public async Task<IActionResult> GetGarageList()
        {
            try
            {
                var model = new GarageListViewModel() { Garages = await _garageService.GetGarages() };
                return PartialView("_garages", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Authorize(Roles = "SuperAdmin, Administrator")]
        [HttpGet("/{lang:lang}/Garage/Create")]
        public async Task<IActionResult> Create()
        {
            try
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
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Authorize(Roles = "SuperAdmin, Administrator")]
        [HttpGet("/{lang:lang}/Garage/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
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
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Authorize(Roles = "SuperAdmin, Administrator")]
        [HttpPost("/{lang:lang}/Garage/Create")]
        public async Task<IActionResult> Create(GarageViewModel model)
        {
            try
            {
                var result = await _garageService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
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
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Authorize(Roles = "SuperAdmin, Administrator")]
        [HttpPut("/{lang:lang}/Garage/Edit")]
        public async Task<IActionResult> Edit(GarageViewModel model)
        {
            try
            {
                var result = await _garageService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Authorize(Roles = "SuperAdmin, Administrator")]
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
                ex.ToExceptionless().Submit();
                return BadRequest();
            }

        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteLogo(int garageId)
        {
            try
            {
                await _blobStorageService.DeleteBlobData(garageId);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult> UploadLogo(int garageId, IFormFile MyUploader)
        {
            try
            {
                byte[] fileData;
                using (var target = new MemoryStream())
                {
                    MyUploader.CopyTo(target);
                    fileData = target.ToArray();
                }

                var imageUrl = _blobStorageService.UploadFileToBlob($"{garageId}.png", fileData, MyUploader.ContentType);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}