using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<int> CreateMaintenanceType(MaintenanceTypeViewModel maintenanceType, IEnumerable<MaintenanceTypeProductGroup> products)
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
                        var productList = new List<MaintenanceProduct>(); 

                        foreach (var p in products)
                        {
                            productList.Add(new MaintenanceProduct()
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    public class MaintenanceProduct
    {
        public int MaintenanceTypeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
