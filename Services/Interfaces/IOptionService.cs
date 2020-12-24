using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IOptionService
    {
        Task<PrinterConfigurationViewModel> GetPrinterConfiguration(int garageId);
        Task<int> SavePrinterConfiguration(PrinterConfigurationViewModel configuration, int garageId);
        Task<IEnumerable<OptionViewModel>> GetVerificationList(int garageId);
        Task<IEnumerable<SelectListItem>> GetVerificationSelectList(int garageId);
        Task<int> CreateVerification(int garageId, string name, string description);
        Task<int> UpdateVerification(int id, string name, string description);
        Task<int> DeleteVerification(int id);
        Task<IEnumerable<OptionViewModel>> GetMaintenanceList(int garageId);
        Task<IEnumerable<SelectListItem>> GetMaintenanceSelectList(int garageId);
        Task<int> CreateMaintenance(int garageId, string name);
        Task<int> UpdateMaintenance(int id, string name);
        Task<int> DeleteMaintenance(int id);
        Task<IEnumerable<OptionViewModel>> GetAppointmentList(int garageId);
        Task<IEnumerable<SelectListItem>> GetAppointmentSelectList(int garageId);
        Task<int> CreateAppointment(int garageId, string name);
        Task<int> UpdateAppointment(int id, string name);
        Task<int> DeleteAppointment(int id);
    }
}
