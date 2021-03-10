using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class VehicleProgramModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int ProgramId { get; set; }
        public string Note { get; set; }

    }
}
