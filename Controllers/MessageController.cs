using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        public readonly IReferenceService _referenceService;
        public readonly IUserService _userService;
        public readonly IGarageService _garageService;
        public readonly IOptionService _optionService;
        public readonly IVehicleService _vehicleService;

        public MessageController(IReferenceService referenceService,
             IHttpContextAccessor httpContextAccessor,
             IGarageService garageService,
             IOptionService optionService,
             IVehicleService vehicleService,
             IUserService userService) : base(httpContextAccessor, userService)
        {
            _referenceService = referenceService;
            _garageService = garageService;
            _optionService = optionService;
            _vehicleService = vehicleService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var printerConfiguration = await _optionService.GetPrinterConfiguration(CurrentUser.GarageId);

                var model = new MessageClientViewModel()
                {
                    RootUrl = BaseRootUrl,
                    PrinterConfiguration = printerConfiguration ?? new PrinterConfigurationViewModel(),
                    RecommendationList = await _optionService.GetRecommendationSelectList(CurrentUser.GarageId),
                    MaintenanceList = await _optionService.GetMaintenanceSelectList(CurrentUser.GarageId),
                    AppointmentList = await _optionService.GetAppointmentSelectList(CurrentUser.GarageId),
                    CarMakeList = await _vehicleService.GetCarMakeSelectList(),
                    CarColorList = await _vehicleService.GetCarColorSelectList(CurrentUser.GarageSetting.Language)
                };

                //Get garage default
                //var defaultValue = await _garageService.GetSingleDefault(CurrentUser.GarageId);
                //if (defaultValue != null)
                //{
                //    model.Comment = defaultValue.Comment;
                //    model.SelectedUnit = defaultValue.SelectedUnit;
                //    model.SelectedOil = defaultValue.SelectedOil;
                //    model.SelectedChoice = defaultValue.SelectedChoice;
                //    model.SelectedPeriodChoice1 = defaultValue.Choice1SelectedMonth;
                //    model.SelectedMileageChoice1 = defaultValue.Choice1SelectedMileage;
                //    model.SelectedMileageChoice2 = defaultValue.Choice2SelectedMileage;
                //    model.SelectedMonthChoice3 = defaultValue.Choice3SelectedMonth;
                //    model.SelectedYearChoice3 = defaultValue.Choice3SelectedYear;
                //    model.SelectedMileageChoice3 = defaultValue.Choice3SelectedMileage;
                //}


                model.MileageList = await _referenceService.GetMileageSelectListItem(CurrentUser.GarageId, model.SelectedUnit == "KM" ? 1 : model.SelectedUnit == "MI" ? 2 : model.SelectedUnit == "HM" ? 3 : 1);
                //model.PeriodList = await _referenceService.GetPeriodSelectListItem();
                //model.YearList = await _referenceService.GetYearSelectListItem();
                //model.MonthList = await _referenceService.GetMonthSelectListItem(CurrentUser.GarageSetting.Language);
                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("/{lang:lang}/Message/ModelSelectList")]
        public async Task<IEnumerable<SelectListItem>> GetModelListList(string make)
        {
            try
            {
                return await _vehicleService.GetCarModelSelectList(make);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return null;
            }
        }

        [HttpGet("/{lang:lang}/Message/RecommendationMessageToDisplay")]
        public async Task<string> GetRecommendationMessageToDisplay(int recommendationId)
        {
            try
            {
                var recommendationList = await _optionService.GetRecommendationList(CurrentUser.GarageId);
                return recommendationList.FirstOrDefault(p => p.Id == recommendationId)?.Description;
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return null;
            }
        }
    }
}