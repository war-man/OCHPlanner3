using Mapster;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramFactory _programFactory;

        public ProgramService(IProgramFactory programFactory)
        {
            _programFactory = programFactory;
        }

        public async Task<int> Create(ProgramViewModel program)
        {
            var programModel = program.Adapt<ProgramModel>();
            var result = await _programFactory.Create(programModel);
            return result;
        }

        public async Task<int> Delete(int id)
        {
            return await _programFactory.Delete(id);
        }

        public async Task<int> Update(ProgramViewModel program)
        {
            var programModel = program.Adapt<ProgramModel>();
            var result = await _programFactory.Update(programModel);
            return result;
        }

        public async Task<IEnumerable<ProgramViewModel>> GetPrograms(int garageId)
        {
            var result = await _programFactory.GetPrograms(garageId);
            return result.Adapt<IEnumerable<ProgramViewModel>>();
        }
    }
}
