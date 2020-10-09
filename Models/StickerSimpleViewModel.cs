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
        public IEnumerable<SelectListItem> OilList { get; set; }
        public int SelectedMonth { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public int SelectedMileage { get; set; }
        public IEnumerable<SelectListItem> MileageList { get; set; }
        public int SelectedPeriod { get; set; }
        public IEnumerable<SelectListItem> PeriodList { get; set; }

        public int SelectedMonthIntervalNextService { get; set; }
        public int SelectedUnitNextService { get; set; }
        public int SelectedYear { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }

        public int SelectedMonthFixe { get; set; }
        public int SelectedYearFixe { get; set; }
        public int SelectedOilFixe { get; set; }

        public string DateBox { get; set; }
        public string KmBox { get; set; }

    }
}
