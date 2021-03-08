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
    public class ProgramFactory : IProgramFactory
    {
        private readonly IConfiguration _configuration;

        public ProgramFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> Create(ProgramModel program)
        {
            try
            {

                var sql = @"INSERT INTO [dbo].[Programs2]
                                ([Name]
                                ,[GarageId])
                                VALUES
                                (@Name
                                , @GarageId)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();


                    var result = await connection.ExecuteAsync(sql,
                        new
                        {
                            Name = program.Name,
                            GarageId = program.GarageId
                        },
                        commandType: CommandType.Text);

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int id)
        {

            var sql = @"DELETE FROM [dbo].[Programs2] WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> Update(ProgramModel program)
        {

            try
            {
                var sql = @"UPDATE [dbo].[Programs2]
		                 SET [Name] = @Name
                         WHERE Id = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();


                    var result = await connection.ExecuteAsync(sql,
                         new
                         {
                             Name = program.Name,
                             Id = program.Id
                         },
                         commandType: CommandType.Text);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProgramModel> GetProgram(int id)
        {
            return new ProgramModel();
            //var sql = @"SELECT [Id]
            //              ,[Code]
            //              ,[Name]
            //              ,[Material]
            //              ,[MaterialCost]
            //              ,[MaterialRetail]
            //              ,[WorkTime]
            //              ,[HourlyRateCost]
            //              ,[HourlyRateBillable]
            //              ,[WorkCost]
            //              ,[WorkTotal]
            //              ,[MaintenanceTotalCost]
            //              ,[MaintenanceTotalRetail]
            //              ,[MaintenanceTotalPrice]
            //              ,[ProfitPercentage]
            //              ,[ProfitAmount]
            //              ,[GarageId]
            //          FROM [dbo].[MaintenanceType2]
            //          WHERE Id = @id";

            //using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            //{
            //    connection.Open();

            //    var result = await connection.QueryFirstOrDefaultAsync<MaintenanceTypeModel>(sql,
            //        new
            //        {
            //            Id = id
            //        },
            //        commandType: CommandType.Text);

            //    return result;
            //}
        }

        public async Task<IEnumerable<ProgramModel>> GetPrograms(int garageId)
        {
            var sql = @"SELECT [Id]
                          ,[Name]
                          ,[GarageId]
                      FROM [dbo].[Programs2]
                      WHERE GarageId = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<ProgramModel>(sql,
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
