using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

namespace OCHPlanner3.Controllers
{
    public class ReferenceController : BaseController
    {
        private readonly IReferenceService _referenceService;
        private readonly IUserService _userService;

        public ReferenceController(IReferenceService referenceService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _referenceService = referenceService;
            _userService = userService;
        }

        [HttpGet("/{lang:lang}/reference/intervalSelectList/{mileageType}")]
        public async Task<IEnumerable<MileageViewModel>> GetIntervalSelectListItem(int mileageType)
        {
            try
            {
                return await _referenceService.GetMileageList(CurrentUser.GarageId, mileageType);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return null;
            }
        }

        [HttpGet("/{lang:lang}/reference/branding")]
        public async Task<IActionResult> GetBranding(int brandingId)
        {
            try
            {
                return Ok(await _referenceService.GetBranding(brandingId));
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}