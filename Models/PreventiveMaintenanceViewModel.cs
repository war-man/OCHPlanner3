using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class PreventiveMaintenanceViewModel : BaseViewModel
    {
        public int LastServiceMileage { get; set; }
        public string LastServiceDate { get; set; }
       
        public VehicleViewModel Vehicle { get; set; }
        public IEnumerable<SelectListItem> VinCodeList { get; set; }
       
    }
}
