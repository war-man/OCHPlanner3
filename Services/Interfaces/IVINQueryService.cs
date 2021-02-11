using OCHPlanner3.Models;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IVINQueryService
    {
        Task<VINDetailResultViewModel> GetVINDecode(string vin);
    }
}
