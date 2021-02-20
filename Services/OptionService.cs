using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Enum;
using OCHPlanner3.Helper.Comparer;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class OptionService : IOptionService
    {
        private readonly IGarageFactory _garageFactory;
        private readonly IOptionFactory _optionFactory;

        public OptionService(IGarageFactory garageFactory,
            IOptionFactory optionFactory)
        {
            _garageFactory = garageFactory;
            _optionFactory = optionFactory;
        }

        #region Printer
        public async Task<PrinterConfigurationViewModel> GetPrinterConfiguration(int garageId)
        {
            var defaultValueModel = await _garageFactory.GetSingleDefault(garageId, "PRINTERCONFIGURATION");

            if (defaultValueModel == null) return null;

            var printerConfiguration = JsonConvert.DeserializeObject<PrinterConfigurationViewModel>(defaultValueModel.DefaultValues);
            return printerConfiguration;
        }
               
        public async Task<int> SavePrinterConfiguration(PrinterConfigurationViewModel configuration, int garageId)
        {

            var defaultValueModel = new GarageDefaultModel()
            {
                GarageId = garageId,
                Screen = "PRINTERCONFIGURATION",
                DefaultValues = JsonConvert.SerializeObject(configuration)
            };

            var exist = await _garageFactory.GetSingleDefault(garageId, "PRINTERCONFIGURATION");
            if (exist != null)
            {
                defaultValueModel.Id = exist.Id;
            }

            return exist == null
                ? await _garageFactory.CreateSingleDefault(defaultValueModel)
                : await _garageFactory.UpdateSingleDefault(defaultValueModel);
        }
        #endregion

        #region Recommendation
        public async Task<IEnumerable<OptionViewModel>> GetRecommendationList(int garageId)
        {
            var result = new List<OptionViewModel>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Recommendation, garage.Language);
            result.AddRange(optionBaseList.Adapt<IEnumerable<OptionViewModel>>());

            var reommendationList = await _optionFactory.GetOptions(OptionTypeEnum.Recommendation, garageId);
            result.AddRange(reommendationList.Adapt<IEnumerable<OptionViewModel>>());

            return result;
        }
        public async Task<IEnumerable<SelectListItem>> GetRecommendationSelectList(int garageId)
        {
            var result = new List<SelectListItem>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Recommendation, garage.Language);

            optionBaseList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            var optionList = await _optionFactory.GetOptions(OptionTypeEnum.Recommendation, garageId);

            optionList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            return result.OrderBy(o => o.Text);
        }

        public async Task<int> CreateRecommendation(int garageId, string name, string description)
        {
            return await _optionFactory.CreateOption(OptionTypeEnum.Recommendation, garageId, name, description);
        }

        public async Task<int> UpdateRecommendation(int id, string name, string description)
        {
            return await _optionFactory.UpdateOption(OptionTypeEnum.Recommendation, id, name, description);
        }
        
        public async Task<int> DeleteRecommendation(int id)
        {
            return await _optionFactory.DeleteOption(OptionTypeEnum.Recommendation, id);
        }

        #endregion

        #region Maintenance

        public async Task<IEnumerable<OptionViewModel>> GetMaintenanceList(int garageId)
        {
            var result = new List<OptionViewModel>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Maintenance, garage.Language);
            result.AddRange(optionBaseList.Adapt<IEnumerable<OptionViewModel>>());

            var maintenanceList = await _optionFactory.GetOptions(OptionTypeEnum.Maintenance, garageId);
            result.AddRange(maintenanceList.Adapt<IEnumerable<OptionViewModel>>());

            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetMaintenanceSelectList(int garageId)
        {
            var result = new List<SelectListItem>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Maintenance, garage.Language);

            optionBaseList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            var optionList = await _optionFactory.GetOptions(OptionTypeEnum.Maintenance, garageId);

            optionList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            return result.OrderBy(o => o.Text);
        }

        public async Task<int> CreateMaintenance(int garageId, string name)
        {
            return await _optionFactory.CreateOption(OptionTypeEnum.Maintenance, garageId, name, string.Empty);
        }

        public async Task<int> UpdateMaintenance(int id, string name)
        {
            return await _optionFactory.UpdateOption(OptionTypeEnum.Maintenance, id, name, string.Empty);
        }

        public async Task<int> DeleteMaintenance(int id)
        {
            return await _optionFactory.DeleteOption(OptionTypeEnum.Maintenance, id);
        }

        #endregion

        #region Appointment

        public async Task<IEnumerable<OptionViewModel>> GetAppointmentList(int garageId)
        {
            var result = new List<OptionViewModel>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Appointment, garage.Language);
            result.AddRange(optionBaseList.Adapt<IEnumerable<OptionViewModel>>());

            var appointmentList = await _optionFactory.GetOptions(OptionTypeEnum.Appointment, garageId);
            result.AddRange(appointmentList.Adapt<IEnumerable<OptionViewModel>>());

            return result;
        }
        public async Task<IEnumerable<SelectListItem>> GetAppointmentSelectList(int garageId)
        {
            var result = new List<SelectListItem>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Appointment, garage.Language);

            optionBaseList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            var optionList = await _optionFactory.GetOptions(OptionTypeEnum.Appointment, garageId);

            optionList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            return result.OrderBy(o => o.Text);
        }

        public async Task<int> CreateAppointment(int garageId, string name)
        {
            return await _optionFactory.CreateOption(OptionTypeEnum.Appointment, garageId, name, string.Empty);
        }

        public async Task<int> UpdateAppointment(int id, string name)
        {
            return await _optionFactory.UpdateOption(OptionTypeEnum.Appointment, id, name, string.Empty);
        }

        public async Task<int> DeleteAppointment(int id)
        {
            return await _optionFactory.DeleteOption(OptionTypeEnum.Appointment, id);
        }

        #endregion

        #region Product
        
        public async Task<int> CreateProduct(ProductViewModel product)
        {
            return await _optionFactory.CreateProduct(product.Adapt<ProductModel>());
        }

        public async Task<int> UpdateProduct(ProductViewModel product)
        {
            return await _optionFactory.UpdateProduct(product.Adapt<ProductModel>());
        }

        public async Task<int> DeleteProduct(int id)
        {
            return await _optionFactory.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductList(int garageId)
        {
            var result = new List<ProductViewModel>();
                       
            var productList = await _optionFactory.GetProductList(garageId);
            result.AddRange(productList.Adapt<IEnumerable<ProductViewModel>>());

            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetProductSelectListItem(int garageId, int selectedId = 0)
        {
            var products = await _optionFactory.GetProductList(garageId);
            return await BuildProductSelectListItem(products.OrderBy(x => x.Description, new SemiNumericComparer()), selectedId);
        }

        private async Task<IEnumerable<SelectListItem>> BuildProductSelectListItem(IEnumerable<ProductModel> productList, int selectedId = 0)
        {
            return productList.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Description,
                Selected = selectedId != 0 && selectedId == x.Id
            });
        }

        public async Task<ProductViewModel> GetProduct(int Id)
        {
            var product = await _optionFactory.GetProduct(Id);
            return product.Adapt<ProductViewModel>();
        }

        #endregion
    }
}
