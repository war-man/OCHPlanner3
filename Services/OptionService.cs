using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Enum;
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

        #region Verification
        public async Task<IEnumerable<OptionViewModel>> GetVerificationList(int garageId)
        {
            var result = new List<OptionViewModel>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Verification, garage.Language);
            result.AddRange(optionBaseList.Adapt<IEnumerable<OptionViewModel>>());

            var verificationList = await _optionFactory.GetOptions(OptionTypeEnum.Verification, garageId);
            result.AddRange(verificationList.Adapt<IEnumerable<OptionViewModel>>());

            return result;
        }
        public async Task<IEnumerable<SelectListItem>> GetVerificationSelectList(int garageId)
        {
            var result = new List<SelectListItem>();

            var garage = await _garageFactory.GetGarage(garageId);
            var optionBaseList = await _optionFactory.GetBaseOptions(OptionTypeEnum.Verification, garage.Language);

            optionBaseList.ToList().ForEach(opt =>
            {
                result.Add(new SelectListItem()
                {
                    Value = opt.Id.ToString(),
                    Text = opt.Name
                });
            });

            var optionList = await _optionFactory.GetOptions(OptionTypeEnum.Verification, garageId);

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

        public async Task<int> CreateVerification(int garageId, string name, string description)
        {
            return await _optionFactory.CreateOption(OptionTypeEnum.Verification, garageId, name, description);
        }

        public async Task<int> UpdateVerification(int id, string name, string description)
        {
            return await _optionFactory.UpdateOption(OptionTypeEnum.Verification, id, name, description);
        }
        
        public async Task<int> DeleteVerification(int id)
        {
            return await _optionFactory.DeleteOption(OptionTypeEnum.Verification, id);
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
    }
}
