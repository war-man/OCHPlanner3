using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class GarageListViewModel : BaseViewModel
    {
        public IEnumerable<GarageViewModel> Garages { get; set; }
    }
}
