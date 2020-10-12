using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
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
            var sql = "SELECT [ID] ,[Name] ,[GarageID] ,[MileageTypeID] FROM [dbo].[Mileage] WHERE [GarageId] = @GarageId AND [MileageTypeId] = @MileageType";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<MileageModel>(sql,
                    new { 
                        GarageId = garageId,
                        MileageType = mileageType
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<PeriodModel>> GetPeriodList(int garageId)
        {
            var sql = "SELECT [ID] ,[Name] ,[GarageID] FROM [dbo].[Period] WHERE [GarageId] = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<PeriodModel>(sql,
                    new { GarageId = garageId },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
