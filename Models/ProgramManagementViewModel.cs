using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class ProgramManagementViewModel : BaseViewModel
    {
        public int SelectedGarageId { get; set; }
        public IEnumerable<ProgramViewModel> Programs { get; set; }
    }
}
