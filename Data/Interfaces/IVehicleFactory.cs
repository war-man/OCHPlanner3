using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IVehicleFactory
    {
        Task<IEnumerable<CarMakeModel>> GetMakes();
        Task<IEnumerable<CarModelModel>> GetModels(string make);
        Task<VehicleModel> GetVehicleByVIN(string vinCode);
        Task<int> CreateVehicle(VehicleModel vehicle, int garageId);
        Task<int> UpdateVehicle(VehicleModel vehicle);
        Task<IEnumerable<VehicleProgramModel>> GetVehiclePrograms(int vehicleId);
        Task<IEnumerable<OwnerModel>> GetOwnerList(int garageId);
    }
}
