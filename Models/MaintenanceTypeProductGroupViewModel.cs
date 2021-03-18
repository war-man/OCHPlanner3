using System.Globalization;

namespace OCHPlanner3.Models
{
    public class MaintenanceTypeProductGroupViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }

        public string CostPriceTotal
        {
            get
            {
                var culture = CultureInfo.CreateSpecificCulture("en-CA");

                decimal cost = decimal.Parse(Product.CostPrice, CultureInfo.InvariantCulture);
                var result = Quantity * cost;
                return result.ToString(culture);
            }
        }

        public string RetailPriceTotal
        {
            get
            {
                var culture = CultureInfo.CreateSpecificCulture("en-CA");

                decimal retail = decimal.Parse(Product.RetailPrice, CultureInfo.InvariantCulture);
                var result = Quantity * retail;
                return result.ToString(culture);
            }
        }
    }
}
