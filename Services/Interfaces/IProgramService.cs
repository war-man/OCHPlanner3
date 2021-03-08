using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramViewModel>> GetPrograms(int garageId);
        Task<int> Create(ProgramViewModel program);
        Task<int> Delete(int id);
        Task<int> Update(ProgramViewModel program);
    }
}
