using OCHPlanner3.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IVehicleFactory
    {
        Task<IEnumerable<CarMakeModel>> GetMakes();
        Task<IEnumerable<CarModelModel>> GetModels(string make);
    }
}
