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
    public class GarageFactory : IGarageFactory
    {
        private readonly IConfiguration _configuration;

        public GarageFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<GarageModel> GetGarage(int garageId)
        {
            var sql = @"SELECT G.*, FD.FormatDateId, FD.Français AS 'FormatDate', FD.FormatDatePrint 
                        FROM [dbo].[Garages] G
                        INNER JOIN [dbo].[FormatDate] FD ON FD.[GarageId] = G.ID
                        WHERE [ID] = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<GarageModel>(sql,
                    new { GarageId = garageId },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<GarageDefaultModel> GetSingleDefault(int garageId, string screen)
        {
            var sql = @"SELECT 
                        Id,
                        GarageId,
                        Screen,
                        DefaultValues
                        FROM [dbo].[Garage_Defaults]
                        WHERE GarageId = @garageId
                        AND Screen = @screen";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<GarageDefaultModel>(sql,
                    new
                    {
                        GarageId = garageId,
                        Screen = screen
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> CreateSingleDefault(GarageDefaultModel defaultValues)
        {
            var sql = @"INSERT INTO [dbo].[Garage_Defaults]
                        VALUES(
                        @GarageId,
                        @Screen,
                        @DefaultValues)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        GarageId = defaultValues.GarageId,
                        Screen = defaultValues.Screen,
                        DefaultValues = defaultValues.DefaultValues
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> UpdateSingleDefault(GarageDefaultModel defaultValues)
        {
            var sql = @"UPDATE [dbo].[Garage_Defaults]
                        SET DefaultValues = @DefaultValues
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = defaultValues.Id,
                        DefaultValues = defaultValues.DefaultValues
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
