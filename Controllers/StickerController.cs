﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

namespace OCHPlanner3.Controllers
{
    public class StickerController : BaseController
    {
        public readonly IReferenceService _referenceService;
        public readonly IUserService _userService;

        public StickerController(IReferenceService referenceService,
             IUserService userService) : base(userService)
        {
            _referenceService = referenceService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Simple()
        {
            var model = new StickerSimpleViewModel();

            model.OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId);

            return View(model);
        }
    }
}