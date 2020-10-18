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
    public class GarageService : IGarageService
    {
        private readonly IGarageFactory _garageFactory;

        public GarageService(IGarageFactory garageFactory)
        {
            _garageFactory = garageFactory;
        }

        public async Task<StickerSimpleDefaultValueViewModel> GetSingleDefault(int garageId)
        {
            var defaultValueModel = await _garageFactory.GetSingleDefault(garageId, "SINGLE");

            if (defaultValueModel == null) return null;

            var stickerSimpleDefault = JsonConvert.DeserializeObject<StickerSimpleDefaultValueViewModel>(defaultValueModel.DefaultValues);
            return stickerSimpleDefault;
        }

        public async Task<int> SaveSingleDefault(StickerSimpleDefaultValueViewModel defaultValues, int garageId)
        {
            var defaultValueModel = new GarageDefaultModel()
            {
                GarageId = garageId,
                Screen = "SINGLE",
                DefaultValues = JsonConvert.SerializeObject(defaultValues)
            };

            var exist = await _garageFactory.GetSingleDefault(garageId, "SINGLE");
            if (exist != null)
            {
                defaultValueModel.Id = exist.Id;
            }

            return exist == null 
                ? await _garageFactory.CreateSingleDefault(defaultValueModel)
                : await _garageFactory.UpdateSingleDefault(defaultValueModel);
        }
    }
}
