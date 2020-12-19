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

        public async Task<IEnumerable<GarageModel>> GetGarages()
        {
            var sql = "[web].[Garages_List_Select]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<GarageModel>(sql,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<GarageModel> GetGarage(int garageId)
        {
            var sql = "[web].[Garage_Select]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<GarageModel>(sql,
                    new { GarageId = garageId },
                    commandType: CommandType.StoredProcedure);

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

        public async Task<int> Create(GarageModel garage)
        {
            try
            {
                var sql = "[web].[Garage_Insert]";
                               
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteAsync(sql,
                        new
                        {
                            Name = garage.Name,
                            Address = garage.Address,
                            City = garage.City,
                            State = garage.State,
                            Province = garage.Province,
                            ZipCode = garage.ZipCode,
                            BannerId = garage.BannerId,
                            NbrUser = garage.NbrUser,
                            Phone = garage.Phone,
                            CommunicationModule = garage.CommunicationModule,
                            PersonalizedSticker = garage.PersonalizedSticker,
                            Email = garage.Email,
                            Country = garage.Country,
                            ActivationDate = garage.ActivationDate,
                            Language = garage.Language,
                            OilResetModule = garage.OilResetModule,
                            Support = garage.Support,
                            Note = garage.Note,
                            FormatDate = garage.FormatDate
                        },
                        commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int garageId)
        {
            var sql = "[web].[Garage_Delete]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        GarageId = garageId
                    },
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<int> Update(GarageModel garage)
        {
            try
            {
                var sql = "[web].[Garage_Update]";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteAsync(sql,
                        new
                        {
                            Id = garage.Id,
                            Name = garage.Name,
                            Address = garage.Address,
                            City = garage.City,
                            Province = garage.Province,
                            ZipCode = garage.ZipCode,
                            BannerId = garage.BannerId,
                            NbrUser = garage.NbrUser,
                            Phone = garage.Phone,
                            CommunicationModule = garage.CommunicationModule,
                            PersonalizedSticker = garage.PersonalizedSticker,
                            Email = garage.Email,
                            ActivationDate = garage.ActivationDate,
                            Language = garage.Language,
                            OilResetModule = garage.OilResetModule,
                            Support = garage.Support,
                            Note = garage.Note,
                            FormatDate = garage.FormatDate,
                            CounterOrder = garage.CounterOrder,
                            CounterAlert = garage.CounterAlert,
                            UpdateCounterStock = garage.UpdateCounterStock
                        },
                        commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task IncrementPrintCounter(int garageId)
        {
            var sql = @"UPDATE [dbo].[Garages]
                        SET PrintCount = PrintCount + 1,
                            CounterStock = CounterStock - 1
                        WHERE Id = @garageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        GarageId = garageId
                    },
                    commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<OilModel>> GetOilList(int garageId)
        {
            var sql = @"SELECT OilTypeId, OilTypeName
                        FROM [dbo].[OilType]
                        WHERE GarageId = @garageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OilModel>(sql,
                    new
                    {
                        GarageId = garageId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> CreateOil(int garageId, string name)
        {
            var sql = @"INSERT INTO [dbo].[OilType]
                        VALUES(
                        @OilTypeName,
                        @GarageId
                        )";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        GarageId = garageId,
                        OilTypeName = name,
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> UpdateOil(int oilId, string name)
        {
            var sql = @"UPDATE [dbo].[OilType]
                        SET OilTypeName = @Name
                        WHERE OilTypeId = @OilId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        OilId = oilId,
                        Name = name
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> DeleteOil(int oilId)
        {
            var sql = @"DELETE FROM [dbo].[OilType]
                        WHERE OilTypeID = @OilId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        OilId = oilId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
