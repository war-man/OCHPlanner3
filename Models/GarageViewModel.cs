using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Models
{
    public class GarageViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public int SelectedBannerId { get; set; }
        public string Banner { get; set; }
        public IEnumerable<SelectListItem> BannerList { get; set; }
        public int NbrUser { get; set; }
        public int NbrCustomer { get; set; }
        public string Phone { get; set; }
        public bool PersonalizedSticker { get; set; }
        public string PersonalizedMessage { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string ActivationDate { get; set; }
        public int PrintCount { get; set; }
        public string Language { get; set; }
        public string SelectedLanguageCode { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }
        public bool Support { get; set; }
        public bool OilReset { get; set; }
        public bool Communication { get; set; }
        public string SupportActivationDate { get; set; }
        public int SupportExpiration { get; set; }
        public string Note { get; set; }
        public string NextServiceDistanceLabel { get; set; }
        public string NextDateLabel { get; set; }
        public bool IsModifyTicketLabelByGarage { get; set; }
        public int VINDecodeCount { get; set; }
        public bool FastEntry { get; set; }

        public int FormatDateId { get; set; }
        public string FormatDate { get; set; }
        public bool FormatDatePrint { get; set; }
        public string SelectedDateFormatCode { get; set; }
        public IEnumerable<SelectListItem> DateFormatList { get; set; }

        public int CounterMonthly { get; set; }
        public int CounterOrder { get; set; }
        public int CounterAlert { get; set; }
        public int CounterStock { get; set; }

        public bool UpdateCounterStock { get; set; }
    }
}
