using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Factory
{
    public class MaintenancePlanFactory : IMaintenancePlanFactory
    {
        private readonly IConfiguration _configuration;

        public MaintenancePlanFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
              
        public async Task<IEnumerable<MaintenancePlanModel>> GetMaintenancePlans(int garageId)
        {
            var sql = @"SELECT [Id]
                          ,[Name]
                          
                      FROM [dbo].[MaintenancePlan2]
                      WHERE GarageId = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<MaintenancePlanModel>(sql,
                    new
                    {
                        GarageId = garageId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

    }

}
