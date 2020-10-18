using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IGarageService
    {
        Task<int> SaveSingleDefault(StickerSimpleDefaultValueViewModel defaultValues, int garageId);
        Task<StickerSimpleDefaultValueViewModel> GetSingleDefault(int garageId);
    }
}
