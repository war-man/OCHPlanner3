using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Models
{
    public class GarageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public int BannerId { get; set; }
        public string Banner { get; set; }
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
        public bool Support { get; set; }
        public string SupportActivationDate { get; set; }
        public int SupportExpiration { get; set; }
        public string Note { get; set; }
        public string NextServiceDistanceLabel { get; set; }
        public string NextDateLabel { get; set; }
        public bool IsModifyTicketLabelByGarage { get; set; }
        public int VINDecodeCount { get; set; }
        public bool FastEntry { get; set; }
        public bool CommunicationModule { get; set; }
        public bool OilResetModule { get; set; }
        public int FormatDateId { get; set; }
        public string FormatDate { get; set; }
        public bool FormatDatePrint { get; set; }

        public int CounterMonthly { get; set; }
        public int CounterOrder { get; set; }
        public int CounterAlert { get; set; }
        public int CounterStock { get; set; }
        public bool UpdateCounterStock { get; set; }
        public string StickerLogo { get; set; }
        public int BrandingId { get; set; }

        public string HelpLinkFr { get; set; }
        public string HelpLinkEn { get; set; }
        public string StoreLinkFr { get; set; }
        public string StoreLinkEn { get; set; }
        public string BrandingLogo { get; set; }
    }
}
