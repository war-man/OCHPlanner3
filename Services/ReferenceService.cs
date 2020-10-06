using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IEnumerable<MileageViewModel>> GetMileageList(int garageId)
        {
            var mileageList = await _referenceFactory.GetMileageList(garageId);
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

            return periodList.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = selectedId != 0 && selectedId == x.Id
            }).OrderBy(o => o.Text);
        }

        public async Task<IEnumerable<SelectListItem>> GetMonthSelectListItem(int selectedId = 0)
        {
            return new List<SelectListItem>
            {
                new SelectListItem() { Value = "1", Text = "Janvier", Selected = selectedId != 0 && selectedId == 1 },
                new SelectListItem() { Value = "2", Text = "Février", Selected = selectedId != 0 && selectedId == 2 },
                new SelectListItem() { Value = "3", Text = "Mars", Selected = selectedId != 0 && selectedId == 3 },
                new SelectListItem() { Value = "4", Text = "Avril", Selected = selectedId != 0 && selectedId == 4 },
                new SelectListItem() { Value = "5", Text = "Mai", Selected = selectedId != 0 && selectedId == 5 },
                new SelectListItem() { Value = "6", Text = "Juin", Selected = selectedId != 0 && selectedId == 6 },
                new SelectListItem() { Value = "7", Text = "Juillet", Selected = selectedId != 0 && selectedId == 7 },
                new SelectListItem() { Value = "8", Text = "Août", Selected = selectedId != 0 && selectedId == 8 },
                new SelectListItem() { Value = "9", Text = "Septembre", Selected = selectedId != 0 && selectedId == 9 },
                new SelectListItem() { Value = "10", Text = "Octobre" , Selected = selectedId != 0 && selectedId == 10 },
                new SelectListItem() { Value = "11", Text = "Novembre" , Selected = selectedId != 0 && selectedId == 11 },
                new SelectListItem() { Value = "12", Text = "Décembre" , Selected = selectedId != 0 && selectedId == 12 }
            };
        }

        public async Task<IEnumerable<SelectListItem>> GetMileageSelectListItem(int garageId, int mileageTypeId, int selectedId = 0)
        {
            var mileage = await _referenceFactory.GetMileageList(garageId);

            switch (mileageTypeId)
            {
                //KM
                case 1:
                    return await BuildMileageSelectListItem(mileage.Where(p => p.MileageTypeId == 1), selectedId);
                //Miles
                case 2:
                    return await BuildMileageSelectListItem(mileage.Where(p => p.MileageTypeId == 2), selectedId);
                //Hr Moteur
                case 3:
                    return await BuildMileageSelectListItem(mileage.Where(p => p.MileageTypeId == 3), selectedId);
                default:
                    return new List<SelectListItem>();
            }
        }

        private async Task<IEnumerable<SelectListItem>> BuildMileageSelectListItem(IEnumerable<MileageModel> mileage, int selectedId = 0)
        {
            var result = new List<SelectListItem>();

            mileage.ToList().ForEach(m =>
            {
                result.Add(new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Name,
                    Selected = selectedId != 0 && selectedId == m.Id
                });
            });

            return result.OrderBy(o => o.Text);
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
