using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<SelectListItem>> GetCarMakeSelectList();
        Task<IEnumerable<SelectListItem>> GetCarColorSelectList(string language);
        Task<IEnumerable<SelectListItem>> GetCarModelSelectList(string make);
    }
}
