using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class VINDetailResultViewModel
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string VIN { get; set; }
        public string Transmission { get; set; }
        public string DriveLine { get; set; }
        public string BrakeSystem { get; set; }
        public string Steering { get; set; }
        public string Seating { get; set; }

        public string Description
        {
            get
            {
                return $"{Year} {Make} {Model} {Engine} {Transmission} {DriveLine} {BrakeSystem} {Steering} {Seating}";
            }
        }
    }
}
