using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Factory
{
    public class ReferenceFactory : IReferenceFactory
    {
        private readonly IConfiguration _configuration;

        public ReferenceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<OilModel>> GetOilList(int garageId)
        {
            var sql = "SELECT [OilTypeID],[OilTypeName],[GarageID] FROM [dbo].[OilType] WHERE [GarageId] = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OilModel>(sql,
                    new { GarageId = garageId },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<MileageModel>> GetMileageList(int garageId, int mileageType = 1)
        {
            if (mileageType == 1) //Km
            {
                return MileageKMGlobal(garageId);
            }
            else if (mileageType == 2) //Miles
            {
                return MileageMilesGlobal(garageId);
            }
            else
            {
                return new List<MileageModel>();
            }
        }

        public async Task<IEnumerable<PeriodModel>> GetPeriodList()
        {
            return new List<PeriodModel>()
                {
                    new PeriodModel() {Id = 1, Name = "1"},
                    new PeriodModel() {Id = 2, Name = "2"},
                    new PeriodModel() {Id = 3, Name = "3"},
                    new PeriodModel() {Id = 4, Name = "4"},
                    new PeriodModel() {Id = 6, Name = "6"},
                    new PeriodModel() {Id = 12, Name = "12"},
                    new PeriodModel() {Id = 18, Name = "18"},
                    new PeriodModel() {Id = 24, Name = "24"},
                    new PeriodModel() {Id = 36, Name = "36"},
                    new PeriodModel() {Id = 48, Name = "48"},
                    new PeriodModel() {Id = 60, Name = "60"}
                };

            //var sql = "SELECT [ID] ,[Name] ,[GarageID] FROM [dbo].[Period] WHERE [GarageId] = @GarageId";

            //using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            //{
            //    connection.Open();

            //    var result = await connection.QueryAsync<PeriodModel>(sql,
            //        new { GarageId = garageId },
            //        commandType: CommandType.Text);

            //    return result;
            //}
        }

        public async Task<IEnumerable<BannerModel>> GetBannerList()
        {
            var sql = "SELECT [ID] ,[Name] ,[Logo] FROM [dbo].[Banners]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<BannerModel>(sql,
                    commandType: CommandType.Text);

                return result;
            }
        }


        private IEnumerable<MileageModel> MileageKMGlobal(int garageId)
        {
            return new List<MileageModel>()
                {
                    new MileageModel() {Id = 1, GarageId = garageId, MileageTypeId = 1, Name = "500"},
                    new MileageModel() {Id = 2, GarageId = garageId, MileageTypeId = 1, Name = "1000"},
                    new MileageModel() {Id = 3, GarageId = garageId, MileageTypeId = 1, Name = "5000"},
                    new MileageModel() {Id = 4, GarageId = garageId, MileageTypeId = 1, Name = "6000"},
                    new MileageModel() {Id = 5, GarageId = garageId, MileageTypeId = 1, Name = "8000"},
                    new MileageModel() {Id = 6, GarageId = garageId, MileageTypeId = 1, Name = "10000"},
                    new MileageModel() {Id = 7, GarageId = garageId, MileageTypeId = 1, Name = "12000"},
                    new MileageModel() {Id = 8, GarageId = garageId, MileageTypeId = 1, Name = "16000"},
                    new MileageModel() {Id = 9, GarageId = garageId, MileageTypeId = 1, Name = "18000"},
                    new MileageModel() {Id = 10, GarageId = garageId, MileageTypeId = 1, Name = "20000"},
                    new MileageModel() {Id = 11, GarageId = garageId, MileageTypeId = 1, Name = "24000"},
                    new MileageModel() {Id = 12, GarageId = garageId, MileageTypeId = 1, Name = "36000"},
                    new MileageModel() {Id = 13, GarageId = garageId, MileageTypeId = 1, Name = "48000"},
                    new MileageModel() {Id = 14, GarageId = garageId, MileageTypeId = 1, Name = "60000"},
                    new MileageModel() {Id = 15, GarageId = garageId, MileageTypeId = 1, Name = "72000"},
                    new MileageModel() {Id = 16, GarageId = garageId, MileageTypeId = 1, Name = "84000"},
                    new MileageModel() {Id = 17, GarageId = garageId, MileageTypeId = 1, Name = "96000"},
                    new MileageModel() {Id = 18, GarageId = garageId, MileageTypeId = 1, Name = "120000"},
                    new MileageModel() {Id = 19, GarageId = garageId, MileageTypeId = 1, Name = "144000"},
                    new MileageModel() {Id = 20, GarageId = garageId, MileageTypeId = 1, Name = "192000"},
                    new MileageModel() {Id = 21, GarageId = garageId, MileageTypeId = 1, Name = "240000"}
                };
        }


        private IEnumerable<MileageModel> MileageMilesGlobal(int garageId)
        {
            return new List<MileageModel>()
                {
                    new MileageModel() {Id = 1, GarageId = garageId, MileageTypeId = 1, Name = "300"},
                    new MileageModel() {Id = 2, GarageId = garageId, MileageTypeId = 1, Name = "600"},
                    new MileageModel() {Id = 3, GarageId = garageId, MileageTypeId = 1, Name = "3000"},
                    new MileageModel() {Id = 4, GarageId = garageId, MileageTypeId = 1, Name = "4000"},
                    new MileageModel() {Id = 5, GarageId = garageId, MileageTypeId = 1, Name = "5000"},
                    new MileageModel() {Id = 6, GarageId = garageId, MileageTypeId = 1, Name = "6000"},
                    new MileageModel() {Id = 7, GarageId = garageId, MileageTypeId = 1, Name = "7500"},
                    new MileageModel() {Id = 8, GarageId = garageId, MileageTypeId = 1, Name = "10000"},
                    new MileageModel() {Id = 9, GarageId = garageId, MileageTypeId = 1, Name = "11000"},
                    new MileageModel() {Id = 10, GarageId = garageId, MileageTypeId = 1, Name = "12000"},
                    new MileageModel() {Id = 11, GarageId = garageId, MileageTypeId = 1, Name = "15000"},
                    new MileageModel() {Id = 12, GarageId = garageId, MileageTypeId = 1, Name = "22000"},
                    new MileageModel() {Id = 13, GarageId = garageId, MileageTypeId = 1, Name = "30000"},
                    new MileageModel() {Id = 14, GarageId = garageId, MileageTypeId = 1, Name = "37000"},
                    new MileageModel() {Id = 15, GarageId = garageId, MileageTypeId = 1, Name = "45000"},
                    new MileageModel() {Id = 16, GarageId = garageId, MileageTypeId = 1, Name = "52000"},
                    new MileageModel() {Id = 17, GarageId = garageId, MileageTypeId = 1, Name = "60000"},
                    new MileageModel() {Id = 18, GarageId = garageId, MileageTypeId = 1, Name = "75000"},
                    new MileageModel() {Id = 19, GarageId = garageId, MileageTypeId = 1, Name = "90000"},
                    new MileageModel() {Id = 20, GarageId = garageId, MileageTypeId = 1, Name = "120000"},
                    new MileageModel() {Id = 21, GarageId = garageId, MileageTypeId = 1, Name = "150000"}
                };
        }

        public async Task<IEnumerable<OilModel>> GetBaseOil()
        {
            var sql = "SELECT [OilTypeID],[OilTypeName] FROM [dbo].[OilBase]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OilModel>(sql,
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<BrandingModel> GetBranding(int brandingId)
        {
            var sql = "SELECT [Id] ,[Name] ,[HelpLinkFr] ,[HelpLinkEn] ,[StoreLinkFr] ,[StoreLinkEn], [LogoUrl] FROM [dbo].[Branding] WHERE [Id] = @BrandingId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<BrandingModel>(sql,
                    new { BrandingId = brandingId },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
