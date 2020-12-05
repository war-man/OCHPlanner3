using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IReferenceService
    {
        Task<IEnumerable<OilViewModel>> GetOilList(int garageId);
        Task<IEnumerable<MileageViewModel>> GetMileageList(int garageId, int mileageType = 1);
        Task<IEnumerable<SelectListItem>> GetOilSelectListItem(int garageId, int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetMileageSelectListItem(int garageId, int mileageTypeId, int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetPeriodSelectListItem(int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetYearSelectListItem(int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetMonthSelectListItem(int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetLanguageSelectList(string language);
        Task<IEnumerable<SelectListItem>> GetBannerSelectListItem(int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetDateFormatSelectList();
    }
}
