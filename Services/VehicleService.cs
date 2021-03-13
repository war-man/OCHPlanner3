using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Data.Models;
using OCHPlanner3.Enum;
using OCHPlanner3.Helper.Comparer;
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
        private readonly IProgramService _programService;

        public VehicleService(IVehicleFactory vehicleFactory,
            IVINQueryService vinQueryService,
            IProgramService programService,
            IConfiguration configuration)
        {
            _vehicleFactory = vehicleFactory;
            _vinQueryService = vinQueryService;
            _configuration = configuration;
            _programService = programService;
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

        public async Task<VehicleViewModel> GetVehicleByVIN(string vin, int garageId)
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
                    Phone = vehicle.OwnerPhone,
                    OwnerList = await GetOwnerSelectListItem(garageId)
                };

                result.LicencePlate = vehicle.LicencePlate;
                result.SelectedMaintenancePlan = vehicle.MaintenancePlanId;
                result.OilTypeId = vehicle.OilTypeId;
                result.EntryDate = DateTime.Parse(vehicle.EntryDate).ToString("dd/MM/yy");

                result.Programs = await GetVehiclePrograms(vehicle.Id, garageId);
            }
            else
            {
                //Get VIN Decode values
                var vinResult = await _vinQueryService.GetVINDecode(vin);
                if (vinResult.VIN == null) return new VehicleViewModel();

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
                    EntryDate = new DateTime(Convert.ToInt32(vinResult.Year), 6, 1).ToString("dd/MM/yy")
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

        public async Task<int> SaveVehicle(VehicleViewModel vehicle, int garageId)
        {
            var vehicleModel = vehicle.Adapt<VehicleModel>();
            vehicleModel.VehicleProgram = await GetProgramVehicleList(vehicle);
            if (vehicle.Id == 0)
            {
                return await _vehicleFactory.CreateVehicle(vehicleModel, garageId);
            }
            else
            {
                return await _vehicleFactory.UpdateVehicle(vehicleModel);
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetOwnerSelectListItem(int garageId, int selectedId = 0)
        {
            var owners = await _vehicleFactory.GetOwnerList(garageId);
            return BuildOwnerSelectListItem(owners.OrderBy(x => x.Name, new SemiNumericComparer()), selectedId);
        }

        public async Task<IEnumerable<VehicleProgramModel>> GetProgramVehicleList(VehicleViewModel vehicle)
        {
            if (string.IsNullOrWhiteSpace(vehicle.SelectedPrograms)) return new List<VehicleProgramModel>();

            var programList = new List<VehicleProgramModel>();
            vehicle.SelectedPrograms.Split('|').ToList().ForEach(p =>
            {
                programList.Add(new VehicleProgramModel()
                {
                    ProgramId = Convert.ToInt32(p.Split(',').First()),
                    VehicleId = vehicle.Id,
                    Note = p.Split(',').LastOrDefault()
                });
            });

            return programList;
        }

        public async Task<IEnumerable<ProgramViewModel>> GetVehiclePrograms(int vehicleId, int garageId)
        {
            var vehicleProgram = await _vehicleFactory.GetVehiclePrograms(vehicleId);
            return FlagSelectedPrograms(await _programService.GetPrograms(garageId), vehicleProgram.Adapt<IEnumerable<VehicleProgramViewModel>>());
        }

        private IEnumerable<SelectListItem> BuildOwnerSelectListItem(IEnumerable<OwnerModel> ownerList, int selectedId = 0)
        {
            return ownerList.Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = x.Name,
                Selected = selectedId != 0 && selectedId == x.Id
            });
        }

        private IEnumerable<ProgramViewModel> FlagSelectedPrograms(IEnumerable<ProgramViewModel> programs, IEnumerable<VehicleProgramViewModel> vehiclePrograms)
        {
            var result = new List<ProgramViewModel>();

            programs.ToList().ForEach(pr =>
            {
                if (vehiclePrograms.Any(vp => vp.ProgramId == pr.Id))
                {
                    pr.Note = vehiclePrograms.First(k => k.ProgramId == pr.Id).Note;
                    pr.Selected = true;
                    result.Add(pr);
                }
                else
                {
                    result.Add(pr);
                }
            });

            return result;
        }

    }
}
