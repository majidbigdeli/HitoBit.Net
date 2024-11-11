using System.Diagnostics;
using Asp.Net.Models;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Asp.Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHitoBitDataProvider _dataProvider;

        public HomeController(IHitoBitDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            return View(_dataProvider.LastKline);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
