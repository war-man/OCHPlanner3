using OCHPlanner3.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IGarageFactory
    {
        Task<IEnumerable<GarageModel>> GetGarages();
        Task<GarageModel> GetGarage(int garageId);
        Task<int> CreateSingleDefault(GarageDefaultModel defaultValues);
        Task<int> UpdateSingleDefault(GarageDefaultModel defaultValues);
        Task<GarageDefaultModel> GetSingleDefault(int garageId, string screen);
    }
}
