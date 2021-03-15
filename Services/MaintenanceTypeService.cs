using Mapster;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
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
        private readonly IOptionFactory _optionFactory;

        public MaintenanceTypeService(IMaintenanceTypeFactory maintenanceTypeFactory,
            IOptionFactory optionFactory)
        {
            _maintenanceTypeFactory = maintenanceTypeFactory;
            _optionFactory = optionFactory;
        }

        public async Task<int> CreateMaintenanceType(MaintenanceTypeViewModel maintenanceType)
        {
            var products = new List<MaintenanceTypeProductGroupViewModel>();

            if (!string.IsNullOrWhiteSpace(maintenanceType.ProductString))
            {
                maintenanceType.ProductString.Split(",").ToList().ForEach(p =>
                {
                    var data = p.Split("|");
                    products.Add(new MaintenanceTypeProductGroupViewModel()
                    {
                        Product = new ProductViewModel() { Id = Convert.ToInt32(data.First()) },
                        Quantity = Convert.ToInt32(data.Last())
                    });
                });
            }

            var maintenanceTypeModel = maintenanceType.Adapt<MaintenanceTypeModel>();
            var result = await _maintenanceTypeFactory.CreateMaintenanceType(maintenanceTypeModel, products);
            return result;
        }

        public async Task<int> EditMaintenanceType(MaintenanceTypeViewModel maintenanceType)
        {
            var products = new List<MaintenanceTypeProductGroupViewModel>();

            maintenanceType.ProductString.Split(",").ToList().ForEach(p =>
            {
                var data = p.Split("|");
                products.Add(new MaintenanceTypeProductGroupViewModel()
                {
                    Product = new ProductViewModel() { Id = Convert.ToInt32(data.First()) },
                    Quantity = Convert.ToInt32(data.Last())
                });
            });

            var maintenanceTypeModel = maintenanceType.Adapt<MaintenanceTypeModel>();
            var result = await _maintenanceTypeFactory.EditMaintenanceType(maintenanceTypeModel, products);
            return result;
        }

        public async Task<int> Delete(int id)
        {
            return await _maintenanceTypeFactory.Delete(id);
        }

        public async Task<MaintenanceTypeViewModel> GetMaintenanceType(int id)
        {
            var result = await _maintenanceTypeFactory.GetMaintenanceType(id);
            return result.Adapt<MaintenanceTypeViewModel>();
        }

        public async Task<IEnumerable<MaintenanceTypeViewModel>> GetMaintenanceTypes(int garageId)
        {
            var result = await _maintenanceTypeFactory.GetMaintenanceTypes(garageId);
            return result.Adapt<IEnumerable<MaintenanceTypeViewModel>>();
        }

        public async Task<IList<MaintenanceTypeProductGroupViewModel>> GetSelectedProducts(int id)
        {
            var result = new List<MaintenanceTypeProductGroupViewModel>();

            var productModel = await _maintenanceTypeFactory.GetSelectedProducts(id);

            foreach (var p in productModel)
            {
                var product = await _optionFactory.GetProduct(p.ProductId);

                result.Add(new MaintenanceTypeProductGroupViewModel()
                {
                    Product = product.Adapt<ProductViewModel>(),
                    Quantity = p.Quantity
                });
            }

            return result;
        }
    }
}
