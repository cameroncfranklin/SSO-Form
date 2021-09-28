using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSORequestApplication.Models;
using SSORequestApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SSORequestApplication.Controllers
{
    public class SSORequestController : Controller
    {
        private ServicesRegistryContext _db;
        private readonly ILogger<SSORequestController> _logger;

        public SSORequestController(ILogger<SSORequestController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected");
            _db = new ServicesRegistryContext(configuration.GetConnectionString("SsoDb"));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {

            return View();
        }

        // Run when user clicks the "create" button in Create View
        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(SSORequestViewModel model)// model is filled with the input data from the user
        {            
            if (ModelState.IsValid) // If the model's requirements were met this is True, othewise False
            {   // requirements for ModelState found as annotations on objects/properties of the SSORequestViewModel
                // If the model is filled correctly, the following code runs and saves the data to the server

                Ssorequest request = new Ssorequest();
                _logger.LogInformation("ModelState valid, before '_db.Ssorequest.Add'");
                _db.Ssorequest.Add(request);
                _logger.LogInformation("ModelState valid, after '_db.Ssorequest.Add'");
                string[] splitter = { "(", ") ", "-"}; // list of strings to be used to split phone numbers for reformating.

                // Requester info other than phone number are hard coded to get the CAS values, regardless of user fiddling.
                request.RequesterName = User.Claims.Where(c => c.Type == "displayName").Select(c => c.Value).SingleOrDefault();
                request.RequesterDepartment = User.Claims.Where(c => c.Type == "department").Select(c => c.Value).SingleOrDefault();
                request.RequesterEmail = User.Claims.Where(c => c.Type == "mail").Select(c => c.Value).SingleOrDefault();
                // splits and rebuilds phone number with ###-###-#### format
                string[] split = model.RequesterPhone.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                request.RequesterPhone = split[0] + '-' + split[1] + '-' + split[2];

                request.TechnicalName = model.TechnicalName.Trim();
                request.TechnicalCompany = model.TechnicalCompany.Trim();
                request.TechnicalEmail = model.TechnicalEmail.Trim();
                // splits and rebuilds phone number with ###-###-#### format
                split = model.TechnicalPhone.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                request.TechnicalPhone = split[0] + '-' + split[1] + '-' + split[2];

                request.AdminName = model.AdminName.Trim();
                request.AdminCompany = model.AdminCompany.Trim();
                request.AdminEmail = model.AdminEmail.Trim();
                // splits and rebuilds phone number with ###-###-#### format
                split = model.AdminPhone.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                request.AdminPhone = split[0] + '-' + split[1] + '-' + split[2];

                request.AppName = model.AppName.Trim();
                request.LaunchDate = model.LaunchDate.Trim();

                request.Protocol = model.Protocol;
                request.RestrictedData = model.RestrictedData;
                request.Memorandum = model.Memorandum;
                request.Reviewed = model.Reviewed;

                request.TechSame = model.TechSame;
                request.AdminSame = model.AdminSame;

                request.SubmittedOn = DateTime.Now;

                // A guid is generated and saved on the server as a string
                string g = Guid.NewGuid().ToString();
                request.Guid = g;
                try
                {
                    _db.SaveChanges(); // Changes are saved, this is the point where the primary key "Id" is generated
                } catch (Exception e)
                {
                    _logger.LogError("SSORequest form data couldn't be saved to database.", e);
                }

                // This if checks if the user is making a duplicate request, and removes if if found
           
                try
                {
                    if (request.AppName == _db.Ssorequest.Find(request.Id - 1).AppName)
                    {
                        _logger.LogInformation("SSORequest/Create POST: Removing duplicate request entry");
                        _db.Ssorequest.Remove(_db.Ssorequest.Find(request.Id - 1));
                        _db.Guidindex.Remove(_db.Guidindex.Find(TempData["guid"].ToString()));
                        _db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("There is no request.Id - 1. This is the first request, or previous was deleted already. Exception: " + e);
                }
                

                // TempData is used to store the id for use by the next action/view
                TempData["id"] = request.Id;
                TempData["guid"] = g;

                // A new entry in the Guidindex table is created, with the guid and the id
                // For info on the implementation of this table see: 
                Guidindex index = new Guidindex { Guid = g, Id = request.Id };
                
                try
                {
                    _db.Guidindex.Add(index);
                    _db.SaveChanges();
                } catch (Exception e)
                {
                    string inGuidIndex = "GuidIndex object has Guid = " + g.ToString() + ", and Id = " + request.Id.ToString();
                    _logger.LogError(
                        "Could not add new GuidIndex object to context, or could not save changes to GuidIndex table on database. " +
                        inGuidIndex, e);
                }

                if (request.TechSame == false) // if the Requester is not the same as technical contact
                {
                    if (request.Protocol == "unknown") // if the Requester doesn't know what the protocol is
                    {
                        // Redirects to the RequesterConfirm view via it's action in ConfirmationController
                        return RedirectToAction("ssorequest", "confirmation");
                    }
                    // The Requester does know the protocol, and will be asked for more technical details
                    request.ReqAccessTech = true;
                    _db.SaveChanges();
                    // Redirects to the TechForm view via it's action in TechContactController
                    return RedirectToAction("create", "technicaldetails");
                }
                return RedirectToAction("create", "technicaldetails");
            }
            _logger.LogInformation("ModelState is not valid");
            // If the ModelState is invalid, this line will run and the view will be re-loaded
            // Error messages will be displayed (e.g. "Please enter a valid phone number")
            return View(model);
        }

        [AllowAnonymous]
        [Route("login")]
        public async Task Login(string returnUrl)
        {
            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            await HttpContext.ChallengeAsync("CAS", props);
        }
    }
}