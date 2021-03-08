using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IProgramFactory
    {
        Task<int> Create(ProgramModel program);
        Task<int> Delete(int id);
        Task<int> Update(ProgramModel program);
        Task<ProgramModel> GetProgram(int id);
        Task<IEnumerable<ProgramModel>> GetPrograms(int garageId);
    }
}
