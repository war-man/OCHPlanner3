using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class MileageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GarageId { get; set; }
        public int MileageTypeId { get; set; }

    }
}
