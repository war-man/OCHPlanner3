using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class BaseViewModel
    {
        public string RootUrl { get; set; }
        public PrinterConfigurationViewModel PrinterConfiguration { get; set; }
    }
}
