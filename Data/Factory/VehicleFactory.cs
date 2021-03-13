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
                var sql = @"SELECT V.[Id]
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
                      ,V.[SelectedUnit]
                      ,V.[EntryDate]
                      ,V.[MonthlyMileage]
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

        public async Task<int> CreateVehicle(VehicleModel vehicle, int garageId)
        {
            try
            {
                var sqlOwner = @"INSERT INTO [dbo].[VehicleOwner]
                                ([Company] 
                                ,[Name]
                                ,[Address]
                                ,[Phone] 
                                ,[Email]
                                ,[GarageId])
                                OUTPUT INSERTED.Id
                                VALUES
                                (@OwnerCompany
                                , @OwnerName
                                , @OwnerAddress
                                , @OwnerPhone
                                , @OwnerEmail
                                , @GarageId)";

                var sqlDriver = @"INSERT INTO [dbo].[VehicleDriver]
                                ([Name] 
                                ,[Phone]
                                ,[Cellphone]
                                ,[Email] 
                                ,[Notes])
                                OUTPUT INSERTED.Id
                                VALUES
                                (@DriverName
                                , @DriverPhone
                                , @DriverCellphone
                                , @DriverEmail
                                , @DriverNotes)";

                var sql = @"INSERT INTO [dbo].[Vehicle2]
		               ([Vincode]
                      ,[Description]
                      ,[Year]
                      ,[Make]
                      ,[Model]
                      ,[Engine]
                      ,[Transmission]
                      ,[Propulsion]
                      ,[BrakeSystem]
                      ,[Steering]
                      ,[Color]
                      ,[UnitNo]
                      ,[LicencePlate]
                      ,[Seating]
                      ,[Odometer]
                      ,[SelectedUnit]
                      ,[EntryDate]
                      ,[MonthlyMileage]
                      ,[OilTypeId]
                      ,[MaintenancePlanId]
                      ,[VehicleOwnerId]
                      ,[VehicleDriverId])
                    OUTPUT INSERTED.Id
	                VALUES(
		                 @Vincode
                        ,@Description
                        ,@Year
                        ,@Make
                        ,@Model
                        ,@Engine
                        ,@Transmission
                        ,@Propulsion
                        ,@BrakeSystem
                        ,@Steering
                        ,@Color
                        ,@UnitNo
                        ,@LicencePlate
                        ,@Seating
                        ,@Odometer
                        ,@SelectedUnit
                        ,@EntryDate
                        ,@MonthlyMileage
                        ,@OilTypeId
                        ,@MaintenancePlanId
                        ,@VehicleOwnerId
                        ,@VehicleDriverId)";

                var sqlProgram = @"INSERT INTO [dbo].[Vehicle2_Program2]
                                ([VehicleId] 
                                ,[ProgramId]
                                ,[Note])
                                VALUES
                                (@VehicleId
                                , @ProgramId
                                , @Note)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        //insert owner
                        var OwnerInserted = await connection.QuerySingleAsync<int>(sqlOwner,
                            new
                            {
                                OwnerCompany = vehicle.OwnerCompany,
                                OwnerName = vehicle.OwnerName,
                                OwnerAddress = vehicle.OwnerAddress,
                                OwnerPhone = vehicle.OwnerPhone,
                                OwnerEmail = vehicle.OwnerEmail,
                                GarageId = garageId
                                
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //insert driver
                        var DriverInserted = await connection.QuerySingleAsync<int>(sqlDriver,
                            new
                            {
                                DriverName = vehicle.DriverName,
                                DriverPhone = vehicle.DriverPhone,
                                DriverCellphone = vehicle.DriverCellphone,
                                DriverNotes = vehicle.DriverNotes,
                                DriverEmail = vehicle.DriverEmail

                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //insert vehicle

                        var vehicleInserted = await connection.QuerySingleAsync<int>(sql,
                            new
                            {
                                Vincode = vehicle.VinCode
                                ,Description = string.IsNullOrWhiteSpace(vehicle.Description) ? string.Empty : vehicle.Description
                                ,Year = vehicle.Year
                                ,Make = string.IsNullOrWhiteSpace(vehicle.Make) ? string.Empty : vehicle.Make
                                ,Model = string.IsNullOrWhiteSpace(vehicle.Model) ? string.Empty : vehicle.Model
                                ,Engine = string.IsNullOrWhiteSpace(vehicle.Engine) ? string.Empty : vehicle.Engine
                                ,Transmission = string.IsNullOrWhiteSpace(vehicle.Transmission) ? string.Empty : vehicle.Transmission
                                ,Propulsion = string.IsNullOrWhiteSpace(vehicle.Propulsion) ? string.Empty : vehicle.Propulsion
                                ,BrakeSystem = string.IsNullOrWhiteSpace(vehicle.BrakeSystem) ? string.Empty : vehicle.BrakeSystem
                                ,Steering = string.IsNullOrWhiteSpace(vehicle.Steering) ? string.Empty : vehicle.Steering
                                ,Color = string.IsNullOrWhiteSpace(vehicle.Color) ? string.Empty : vehicle.Color                                
                                ,UnitNo = string.IsNullOrWhiteSpace(vehicle.UnitNo) ? string.Empty : vehicle.UnitNo
                                ,LicencePlate = string.IsNullOrWhiteSpace(vehicle.LicencePlate) ? string.Empty : vehicle.LicencePlate
                                ,Seating = string.IsNullOrWhiteSpace(vehicle.Seating) ? string.Empty : vehicle.Seating
                                ,Odometer = vehicle.Odometer                                
                                ,SelectedUnit = vehicle.SelectedUnit                                 
                                ,EntryDate = vehicle.EntryDate                                 
                                ,MonthlyMileage = vehicle.MonthlyMileage                                
                                ,OilTypeId = vehicle.OilTypeId                                
                                ,MaintenancePlanId = vehicle.MaintenancePlanId                                
                                ,VehicleOwnerId = OwnerInserted
                                ,VehicleDriverId = DriverInserted
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        if (vehicle.VehicleProgram.Any())
                        {
                            var programList = new List<VehicleProgramModel>();

                            vehicle.VehicleProgram.ToList().ForEach(p =>
                            {
                                programList.Add(new VehicleProgramModel()
                                {
                                    ProgramId = p.ProgramId,
                                    VehicleId = vehicleInserted,
                                    Note = p.Note
                                });
                            });

                            var affectedRows = await connection.ExecuteAsync(sqlProgram, programList, transaction: transaction);
                        }

                        transaction.Commit();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateVehicle(VehicleModel vehicle)
        {
            try
            {
                var sqlOwner = @"UPDATE [dbo].[VehicleOwner]
                                SET [Company] = @OwnerCompany 
                                ,[Name] = @OwnerName
                                ,[Address] = @OwnerAddress
                                ,[Phone]  = @OwnerPhone
                                ,[Email] = @OwnerEmail
                                WHERE Id = @OwnerId";

                var sqlDriver = @"UPDATE [dbo].[VehicleDriver]
                                SET [Name] = @DriverName 
                                ,[Phone] = @DriverPhone
                                ,[Cellphone] = @DriverCellphone
                                ,[Email] = @DriverEmail
                                ,[Notes] = @DriverNotes
                                WHERE Id = @DriverId";

                var sql = @"UPDATE [dbo].[Vehicle2]
		               SET [Vincode] = @Vincode
                      ,[Description] = @Description
                      ,[Year] = @Year
                      ,[Make] = @Make
                      ,[Model] = @Model
                      ,[Engine] = @Engine
                      ,[Transmission] = @Transmission
                      ,[Propulsion] = @Propulsion
                      ,[BrakeSystem] = @BrakeSystem
                      ,[Steering] = @Steering
                      ,[Color] = @Color
                      ,[UnitNo] = @UnitNo
                      ,[LicencePlate] = @LicencePlate
                      ,[Seating] = @Seating
                      ,[Odometer] = @Odometer
                      ,[SelectedUnit] = @SelectedUnit
                      ,[EntryDate] = @EntryDate
                      ,[MonthlyMileage] = @MonthlyMileage
                      ,[OilTypeId] = @OilTypeId
                      ,[MaintenancePlanId] = @MaintenancePlanId
                        WHERE Id = @Id";

                var sqlProgramDelete = @"DELETE FROM [dbo].[Vehicle2_Program2] WHERE VehicleId = @VehicleId";

                var sqlProgram = @"INSERT INTO [dbo].[Vehicle2_Program2]
                                ([VehicleId] 
                                ,[ProgramId]
                                ,[Note])
                                VALUES
                                (@VehicleId
                                , @ProgramId
                                , @Note)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        //update owner
                        await connection.ExecuteAsync(sqlOwner,
                            new
                            {
                                OwnerCompany = vehicle.OwnerCompany,
                                OwnerName = vehicle.OwnerName,
                                OwnerAddress = vehicle.OwnerAddress,
                                OwnerPhone = vehicle.OwnerPhone,
                                OwnerEmail = vehicle.OwnerEmail,
                                OwnerId = vehicle.VehicleOwnerId
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //update driver
                        await connection.ExecuteAsync(sqlDriver,
                            new
                            {
                                DriverName = vehicle.DriverName,
                                DriverPhone = vehicle.DriverPhone,
                                DriverCellphone = vehicle.DriverCellphone,
                                DriverNotes = vehicle.DriverNotes,
                                DriverEmail = vehicle.DriverEmail,
                                DriverId = vehicle.VehicleDriverId
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //update vehicle

                        await connection.ExecuteAsync(sql,
                            new
                            {
                                Vincode = vehicle.VinCode,
                                Description = string.IsNullOrWhiteSpace(vehicle.Description) ? string.Empty : vehicle.Description,
                                Year = vehicle.Year,
                                Make = string.IsNullOrWhiteSpace(vehicle.Make) ? string.Empty : vehicle.Make,
                                Model = string.IsNullOrWhiteSpace(vehicle.Model) ? string.Empty : vehicle.Model,
                                Engine = string.IsNullOrWhiteSpace(vehicle.Engine) ? string.Empty : vehicle.Engine,
                                Transmission = string.IsNullOrWhiteSpace(vehicle.Transmission) ? string.Empty : vehicle.Transmission,
                                Propulsion = string.IsNullOrWhiteSpace(vehicle.Propulsion) ? string.Empty : vehicle.Propulsion,
                                BrakeSystem = string.IsNullOrWhiteSpace(vehicle.BrakeSystem) ? string.Empty : vehicle.BrakeSystem,
                                Steering = string.IsNullOrWhiteSpace(vehicle.Steering) ? string.Empty : vehicle.Steering,
                                Color = string.IsNullOrWhiteSpace(vehicle.Color) ? string.Empty : vehicle.Color,
                                UnitNo = string.IsNullOrWhiteSpace(vehicle.UnitNo) ? string.Empty : vehicle.UnitNo,
                                LicencePlate = string.IsNullOrWhiteSpace(vehicle.LicencePlate) ? string.Empty : vehicle.LicencePlate,
                                Seating = string.IsNullOrWhiteSpace(vehicle.Seating) ? string.Empty : vehicle.Seating,
                                Odometer = vehicle.Odometer,
                                SelectedUnit = vehicle.SelectedUnit,
                                EntryDate = vehicle.EntryDate,
                                MonthlyMileage = vehicle.MonthlyMileage,
                                OilTypeId = vehicle.OilTypeId,
                                MaintenancePlanId = vehicle.MaintenancePlanId,
                                Id = vehicle.Id
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        if(vehicle.VehicleProgram.Any())
                        {
                            //delete Vehicle program
                            await connection.ExecuteAsync(sqlProgramDelete,
                                new
                                {
                                    VehicleId = vehicle.Id,
                                    
                                },
                                commandType: CommandType.Text,
                                transaction: transaction);

                            var programList = new List<VehicleProgramModel>();

                            vehicle.VehicleProgram.ToList().ForEach(p =>
                            {
                                programList.Add(new VehicleProgramModel()
                                {
                                    ProgramId = p.ProgramId,
                                    VehicleId = p.VehicleId,
                                    Note = p.Note
                                });
                            });

                            var affectedRows = await connection.ExecuteAsync(sqlProgram, programList, transaction: transaction);
                        }

                        transaction.Commit();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<VehicleProgramModel>> GetVehiclePrograms(int vehicleId)
        {
            
                var sql = @"SELECT [Id]
                      ,[VehicleId]
                      ,[ProgramId]
                      ,[Note]
                  FROM[dbo].[Vehicle2_Program2]
                  WHERE [VehicleId] = @VehicleId";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QueryAsync<VehicleProgramModel>(sql,
                         new
                         {
                             VehicleId = vehicleId
                         },
                        commandType: CommandType.Text);

                    return result;
                }
           
        }

        public async Task<IEnumerable<OwnerModel>> GetOwnerList(int garageId)
        {
            var sql = "SELECT [Id], [Company], [Name] FROM [dbo].[VehicleOwner] WHERE [GarageId] = @GarageId ORDER BY [Name]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OwnerModel>(sql,
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
