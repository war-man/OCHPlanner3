using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class MaintenanceTypeService : IMaintenanceTypeService
    {
        private readonly IMaintenanceTypeFactory _maintenanceTypeFactory;

        public MaintenanceTypeService(IMaintenanceTypeFactory maintenanceTypeFactory)
        {
            _maintenanceTypeFactory = maintenanceTypeFactory;
        }

        public async Task<int> CreateMaintenanceType(MaintenanceTypeViewModel maintenanceType)
        {
            var products = new List<MaintenanceTypeProductGroup>();

            maintenanceType.ProductString.Split(",").ToList().ForEach(p =>
            {
                var data = p.Split("|");
                products.Add(new MaintenanceTypeProductGroup()
                {
                    Product = new ProductViewModel() { Id = Convert.ToInt32(data.First()) },
                    Quantity = Convert.ToInt32(data.Last())
                });
            });

            var result = await _maintenanceTypeFactory.CreateMaintenanceType(maintenanceType, products);
            return result;
        }
    }
}
