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
        
        public async Task<VehicleModel> GetVehicleByVIN(string vinCode)
        {
            try
            {
                var sql = @"SELECT TOP(1000) V.[Id]
                      ,V.[Vincode]
                      ,V.[Description]
                      ,V.[Year]
                      ,V.[Make]
                      ,V.[Model]
                      ,V.[Engine]
                      ,V.[Transmission]
                      ,V.[Propulsion]
                      ,V.[BrakeSystem]
                      ,V.[Steering]
                      ,V.[Color]
                      ,V.[UnitNo]
                      ,V.[LicencePlate]
                      ,V.[Seating]
                      ,V.[Odometer]
                      ,V.[MileageType]
                      ,V.[EntryDate]
                      ,V.[MonthlyAverage]
                      ,V.[OilTypeId]
                      ,V.[MaintenancePlanId]
                      ,V.[VehicleOwnerId]
                      ,V.[VehicleDriverId]
	                  ,VO.[Company] AS 'OwnerCompany'
                      ,VO.[Name] AS 'OwnerName'
                      ,VO.[Address] as 'OwnerAddress'
                      ,VO.[Phone] as 'OwnerPhone'
                      ,VO.[Email] as 'OwnerEmail'
	                  ,VD.[Name] as 'DriverName'
                      ,VD.[Phone] as 'DriverPhone'
                      ,VD.[Cellphone] as 'DriverCellphone'
                      ,VD.[Email] as 'DriverEmail'
                      ,VD.[Notes] as 'DriverNotes'
                  FROM[dbo].[Vehicle2] V
                  INNER JOIN VehicleOwner VO
                      ON VO.Id = V.VehicleOwnerId
                  INNER JOIN VehicleDriver VD
                      ON VD.Id = V.VehicleDriverId
                WHERE V.[VinCode] = @VinCode";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QueryFirstOrDefaultAsync<VehicleModel>(sql,
                         new
                         {
                             VinCode = vinCode
                         },
                        commandType: CommandType.Text);

                    return result;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
