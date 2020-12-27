using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Factory
{
    public class VehicleFactory : IVehicleFactory
    {
        private readonly IConfiguration _configuration;

        public VehicleFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               
        public async Task<IEnumerable<CarMakeModel>> GetMakes()
        {
            var sql = "SELECT DISTINCT [Make] FROM[dbo].[CarQuery] ORDER BY Make";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<CarMakeModel>(sql,
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<CarModelModel>> GetModels(string make)
        {
            var sql = "SELECT DISTINCT [Model] FROM [dbo].[CarQuery] WHERE [Make] = @Make ORDER BY Model";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<CarModelModel>(sql,
                     new
                     {
                         Make = make
                     },
                    commandType: CommandType.Text);

                return result;
            }
        }

    }
}
