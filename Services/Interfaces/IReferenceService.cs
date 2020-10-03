using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IReferenceService
    {
        Task<IEnumerable<OilViewModel>> GetOilList(int garageId);
        Task<IEnumerable<SelectListItem>> GetOilSelectListItem(int garageId, int selectedId = 0);
    }
}
