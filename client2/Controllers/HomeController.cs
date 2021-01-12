using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using client2.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;

namespace client2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var apiClient = new HttpClient();

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync("https://localhost:5666/api/Values/UserClaims");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Content = "Error Statuscode= " + response.StatusCode;
            }
            else
            {
                ViewBag.Content = await response.Content.ReadAsStringAsync();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
