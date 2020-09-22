using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCHPlanner3.Models;

namespace OCHPlanner3.Controllers
{
    public class StickerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Simple()
        {
            var model = new StickerSimpleViewModel();

            model.OilList = new List<SelectListItem>
           {
              new SelectListItem { Value = "Item1", Text = "Item One" },
              new SelectListItem { Value = "Item2", Text = "Item Two" },
              new SelectListItem { Value = "Item3", Text = "Item Three" },
           };

            return View(model);
        }
    }
}