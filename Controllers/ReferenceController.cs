using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            IUserService userService) : base(userService)
        {
            _referenceService = referenceService;
            _userService = userService;
        }

        [HttpGet("/reference/intervalSelectList/{mileageType}")]
        public async Task<IEnumerable<MileageViewModel>> GetIntervalSelectListItem(int mileageType)
        {
            return await _referenceService.GetMileageList(CurrentUser.GarageId, mileageType);
        }
    }
}