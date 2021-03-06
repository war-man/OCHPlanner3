using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Helper;
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
        private readonly IReferenceFactory _referenceFactory;
        private readonly IBlobStorageService _blobStorageService;

        public GarageService(IGarageFactory garageFactory,
            IReferenceFactory referenceFactory,
            IBlobStorageService blobStorageService)
        {
            _garageFactory = garageFactory;
            _referenceFactory = referenceFactory;
            _blobStorageService = blobStorageService;
        }

        public async Task<IEnumerable<GarageViewModel>> GetGarages()
        {
            var garage = await _garageFactory.GetGarages();
            return garage.Adapt<IEnumerable<GarageViewModel>>();
        }

        public async Task<IEnumerable<SelectListItem>> GetGaragesSelectList()
        {
            var garages = await _garageFactory.GetGarages();

            return garages.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).OrderBy(o => o.Text);
        }

        public async Task<GarageViewModel> GetGarage(int garageId)
        {
            var garage = await _garageFactory.GetGarage(garageId);
            return garage.Adapt<GarageViewModel>();
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

        public async Task<int> Create(GarageViewModel model)
        {
            try
            {
                //default to Canada
                model.Country = "CA";
                model.Phone = model.Phone.ToPhoneDatabase();

                var factoryModel = model.Adapt<GarageModel>();

                var garageId = await _garageFactory.Create(factoryModel);

                //Copy Sticker Logo
                await _blobStorageService.CopyBlob(garageId, "logos");
                              
                return garageId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int garageId)
        {
            //Delete Sticker Logo
            await _blobStorageService.DeleteBlobData(garageId, "logos");

            return await _garageFactory.Delete(garageId);
        }

        public async Task<int> Update(GarageViewModel model)
        {
            try
            {
                model.Phone = model.Phone.ToPhoneDatabase();

                var factoryModel = model.Adapt<GarageModel>();

                return await _garageFactory.Update(factoryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task IncrementPrintCounter(int garageId)
        {
            await _garageFactory.IncrementPrintCounter(garageId);
        }

        public async Task<IEnumerable<OilViewModel>> GetOilList(int garageId)
        {
            var result = new List<OilViewModel>();

            var oilList = await _garageFactory.GetOilList(garageId);
            var oilBaseList = await _referenceFactory.GetBaseOil();
            result.AddRange(oilList.Adapt<IEnumerable<OilViewModel>>());
            result.AddRange(oilBaseList.Adapt<IEnumerable<OilViewModel>>());

            return result;
        }

        public async Task<int> CreateOil(int garageId, string name)
        {
            return await _garageFactory.CreateOil(garageId, name);
        }

        public async Task<int> UpdateOil(int oilId, string name)
        {
            return await _garageFactory.UpdateOil(oilId, name);
        }

        public async Task<int> DeleteOil(int oilId)
        {
            return await _garageFactory.DeleteOil(oilId);
        }
    }
}
