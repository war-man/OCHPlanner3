using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OCHPlanner3.Models
{
    public class StickerSimpleViewModel
    {
        public int Id { get; set; }
        public string SelectedUnit { get; set; }
        public string UnitValue { get; set; }
        public string Comment { get; set; }
        public int SelectedOil { get; set; }
        public List<SelectListItem> OilList { get; set; } 

        public int SelectedMonthIntervalNextService { get; set; }
        public int SelectedUnitNextService { get; set; }

        public int selectedDateOfDayInterval { get; set; }

        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }

        public string DateBox { get; set; }
        public string KmBox { get; set; }

    }
}
