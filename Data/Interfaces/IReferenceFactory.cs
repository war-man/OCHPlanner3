using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IReferenceFactory
    {
        Task<IEnumerable<OilModel>> GetOilList(int garageId);
        Task<IEnumerable<MileageModel>> GetMileageList(int garageId, int mileageType = 1);
        Task<IEnumerable<PeriodModel>> GetPeriodList();
        Task<IEnumerable<BannerModel>> GetBannerList();
        Task<IEnumerable<OilModel>> GetBaseOil();
        Task<BrandingModel> GetBranding(int brandingId);
    }
}
