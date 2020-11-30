using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OCHPlanner3.Controllers
{
    public class PrintController : Controller
    {
        public IActionResult Test()
        {
            return View();
        }

        [HttpGet("/{lang:lang}/print/center")]
        public int Center(string word)
        {
            var totalSpace = 425;
            var spaceForOneLetter = 13;
            var letterTotal = word.Length;

            var spaceNeeded = letterTotal * spaceForOneLetter;
            var left = (totalSpace - spaceNeeded) / 2;

            return left; 
        }
    }
}