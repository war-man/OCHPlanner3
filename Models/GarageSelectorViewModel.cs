using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class GarageSelectorViewModel
    {
        public string Name { get; set; }
        public int SelectedGarageId { get; set; }
        public IEnumerable<SelectListItem> Garages { get; set; }
        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }
    }
}
