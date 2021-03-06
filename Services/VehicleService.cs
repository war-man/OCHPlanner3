using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Enum;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleFactory _vehicleFactory;
        private readonly IConfiguration _configuration;
        private readonly IVINQueryService _vinQueryService;

        public VehicleService(IVehicleFactory vehicleFactory,
            IVINQueryService vinQueryService,
            IConfiguration configuration)
        {
            _vehicleFactory = vehicleFactory;
            _vinQueryService = vinQueryService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<SelectListItem>> GetCarMakeSelectList()
        {
            var makes = await _vehicleFactory.GetMakes();

            return makes.Select(x => new SelectListItem
            {
                Value = x.Make,
                Text = x.Make
            }).OrderBy(o => o.Text);
        }

        public async Task<VehicleViewModel> GetVehicleByVIN(string vin)
        {
            var vehicle = await _vehicleFactory.GetVehicleByVIN(vin);
            var result = new VehicleViewModel();

            if (vehicle != null)
            {
                result = vehicle.Adapt<VehicleViewModel>();

                result.Driver = new DriverViewModel()
                {
                    Notes = vehicle.DriverNotes,
                    CellPhone = vehicle.DriverCellphone,
                    Email = vehicle.DriverEmail,
                    Name = vehicle.DriverName,
                    Phone = vehicle.DriverPhone
                };

                result.Owner = new OwnerViewModel()
                {
                    Address = vehicle.OwnerAddress,
                    Company = vehicle.OwnerCompany,
                    Email = vehicle.OwnerEmail,
                    Name = vehicle.OwnerName,
                    Phone = vehicle.OwnerPhone
                };

                result.LicencePlate = vehicle.LicencePlate;
                result.SelectedMaintenancePlan = vehicle.MaintenancePlanId;
                result.OilTypeId = vehicle.OilTypeId;
            }
            else
            {
                //Get VIN Decode values
                var vinResult = await _vinQueryService.GetVINDecode(vin);

                result = new VehicleViewModel()
                {
                    VinCode = vinResult.VIN,
                    Description = vinResult.Description,
                    Year = Convert.ToInt32(vinResult.Year),
                    Make = vinResult.Make,
                    Model = vinResult.Model,
                    BrakeSystem = vinResult.BrakeSystem,
                    Engine = vinResult.Engine,
                    Seating = vinResult.Seating,
                    Steering = vinResult.Steering,
                    Propulsion = vinResult.DriveLine,
                    Transmission = vinResult.Transmission,
                    EntryDate = new DateTime(Convert.ToInt32(vinResult.Year),6,1).ToString()
                };
            }

            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetCarModelSelectList(string make)
        {
            var makes = await _vehicleFactory.GetModels(make);

            return makes.Select(x => new SelectListItem
            {
                Value = x.Model,
                Text = x.Model
            }).OrderBy(o => o.Text);
        }

        public async Task<IEnumerable<SelectListItem>> GetCarColorSelectList(string language)
        {
            var colors = new List<SelectListItem>();

            var carColor = _configuration.GetSection("CarColors");

            carColor.Value.Split(",").ToList().ForEach(c =>
            {
                var fr = c.Split("|").First();
                var en = c.Split("|").Last();

                colors.Add(new SelectListItem() { Value = language.ToUpper() == "FR" ? fr : en, Text = language.ToUpper() == "FR" ? fr : en });
            });

            return await Task.FromResult(colors.OrderBy(o => o.Text)).ConfigureAwait(false);
        }

        public async Task<int> SaveVehicle(VehicleViewModel vehicle)
        {
            if (vehicle.Id == 0)
            {
                return await _vehicleFactory.CreateVehicle(vehicle.Adapt<VehicleModel>());
            }
            else
            {
                return await _vehicleFactory.UpdateVehicle(vehicle.Adapt<VehicleModel>());
            }
        }
    }
}
