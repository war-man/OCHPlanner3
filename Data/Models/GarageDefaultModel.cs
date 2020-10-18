using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Models
{
    public class GarageDefaultModel
    {
        public int Id { get; set; }
        public int GarageId { get; set; }
        public string Screen { get; set; }
        public string DefaultValues { get; set; }
    }
}
