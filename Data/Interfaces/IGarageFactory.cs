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
        Task<int> Create(GarageModel garage);
        Task<int> Delete(int garageId);
        Task<int> Update(GarageModel garage);
        Task IncrementPrintCounter(int garageId);
        Task<IEnumerable<OilModel>> GetOilList(int garageId);
        Task<int> CreateOil(int garageId, string name);
        Task<int> UpdateOil(int oilId, string name);
        Task<int> DeleteOil(int oilId);
    }
}
