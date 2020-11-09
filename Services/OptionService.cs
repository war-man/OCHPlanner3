using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
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

        public OptionService(IGarageFactory garageFactory)
        {
            _garageFactory = garageFactory;
        }

        public async Task<PrinterConfigurationViewModel> GetPrinterConfiguration(int garageId)
        {
            var defaultValueModel = await _garageFactory.GetSingleDefault(garageId, "PRINTERCONFIGURATION");

            if (defaultValueModel == null) return null;

            var printerConfiguration = JsonConvert.DeserializeObject<PrinterConfigurationViewModel>(defaultValueModel.DefaultValues);
            return printerConfiguration;
        }

        //public async Task<int> SaveSingleDefault(StickerSimpleDefaultValueViewModel defaultValues, int garageId)
        //{
        //    var defaultValueModel = new GarageDefaultModel()
        //    {
        //        GarageId = garageId,
        //        Screen = "SINGLE",
        //        DefaultValues = JsonConvert.SerializeObject(defaultValues)
        //    };

        //    var exist = await _garageFactory.GetSingleDefault(garageId, "SINGLE");
        //    if (exist != null)
        //    {
        //        defaultValueModel.Id = exist.Id;
        //    }

        //    return exist == null 
        //        ? await _garageFactory.CreateSingleDefault(defaultValueModel)
        //        : await _garageFactory.UpdateSingleDefault(defaultValueModel);
        //}

       
    }
}
