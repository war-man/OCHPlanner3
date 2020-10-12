using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Helper.Comparer;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class ReferenceService : IReferenceService
    {
        public readonly IReferenceFactory _referenceFactory;

        public ReferenceService(IReferenceFactory referenceFactory)
        {
            _referenceFactory = referenceFactory;
        }
        public async Task<IEnumerable<OilViewModel>> GetOilList(int garageId)
        {
            var oilList = await _referenceFactory.GetOilList(garageId);
            return oilList.Adapt<IEnumerable<OilViewModel>>();
        }

        public async Task<IEnumerable<MileageViewModel>> GetMileageList(int garageId, int mileageType = 1)
        {
            var mileageList = await _referenceFactory.GetMileageList(garageId, mileageType);
            return mileageList.Adapt<IEnumerable<MileageViewModel>>();
        }

        public async Task<IEnumerable<SelectListItem>> GetOilSelectListItem(int garageId, int selectedId = 0)
        {
            var oilList = await _referenceFactory.GetOilList(garageId);

            return oilList.Select(x => new SelectListItem()
            {
                Value = x.OilTypeId.ToString(),
                Text = x.OilTypeName,
                Selected = selectedId != 0 && selectedId == x.OilTypeId
            }).OrderBy(o => o.Text);
        }

        public async Task<IEnumerable<SelectListItem>> GetPeriodSelectListItem(int garageId, int selectedId = 0)
        {
            var periodList = await _referenceFactory.GetPeriodList(garageId);

            return periodList.Where(p => p.Name.ToUpper() != "N/A").Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = selectedId != 0 && selectedId == x.Id
            }).OrderBy(o => o.Text);
        }
               
        public async Task<IEnumerable<SelectListItem>> GetMileageSelectListItem(int garageId, int mileageTypeId, int selectedId = 0)
        {
            var mileages = await _referenceFactory.GetMileageList(garageId, mileageTypeId);
            return await BuildMileageSelectListItem(mileages.OrderBy(x => x.Name, new SemiNumericComparer()), selectedId);
        }

        private async Task<IEnumerable<SelectListItem>> BuildMileageSelectListItem(IEnumerable<MileageModel> mileageList, int selectedId = 0)
        {
            return mileageList.Where(p => p.Name.ToUpper() != "N/A").Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = selectedId != 0 && selectedId == x.Id
            }).OrderBy(o => o.Text);
        }

        public async Task<IEnumerable<SelectListItem>> GetYearSelectListItem(int selectedId = 0)
        {
            var result = new List<SelectListItem>();

            var currentYear = DateTime.Now.Year;
            var nextYear = DateTime.Now.AddYears(1).Year;

            result.Add(new SelectListItem()
            {
                Value = currentYear.ToString(),
                Text = currentYear.ToString(),
                Selected = selectedId != 0 && selectedId == currentYear
            });

            result.Add(new SelectListItem()
            {
                Value = nextYear.ToString(),
                Text = nextYear.ToString(),
                Selected = selectedId != 0 && selectedId == nextYear
            });

            return result;
        }
    }
}
