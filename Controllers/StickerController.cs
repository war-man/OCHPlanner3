using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    public class StickerController : BaseController
    {
        public readonly IReferenceService _referenceService;
        public readonly IUserService _userService;
        public readonly IGarageService _garageService;

        public StickerController(IReferenceService referenceService,
             IGarageService garageService,
             IUserService userService) : base(userService)
        {
            _referenceService = referenceService;
            _garageService = garageService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Simple()
        {
            var model = new StickerSimpleViewModel();

            //Get garage default
            var defaultValue = await _garageService.GetSingleDefault(CurrentUser.GarageId);
            if(defaultValue != null)
            {
                model.Comment = defaultValue.Comment;
                model.SelectedUnit = defaultValue.SelectedUnit;
                model.SelectedOil = defaultValue.SelectedOil;
                model.SelectedChoice = defaultValue.SelectedChoice;
                model.SelectedPeriodChoice1 = defaultValue.Choice1SelectedMonth;
                model.SelectedMileageChoice1 = defaultValue.Choice1SelectedMileage;
                model.SelectedMileageChoice2 = defaultValue.Choice2SelectedMileage;
                model.SelectedMonthChoice3 = defaultValue.Choice3SelectedMonth;
                model.SelectedYearChoice3 = defaultValue.Choice3SelectedYear;
                model.SelectedMileageChoice3 = defaultValue.Choice3SelectedMileage;
            }

            model.OilList = await _referenceService.GetOilSelectListItem(CurrentUser.GarageId);
            model.MileageList = await _referenceService.GetMileageSelectListItem(CurrentUser.GarageId, model.SelectedUnit == "KM" ? 1 : model.SelectedUnit == "MI" ? 2 : model.SelectedUnit == "HM" ? 3 : 1);
            model.PeriodList = await _referenceService.GetPeriodSelectListItem(CurrentUser.GarageId);
            model.YearList = await _referenceService.GetYearSelectListItem();
            model.MonthList = await _referenceService.GetMonthSelectListItem();
            return View(model);
        }

        [HttpPost("/simple/save")]
        public async Task<int> SaveSimpleDefault(StickerSimpleDefaultValueViewModel defaultValue)
        {
            return await _garageService.SaveSingleDefault(defaultValue, base.CurrentUser.GarageId);
        }
    }
}