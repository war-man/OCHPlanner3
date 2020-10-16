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
    public class GarageFactory : IGarageFactory
    {
        private readonly IConfiguration _configuration;

        public GarageFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<GarageModel> GetGarage(int garageId)
        {
            var sql = "SELECT * FROM [dbo].[Garages] WHERE [ID] = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<GarageModel>(sql,
                    new { GarageId = garageId },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
