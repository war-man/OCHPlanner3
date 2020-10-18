using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class StickerSimpleDefaultValueViewModel
    {
        
        public string SelectedUnit { get; set; }
        public string Comment { get; set; }
        public int SelectedOil { get; set; }
        public string SelectedChoice { get; set; }
        
        public int Choice1SelectedMonth { get; set; }
        public int Choice1SelectedMileage { get; set; }
        public int Choice2SelectedMileage { get; set; }

        public int Choice3SelectedMonth { get; set; }
        public int Choice3SelectedYear { get; set; }
        public int Choice3SelectedMileage { get; set; }
    }
}
