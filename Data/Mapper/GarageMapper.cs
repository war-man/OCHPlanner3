using Mapster;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Mapper
{
    public static class GarageMapper
    {
        public static void ConfigGarageMapper()
        {
            TypeAdapterConfig<GarageViewModel, GarageModel>
                .NewConfig()
                .Map(dest => dest.Language, src => src.SelectedLanguageCode)
                .Map(dest => dest.FormatDate, src => src.SelectedDateFormatCode)
                .Map(dest => dest.BannerId, src => src.SelectedBannerId)
                .Map(dest => dest.Province, src => src.Province.ToUpper())
                .Map(dest => dest.OilResetModule, src => src.OilReset)
                .Map(dest => dest.CommunicationModule, src => src.Communication)
                .Map(dest => dest.Support, src => src.Support)
                .Map(dest => dest.ActivationDate, src => Convert.ToDateTime(src.ActivationDate).ToString("dd/MM/yyyy"));

        }
    }
}
