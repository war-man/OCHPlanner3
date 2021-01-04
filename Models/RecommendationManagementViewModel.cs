using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class RecommendationManagementViewModel : BaseViewModel
    {
        public IEnumerable<OptionViewModel> RecommendationList { get; set; }
        public GarageSelectorViewModel GarageSelector { get; set; }
    }
}
