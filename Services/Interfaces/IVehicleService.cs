using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<SelectListItem>> GetCarMakeSelectList();
        Task<IEnumerable<SelectListItem>> GetCarColorSelectList(string language);
        Task<IEnumerable<SelectListItem>> GetCarModelSelectList(string make);
        Task<VehicleViewModel> GetVehicleByVIN(string vin, int garageId);
        Task<int> SaveVehicle(VehicleViewModel model);
        Task<IEnumerable<ProgramViewModel>> GetVehiclePrograms(int vehicleId, int garageId);
    }
}
