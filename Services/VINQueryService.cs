using Microsoft.Extensions.Configuration;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace OCHPlanner3.Services
{
    public class VINQueryService : IVINQueryService
    {
        private readonly IConfiguration _configuration;

        private string VINQueryUrl { get; set; }
        private string VINQueryKey { get; set; }
 
        private const string GETVINDECODE_URL = "?reportType=0&vin={0}";

        public VINQueryService(IConfiguration configuration)
        {
            _configuration = configuration;
            VINQueryUrl = _configuration.GetSection("VINQuery_Url").Value;
            VINQueryKey = _configuration.GetSection("VINQuery_Key").Value;
        }

        public async Task<VINDetailResultViewModel> GetVINDecode(string vin)
        {
            var result = new VINDetailResultViewModel();

            result.VIN = vin;

            var vinDecode = await GetVINDetail(vin.ToUpper());

            if (!string.IsNullOrWhiteSpace(vinDecode.Engine))
            {
                var extract = vinDecode.Engine.Substring(0, vinDecode.Engine.IndexOf("L"));

                try
                {
                    double engine;
                    double.TryParse(extract, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out engine);
                    vinDecode.Engine = engine != 0 ? extract : string.Empty;
                }
                catch (Exception)
                {
                    vinDecode.Engine = string.Empty;
                }
            }

            return vinDecode;
        }

        private async Task<VINDetailResultViewModel> GetVINDetail(string vin)
        {
            var url = string.Format(GETVINDECODE_URL, vin);
            return await GetItem(url);
        }

        protected async Task<HttpClient> GetHttpClient()
        {
            var client = new HttpClient();
            //set basic info of the httpClient to be used
            client.BaseAddress = new Uri(VINQueryUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            return client;
        }


        private async Task<bool> ValidateResponse(HttpResponseMessage response)
        {
            if (response == null) return false;
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return false;

            return true;
        }
      
        protected async Task<VINDetailResultViewModel> GetItem(string url)
        {
            var xdoc = new XmlDocument();
            var result = new VINDetailResultViewModel();

            //Call KApi API via HttpClient
            using (var client = await GetHttpClient())
            {
                url += string.Format("&accesscode={0}", VINQueryKey);

                var xml = client.GetStringAsync(url).Result;

                xdoc.LoadXml(xml);
                var status = xdoc.SelectSingleNode("/VINquery/VIN/@Status").Value;

                if (status == "SUCCESS")
                {
                    result.VIN = xdoc.SelectSingleNode("/VINquery/VIN/@Number").Value;
                    result.Year = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/@Model_Year").Value;
                    result.Make = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/@Make").Value;
                    result.Model = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/@Model").Value;
                    result.Engine = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/Item[@Key='Engine Type']/@Value").Value;
                    result.Transmission = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/Item[@Key='Transmission-short']/@Value").Value;
                    result.DriveLine = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/Item[@Key='Driveline']/@Value").Value;
                    result.BrakeSystem = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/Item[@Key='Anti-Brake System']/@Value").Value;
                    result.Steering = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/Item[@Key='Steering Type']/@Value").Value;
                    result.Seating = xdoc.SelectSingleNode("/VINquery/VIN/Vehicle/Item[@Key='Standard Seating']/@Value").Value;
                }

                return result;
            }
        }

    }
}
