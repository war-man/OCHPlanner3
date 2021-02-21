using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Extensions;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class MaintenanceTypeController : BaseController
    {
        public readonly IOptionService _optionService;
        public readonly IMaintenanceTypeService _maintenanceTypeService;

        public MaintenanceTypeController(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IMaintenanceTypeService maintenanceTypeService,
            IOptionService optionService) : base(httpContextAccessor, userService)
        {
            _optionService = optionService;
            _maintenanceTypeService = maintenanceTypeService;
        }

        public IActionResult Index3(BrandingViewModel model)
        {
            return View();
        }

        [Route("/{lang:lang}/MaintenanceType/{id}")]
        public IActionResult Index(int id)
        {
            try
            {
                var model = new MaintenanceTypeManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    SelectedGarageId = id,
                    MaintenanceTypeList = new List<MaintenanceTypeViewModel>()
                };


                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Route("/{lang:lang}/MaintenanceType/Create/{id}")]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                await ClearProducts();

                var model = new MaintenanceTypeViewModel()
                {
                    RootUrl = BaseRootUrl,
                    GarageId = id,
                    ProductList = await _optionService.GetProductSelectListItem(id),
                    Products = new List<MaintenanceTypeProductGroup>()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Route("/{lang:lang}/MaintenanceType/SelectedProducts")]
        public async Task<IActionResult> GetSelectedProduct(MaintenanceTypeProductGroup product)
        {
            try
            {
                var result = new List<string>();
                var products = HttpContext.Session.GetObject<IList<MaintenanceTypeProductGroup>>("SelectedProducts");
                if (products == null)
                {
                    products = new List<MaintenanceTypeProductGroup>();
                }
                               
                products.ToList().ForEach(p =>
                {
                    result.Add($"{p.Product.Id}|{p.Quantity}");
                });

                return Ok(string.Join(",", result));
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        private async Task<IActionResult> ClearProducts()
        {
            try
            {
                var products = HttpContext.Session.GetObject<IList<MaintenanceTypeProductGroup>>("SelectedProducts");
                if (products != null)
                {
                    HttpContext.Session.Remove("SelectedProducts");

                }

                return Ok();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Route("/{lang:lang}/MaintenanceType/AddProduct")]
        public async Task<IActionResult> AddProduct(MaintenanceTypeProductGroup product)
        {
            try
            {
                var products = HttpContext.Session.GetObject<IList<MaintenanceTypeProductGroup>>("SelectedProducts");
                if (products == null)
                {
                    products = new List<MaintenanceTypeProductGroup>();
                }

                products.Add(new MaintenanceTypeProductGroup()
                {
                    Product = await _optionService.GetProduct(product.Product.Id),
                    Quantity = product.Quantity
                });

                HttpContext.Session.SetObject("SelectedProducts", products);

                return PartialView("_selectedProducts", products);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [Route("/{lang:lang}/MaintenanceType/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var products = HttpContext.Session.GetObject<IList<MaintenanceTypeProductGroup>>("SelectedProducts");

                products.Remove(products.FirstOrDefault(p => p.Product.Id == id));

                HttpContext.Session.SetObject("SelectedProducts", products);

                return PartialView("_selectedProducts", products);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/MaintenanceType/Create")]
        public async Task<IActionResult> Create(MaintenanceTypeViewModel model)
        {
            try
            {
                var result = await _maintenanceTypeService.CreateMaintenanceType(model);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}
