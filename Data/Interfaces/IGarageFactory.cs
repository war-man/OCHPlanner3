using OCHPlanner3.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IGarageFactory
    {
        Task<GarageModel> GetGarage(int garageId);
    }
}
