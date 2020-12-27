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

        public VehicleService(IVehicleFactory vehicleFactory,
            IConfiguration configuration)
        {
            _vehicleFactory = vehicleFactory;
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
    }
}
