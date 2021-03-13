using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class OwnerViewModel
    {
        public bool IsReadOnly { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public IEnumerable<SelectListItem> OwnerList { get; set; }
    }
}
