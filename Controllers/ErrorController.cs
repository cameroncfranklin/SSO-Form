using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSORequestApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace SSORequestApplication.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly Dictionary<string, string> _errorTypes = new Dictionary<string, string>() {
            { "Error", "An error occurred while processing your request. Please contact appdev@sampleuniversity.edu for assistance with your request." },
            {"Invalid GUID", "There is no SSO Request with that ID.Please contact appdev@sampleuniversity.edu for assistance." },
        };

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // An error view, unchanged from what was supplied by VS template for .NET Core MVC.
        [Route("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message = "Error")
        {
            _logger.LogInformation("Error action has been entered with message: " + message);
            try
            {
                return View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Header = message,
                    Message = _errorTypes[message]
                });
            } catch
            {
                return View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Header = "Error",
                    Message = _errorTypes["Error"]
                });
            }
        }

        [Route("/error/{0}")]
        public IActionResult ErrorWithCode(int code) {
            return View(code.ToString());
        }

    }
}
