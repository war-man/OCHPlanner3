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

    }
}
