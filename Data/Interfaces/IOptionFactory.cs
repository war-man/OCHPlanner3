using OCHPlanner3.Data.Models;
using OCHPlanner3.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Interfaces
{
    public interface IOptionFactory
    {
        Task<IEnumerable<OptionModel>> GetBaseOptions(OptionTypeEnum optionType, string language);
        Task<IEnumerable<OptionModel>> GetOptions(OptionTypeEnum optionType, int garageId);
        Task<int> CreateOption(OptionTypeEnum optionType, int garageId, string name, string description);
        Task<int> UpdateOption(OptionTypeEnum optionType, int Id, string name, string description);
        Task<int> DeleteOption(OptionTypeEnum optionType, int id);
    }
}
