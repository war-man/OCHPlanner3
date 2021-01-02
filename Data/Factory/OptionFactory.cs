using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Enum;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Data.Factory
{
    public class OptionFactory : IOptionFactory
    {
        private readonly IConfiguration _configuration;

        public OptionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<OptionModel>> GetBaseOptions(OptionTypeEnum optionType, string language)
        {
            var sql = "SELECT [Id],[Name],[Description] FROM [dbo].[OptionsBase] WHERE OptionType = @optionType AND Language = @language";
                       
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OptionModel>(sql,
                    new
                    {
                        OptionType = (int)optionType,
                        Language = language.ToUpper()
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<OptionModel>> GetOptions(OptionTypeEnum optionType, int garageId)
        {
            var sql = "";

            switch (optionType)
            {
                case OptionTypeEnum.Verification:
                    sql = "SELECT [Id],[Name],[Description] FROM [dbo].[OptionsVerification] WHERE GarageId = @garageId";
                    break;
                case OptionTypeEnum.Maintenance:
                    sql = "SELECT [Id],[Name],[Description] FROM [dbo].[OptionsMaintenance] WHERE GarageId = @garageId";
                    break;
                case OptionTypeEnum.Appointment:
                    sql = "SELECT [Id],[Name],[Description] FROM [dbo].[OptionsNextAppointment] WHERE GarageId = @garageId";
                    break;
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OptionModel>(sql,
                    new
                    {
                        GarageId = garageId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> CreateOption(OptionTypeEnum optionType, int garageId, string name, string description)
        {
            var sql = "";

            switch (optionType)
            {
                case OptionTypeEnum.Verification:
                    sql = @"INSERT INTO [dbo].[OptionsVerification] VALUES(@Name, @Description, @GarageId)";
                    break;
                case OptionTypeEnum.Maintenance:
                    sql = @"INSERT INTO [dbo].[OptionsMaintenance] VALUES(@Name, @Description, @GarageId)";
                    break;
                case OptionTypeEnum.Appointment:
                    sql = @"INSERT INTO [dbo].[OptionsNextAppointment] VALUES(@Name, @Description, @GarageId)";
                    break;
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Name = name,
                        Description = description,
                        GarageId = garageId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> UpdateOption(OptionTypeEnum optionType, int id, string name, string description)
        {
            var sql = "";
           
            switch (optionType)
            {
                case OptionTypeEnum.Verification:
                    sql = @"UPDATE [dbo].[OptionsVerification] SET Name = @Name, Description = @Description WHERE Id = @Id";
                    break;
                case OptionTypeEnum.Maintenance:
                    sql = @"UPDATE [dbo].[OptionsMaintenance] SET Name = @Name, Description = @Description WHERE Id = @Id";
                    break;
                case OptionTypeEnum.Appointment:
                    sql = @"UPDATE [dbo].[OptionsNextAppointment] SET Name = @Name, Description = @Description WHERE Id = @Id";
                    break;
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = id,
                        Name = name,
                        Description = description
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> DeleteOption(OptionTypeEnum optionType, int id)
        {
            var sql = "";

            switch (optionType)
            {
                case OptionTypeEnum.Verification:
                    sql = @"DELETE FROM [dbo].[OptionsVerification] WHERE Id = @Id";
                    break;
                case OptionTypeEnum.Maintenance:
                    sql = @"DELETE FROM [dbo].[OptionsMaintenance] WHERE Id = @Id";
                    break;
                case OptionTypeEnum.Appointment:
                    sql = @"DELETE FROM [dbo].[OptionsNextAppointment] WHERE Id = @Id";
                    break;
            }

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
    }
}
