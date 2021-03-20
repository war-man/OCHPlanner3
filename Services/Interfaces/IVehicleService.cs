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
        Task<int> SaveVehicle(VehicleViewModel model, int garageId);
        Task<IEnumerable<ProgramViewModel>> GetVehiclePrograms(int vehicleId, int garageId);
        Task<IEnumerable<SelectListItem>> GetOwnerSelectListItem(int garageId, int selectedId = 0);
        Task<IEnumerable<SelectListItem>> GetVinSelectListItem(int garageId, string selected = "");
        Task<OwnerViewModel> GetOwner(int ownerId);
        Task<OwnerViewModel> GetOwner(string name, string phone, int garageId);
    }
}
