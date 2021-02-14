using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DetailPrice { get; set; }
    }

    public class ProductManagementViewModel : BaseViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
