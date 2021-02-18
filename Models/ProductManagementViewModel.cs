using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class ProductManagementViewModel : BaseViewModel
    {
        public int SelectedGarageId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
