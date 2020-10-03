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
    }
}
