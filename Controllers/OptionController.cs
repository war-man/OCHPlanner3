using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class OptionController : BaseController
    {
        private readonly IGarageService _garageService;
        private readonly IOptionService _optionService;

        public OptionController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IGarageService garageService,
            IOptionService optionService) : base(httpContextAccessor, userService)
        {
            _optionService = optionService;
            _garageService = garageService;
        }

        #region Printers

        [Route("/{lang:lang}/Options/Printer")]
        public async Task<IActionResult> Printer()
        {
            try
            {
                //Get printer Configuration
                var printerConfiguration = await _optionService.GetPrinterConfiguration(CurrentUser.GarageId);

                var model = printerConfiguration ?? new PrinterConfigurationViewModel();
                model.RootUrl = BaseRootUrl;

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/Printer/Save")]
        public async Task<int> SavePrinter(PrinterConfigurationViewModel printerConfig)
        {
            try
            {
                return await _optionService.SavePrinterConfiguration(printerConfig, base.CurrentUser.GarageId);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return 0;
            }
        }

        #endregion

        #region Oil Management

        [Route("/{lang:lang}/Options/Oil")]
        public async Task<IActionResult> OilManagement()
        {
            try
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
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("/{lang:lang}/Options/Oil/{id}")]
        public async Task<IActionResult> OilManagementList(int id)
        {
            try
            {
                if (id == 0)
                    throw new ApplicationException("OilManagementList - Id should ne be set to 0");

                var model = new OilManagementViewModel() { OilList = await _garageService.GetOilList(id) };
                return PartialView("_oils", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/CreateOil")]
        public async Task<IActionResult> CreateOil(int selectedGarageId, string name)
        {
            try
            {
                var result = await _garageService.CreateOil(selectedGarageId, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/UpdateOil")]
        public async Task<IActionResult> UpdateOil(int id, string name)
        {
            try
            {
                var result = await _garageService.UpdateOil(id, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }


        [HttpDelete("/{lang:lang}/Options/DeleteOil")]
        public async Task<IActionResult> DeleteOil(int id)
        {
            try
            {
                var result = await _garageService.DeleteOil(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        #endregion

        #region Recommendation Management

        [Route("/{lang:lang}/Options/Recommendation")]
        public async Task<IActionResult> RecommendationManagement()
        {
            try
            {
                var model = new RecommendationManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    RecommendationList = await _optionService.GetRecommendationList(CurrentUser.GarageId),
                    GarageSelector = new GarageSelectorViewModel
                    {
                        Garages = await _garageService.GetGaragesSelectList(),
                        SelectedGarageId = HttpContext.User.IsInRole("SuperAdmin") ? 0 : CurrentUser.GarageId,
                        disabled = HttpContext.User.IsInRole("Administrator")
                    },
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
              
        [HttpGet("/{lang:lang}/Options/Recommendation/{id}")]
        public async Task<IActionResult> RecommendationManagementList(int id)
        {
            try
            {
                if (id == 0)
                    throw new ApplicationException("RecommendationManagementList - Id should ne be set to 0");

                var model = new RecommendationManagementViewModel() { RecommendationList = await _optionService.GetRecommendationList(id) };
                return PartialView("_recommendations", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/CreateRecommendation")]
        public async Task<IActionResult> CreateRecommendation(int selectedGarageId, string name, string description)
        {
            try
            {
                var result = await _optionService.CreateRecommendation(selectedGarageId, name, description);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/UpdateRecommendation")]
        public async Task<IActionResult> UpdateRecommendation(int id, string name, string description)
        {
            try
            {
                var result = await _optionService.UpdateRecommendation(id, name, description);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }


        [HttpDelete("/{lang:lang}/Options/DeleteRecommendation")]
        public async Task<IActionResult> DeleteRecommendation(int id)
        {
            try
            {
                var result = await _optionService.DeleteRecommendation(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
        #endregion

        #region Maintenance Management

        [Route("/{lang:lang}/Options/Maintenance")]
        public async Task<IActionResult> MaintenanceManagement()
        {
            try
            {
                var model = new MaintenanceManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    MaintenanceList = await _optionService.GetMaintenanceList(CurrentUser.GarageId),
                    GarageSelector = new GarageSelectorViewModel
                    {
                        Garages = await _garageService.GetGaragesSelectList(),
                        SelectedGarageId = HttpContext.User.IsInRole("SuperAdmin") ? 0 : CurrentUser.GarageId,
                        disabled = HttpContext.User.IsInRole("Administrator")
                    },
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("/{lang:lang}/Options/Maintenance/{id}")]
        public async Task<IActionResult> MaintenanceManagementList(int id)
        {
            try
            {
                if (id == 0)
                    throw new ApplicationException("MaintenanceManagementList - Id should ne be set to 0");

                var model = new MaintenanceManagementViewModel() { MaintenanceList = await _optionService.GetMaintenanceList(id) };
                return PartialView("_maintenances", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/CreateMaintenance")]
        public async Task<IActionResult> CreateMaintenance(int selectedGarageId, string name)
        {
            try
            {
                var result = await _optionService.CreateMaintenance(selectedGarageId, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/UpdateMaintenance")]
        public async Task<IActionResult> UpdateMaintenance(int id, string name)
        {
            try
            {
                var result = await _optionService.UpdateMaintenance(id, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }


        [HttpDelete("/{lang:lang}/Options/DeleteMaintenance")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            try
            {
                var result = await _optionService.DeleteMaintenance(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
        #endregion

        #region Appointment Management

        [Route("/{lang:lang}/Options/Appointment")]
        public async Task<IActionResult> AppointmentManagement()
        {
            try
            {
                var model = new AppointmentManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    AppointmentList = await _optionService.GetAppointmentList(CurrentUser.GarageId),
                    GarageSelector = new GarageSelectorViewModel
                    {
                        Garages = await _garageService.GetGaragesSelectList(),
                        SelectedGarageId = HttpContext.User.IsInRole("SuperAdmin") ? 0 : CurrentUser.GarageId,
                        disabled = HttpContext.User.IsInRole("Administrator")
                    },
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("/{lang:lang}/Options/Appointment/{id}")]
        public async Task<IActionResult> AppointmentManagementList(int id)
        {
            try
            {
                if (id == 0)
                    throw new ApplicationException("AppointmentManagementList - Id should ne be set to 0");

                var model = new AppointmentManagementViewModel() { AppointmentList = await _optionService.GetAppointmentList(id) };
                return PartialView("_appointments", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/CreateAppointment")]
        public async Task<IActionResult> CreateAppointment(int selectedGarageId, string name)
        {
            try
            {
                var result = await _optionService.CreateAppointment(selectedGarageId, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/UpdateAppointment")]
        public async Task<IActionResult> UpdateAppointment(int id, string name)
        {
            try
            {
                var result = await _optionService.UpdateAppointment(id, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }


        [HttpDelete("/{lang:lang}/Options/DeleteAppointment")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var result = await _optionService.DeleteAppointment(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
        #endregion

        #region Product Management

        [HttpGet("/{lang:lang}/Options/Product/{id}")]
        public async Task<IActionResult> ProductManagementList(int id)
        {
            try
            {
                if (id == 0)
                    throw new ApplicationException("ProductManagementList - Id should ne be set to 0");

                var model = new ProductManagementViewModel() { Products = await _optionService.GetProductList(id) };
                return PartialView("_appointments", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GarageId</param>
        /// <returns></returns>
        [Route("/{lang:lang}/Options/Products/{id}")]
        public async Task<IActionResult> ProductManagement(int id)
        {
            try
            {
                var model = new ProductManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    SelectedGarageId = id,
                    Products = await _optionService.GetProductList(id)
                };


                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductViewModel product)
        {
            try
            {
                var result = await _optionService.CreateProduct(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Options/UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductViewModel product)
        {
            try
            {
                var result = await _optionService.UpdateProduct(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("/{lang:lang}/Options/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _optionService.DeleteProduct(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        #endregion
    }
}
