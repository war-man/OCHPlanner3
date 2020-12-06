using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class OilManagementViewModel : BaseViewModel
    {
        public IEnumerable<OilViewModel> OilList { get; set; }
        public GarageSelectorViewModel GarageSelector { get; set; }
    }
}
