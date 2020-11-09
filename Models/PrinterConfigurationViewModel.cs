using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class PrinterConfigurationViewModel : BaseViewModel
    {
        public string SelectedOilPrinter { get; set; }
        public int OilOffsetX { get; set; }
        public int OilOffsetY { get; set; }
    }
}
