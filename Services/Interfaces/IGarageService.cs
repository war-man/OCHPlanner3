using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IGarageService
    {
        Task<int> SaveSingleDefault(StickerSimpleDefaultValueViewModel defaultValues, int garageId);
        Task<StickerSimpleDefaultValueViewModel> GetSingleDefault(int garageId);
        Task<IEnumerable<SelectListItem>> GetGaragesSelectList();
        Task<GarageViewModel> GetGarage(int garageId);
        Task<IEnumerable<GarageViewModel>> GetGarages();
        Task<int> Create(GarageViewModel model);
        Task<int> Delete(int garageId);
        Task<int> Update(GarageViewModel model);
        Task IncrementPrintCounter(int garageId);
        Task<IEnumerable<OilViewModel>> GetOilList(int garageId);
        Task<int> CreateOil(int garageId, string name);
        Task<int> UpdateOil(int oilId, string name);
        Task<int> DeleteOil(int oilId);
    }
}
