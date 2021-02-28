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
    public class MaintenanceTypeFactory : IMaintenanceTypeFactory
    {
        private readonly IConfiguration _configuration;

        public MaintenanceTypeFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateMaintenanceType(MaintenanceTypeModel maintenanceType, IEnumerable<MaintenanceTypeProductGroupViewModel> products)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[MaintenanceType2]
		                ([Code]
                        ,[Name]
                        ,[Material]
                        ,[MaterialCost]
                        ,[MaterialRetail]
                        ,[WorkTime]
                        ,[HourlyRateCost]
                        ,[HourlyRateBillable]
                        ,[WorkCost]
                        ,[WorkTotal]
                        ,[MaintenanceTotalCost]
                        ,[MaintenanceTotalRetail]
                        ,[MaintenanceTotalPrice]
                        ,[ProfitPercentage]
                        ,[ProfitAmount]
                        ,[GarageId])
                    OUTPUT INSERTED.Id
	                VALUES(
		                 @Code
                        ,@Name
                        ,@Material
                        ,@MaterialCost
                        ,@MaterialRetail
                        ,@WorkTime
                        ,@HourlyRateCost
                        ,@HourlyRateBillable
                        ,@WorkCost
                        ,@WorkTotal
                        ,@MaintenanceTotalCost
                        ,@MaintenanceTotalRetail
                        ,@MaintenanceTotalPrice
                        ,@ProfitPercentage
                        ,@ProfitAmount
                        ,@GarageId)";

                var sqlProduct = @"INSERT INTO [dbo].[MaintenanceType2_Product]
                                ([MaintenanceTypeId]
                                ,[ProductId]
                                ,[Quantity])
                                VALUES
                                (@MaintenanceTypeId
                                , @ProductId
                                , @Quantity)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var maintenanceTypeInserted = await connection.QuerySingleAsync<int>(sql,
                            new
                            {
                                Code = maintenanceType.Code,
                                Name = maintenanceType.Name,
                                Material = maintenanceType.Material,
                                MaterialCost = maintenanceType.MaterialCost,
                                MaterialRetail = maintenanceType.MaterialRetail,
                                WorkTime = maintenanceType.WorkTime,
                                HourlyRateCost = maintenanceType.HourlyRateCost,
                                HourlyRateBillable = maintenanceType.HourlyRateBillable,
                                WorkCost = maintenanceType.WorkCost,
                                WorkTotal = maintenanceType.WorkTotal,
                                MaintenanceTotalCost = maintenanceType.MaintenanceTotalCost,
                                MaintenanceTotalRetail = maintenanceType.MaintenanceTotalRetail,
                                MaintenanceTotalPrice = maintenanceType.MaintenanceTotalPrice,
                                ProfitPercentage = maintenanceType.ProfitPercentage,
                                ProfitAmount = maintenanceType.ProfitAmount,
                                GarageId = maintenanceType.GarageId
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //insert related products
                        var productList = new List<MaintenanceProductModel>();

                        foreach (var p in products)
                        {
                            productList.Add(new MaintenanceProductModel()
                            {
                                MaintenanceTypeId = (int)maintenanceTypeInserted,
                                ProductId = p.Product.Id,
                                Quantity = p.Quantity
                            });
                        }

                        var affectedRows = await connection.ExecuteAsync(sqlProduct, productList, transaction: transaction);
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

        public async Task<int> Delete(int id)
        {
            var sql = @"DELETE FROM [dbo].[MaintenanceType2] WHERE Id = @Id";
            var sqlProduct = @"DELETE FROM [dbo].[MaintenanceType2_Product] WHERE MaintenanceTypeId = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.Text,
                    transaction: transaction);

                    var productRows = await connection.ExecuteAsync(sqlProduct,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.Text,
                    transaction: transaction);

                    transaction.Commit();

                    return 1;
                }
            }
        }

        public async Task<int> EditMaintenanceType(MaintenanceTypeModel maintenanceType, List<MaintenanceTypeProductGroupViewModel> products)
        {
            try
            {
                var sql = @"UPDATE [dbo].[MaintenanceType2]
		                 SET [Code] = @Code
                        ,[Name] = @Name
                        ,[Material] = @Material
                        ,[MaterialCost] = @MaterialCost
                        ,[MaterialRetail] = @MaterialRetail
                        ,[WorkTime] = @WorkTime
                        ,[HourlyRateCost] = @HourlyRateCost
                        ,[HourlyRateBillable] = @HourlyRateBillable
                        ,[WorkCost] = @WorkCost
                        ,[WorkTotal] = @WorkTotal
                        ,[MaintenanceTotalCost] = @MaintenanceTotalCost
                        ,[MaintenanceTotalRetail] = @MaintenanceTotalRetail
                        ,[MaintenanceTotalPrice] = @MaintenanceTotalPrice
                        ,[ProfitPercentage] = @ProfitPercentage
                        ,[ProfitAmount] = @ProfitAmount
                        WHERE Id = @Id";

                //Delete All products
                var sqlProductDelete = @"DELETE FROM [dbo].[MaintenanceType2_Product] WHERE MaintenanceTypeId = @Id";

                //Insert Products
                var sqlProductInsert = @"INSERT INTO [dbo].[MaintenanceType2_Product]
                                ([MaintenanceTypeId]
                                ,[ProductId]
                                ,[Quantity])
                                VALUES
                                (@MaintenanceTypeId
                                , @ProductId
                                , @Quantity)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var maintenanceTypeUpdated = await connection.ExecuteAsync(sql,
                            new
                            {
                                Code = maintenanceType.Code,
                                Name = maintenanceType.Name,
                                Material = maintenanceType.Material,
                                MaterialCost = maintenanceType.MaterialCost,
                                MaterialRetail = maintenanceType.MaterialRetail,
                                WorkTime = maintenanceType.WorkTime,
                                HourlyRateCost = maintenanceType.HourlyRateCost,
                                HourlyRateBillable = maintenanceType.HourlyRateBillable,
                                WorkCost = maintenanceType.WorkCost,
                                WorkTotal = maintenanceType.WorkTotal,
                                MaintenanceTotalCost = maintenanceType.MaintenanceTotalCost,
                                MaintenanceTotalRetail = maintenanceType.MaintenanceTotalRetail,
                                MaintenanceTotalPrice = maintenanceType.MaintenanceTotalPrice,
                                ProfitPercentage = maintenanceType.ProfitPercentage,
                                ProfitAmount = maintenanceType.ProfitAmount,
                                Id = maintenanceType.Id
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //delete all related products
                        var deleteProducts = await connection.ExecuteAsync(sqlProductDelete,
                            new
                            {
                                Id = maintenanceType.Id
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //insert related products
                        var productList = new List<MaintenanceProductModel>();

                        foreach (var p in products)
                        {
                            productList.Add(new MaintenanceProductModel()
                            {
                                MaintenanceTypeId = maintenanceType.Id,
                                ProductId = p.Product.Id,
                                Quantity = p.Quantity
                            });
                        }

                        var affectedRows = await connection.ExecuteAsync(sqlProductInsert, productList, transaction: transaction);
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

        public async Task<MaintenanceTypeModel> GetMaintenanceType(int id)
        {
            var sql = @"SELECT [Id]
                          ,[Code]
                          ,[Name]
                          ,[Material]
                          ,[MaterialCost]
                          ,[MaterialRetail]
                          ,[WorkTime]
                          ,[HourlyRateCost]
                          ,[HourlyRateBillable]
                          ,[WorkCost]
                          ,[WorkTotal]
                          ,[MaintenanceTotalCost]
                          ,[MaintenanceTotalRetail]
                          ,[MaintenanceTotalPrice]
                          ,[ProfitPercentage]
                          ,[ProfitAmount]
                          ,[GarageId]
                      FROM [dbo].[MaintenanceType2]
                      WHERE Id = @id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<MaintenanceTypeModel>(sql,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<MaintenanceTypeModel>> GetMaintenanceTypes(int garageId)
        {
            var sql = @"SELECT [Id]
                          ,[Code]
                          ,[Name]
                          ,[Material]
                          ,[MaterialCost]
                          ,[MaterialRetail]
                          ,[WorkTime]
                          ,[HourlyRateCost]
                          ,[HourlyRateBillable]
                          ,[WorkCost]
                          ,[WorkTotal]
                          ,[MaintenanceTotalCost]
                          ,[MaintenanceTotalRetail]
                          ,[MaintenanceTotalPrice]
                          ,[ProfitPercentage]
                          ,[ProfitAmount]
                          ,[GarageId]
                      FROM [dbo].[MaintenanceType2]
                      WHERE GarageId = @GarageId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<MaintenanceTypeModel>(sql,
                    new
                    {
                        GarageId = garageId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<MaintenanceTypeProductGroupModel>> GetSelectedProducts(int id)
        {
            var sql = @"SELECT [Id]
                          ,[MaintenanceTypeId]
                          ,[ProductId]
                          ,[Quantity]
                      FROM [dbo].[MaintenanceType2_Product]
                      WHERE [MaintenanceTypeId] = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<MaintenanceTypeProductGroupModel>(sql,
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
