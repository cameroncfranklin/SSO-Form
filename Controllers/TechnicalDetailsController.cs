using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSORequestApplication.Models;
using SSORequestApplication.ViewModels;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;


namespace SSORequestApplication.Controllers
{   
    public class TechnicalDetailsController : Controller
    {
        private readonly ServicesRegistryContext _db;
        private readonly ILogger<TechnicalDetailsController> _logger;
        private readonly string _sendToLink;
        private readonly float _attrMult = 1.5f;
        private readonly Dictionary<string, string> _attrDict = new Dictionary<string, string> {
            {"sAMAccountName", "NetworkID"},
            {"givenName", "Full Name"},
            {"mail", "Email"},
            {"sn", "Last Name"},
            {"employeeID", "CWID"},
            {"memberOf", "Group Membership"},
            {"department", "Department"},
            {"pepRole1", "Role 1"},
            {"pepRole2", "Role 2"},
            {"custom", "Custom"}
        };

        public TechnicalDetailsController(ILogger<TechnicalDetailsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            // Retreives link from appsetting.json
            _sendToLink = configuration.GetSection("Links").GetSection("RequesterConfirmLink").Value;
            _db = new ServicesRegistryContext(configuration.GetConnectionString("SsoDb"));
        }

        private TechnicalDetailsViewModel CreateHelper(int id)
        {
            // Use the id to access the correct entry of the Ssorequest entry
            Ssorequest request = _db.Ssorequest.Find(id);
            // Instantiates a TechViewModel object with some necessary values for prepopulation, 
            // and display to the user
            TechnicalDetailsViewModel techView = new TechnicalDetailsViewModel
            {
                AppName = request.AppName,
                RequesterName = request.RequesterName,
                ReqAccessTech = request.ReqAccessTech,
                Protocol = request.Protocol,
                ProdAndDev = true,
                MoreAttr = false,
                AttrDict = _attrDict,
                AttrMult = _attrMult,
                FailVal = false,
            };
            return techView;
        }

        // Serves the user the TechForm View, See documentation \/\/\/
        // https://docs.google.com/document/d/1kfQrGrDTuxpPMHkoXXUEVAEV-Qw0f5Fg73bddhgTAcU/edit#heading=h.ekb5h8b3ikb
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            // Pull id from TempData. Id is no longer stored in TempData
            int id = (int)TempData["id"];
            // id is kept in TempData, so it can be pulled out in the HttpPost TechForm action
            TempData.Keep("id");
            // Sets a new TechnicalDetailsViewModel object using CreateHelper to setup the object
            TechnicalDetailsViewModel techView = CreateHelper(id);
            TempData["app"] = techView.AppName;
            TempData["req"] = techView.RequesterName;
            TempData["sendToLink"] = _sendToLink;
            // Calls the view with techView, so that the view can reverence the model
            return View(techView);
        }

        [AllowAnonymous]
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateLinked(int id)
        {
            // Sets a new TechnicalDetailsViewModel object using CreateHelper to setup the object
            TechnicalDetailsViewModel techView = CreateHelper(id);
            // Gives values to keys in TempData for use in the view
            TempData["app"] = techView.AppName;
            TempData["req"] = techView.RequesterName;
            // Calls the view with techView, so that the view can reference the model
            return View("Create", techView);
        }

        // This action is for redirecting Tech contacts who access the form via the emailed link
        [Route("technicaldetails/create/{guid}")]
        [AllowAnonymous]
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult TechRedirect(string guid)
        {
            // Uses the guid to access id, stores the id for the TechForm action, and redirects to that TechForm action.
            try
            {
                Guidindex index = _db.Guidindex.Find(guid);
                int id = index.Id;
                TempData["id"] = id;

                return CreateLinked(id);
            } catch
            {
                return RedirectToAction("Error", "Error", new { message = "Invalid GUID" });
            }
            
        }

        // Post method: when ModelState is valid, saves everything to _db. Saves to 2 different tables.
        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(TechnicalDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState.IsValid == TRUE");
                int id = (int)TempData["id"];
                // Finds correct entry in Ssorequest table using id, and id is taken out of TempData
                Ssorequest request = _db.Ssorequest.Find(id);

                // updates the table with all the data from the form
                request.Protocol = model.Protocol;
                request.BaseUrlDev = model.BaseUrlDev;
                request.BaseUrlProd = model.BaseUrlProd;
                request.AcsUrlProd = model.AcsUrlProd;
                request.AcsUrlDev = model.AcsUrlDev;
                request.EntityUrlDev = model.EntityUrlDev;
                request.EntityUrlProd = model.EntityUrlProd;
                request.MetadataDev = model.MetadataDev;
                request.MetadataProd = model.MetadataProd;
                request.MetadataXmlprod = model.MetadataXMLProd;
                request.MetadataXmldev = model.MetadataXMLDev;
                request.Samlinfo = model.SAMLInfo;
                request.ProdAndDev = model.ProdAndDev;
                request.UniqueId = model.UniqueId;
                request.UniqueIdAttr = model.UniqueIdAttr;
                request.UniqueIdSpIn = model.UniqueIdSpIn;
                request.MoreAttr = model.MoreAttr;

                if (model.MoreAttr == true)
                {
                    try
                    {
                        for (int x = 0; x < model.Attributes.Count; x++)
                        {
                            if (model.Attributes[x].Attr != null)// possibly unneeded check
                            {
                                AttributeRelease entry = new AttributeRelease();
                                _db.AttributeRelease.Add(entry);
                                entry.Id = id;
                                entry.Attribute = model.Attributes[x].Attr;
                                entry.Release = model.Attributes[x].AttrRel;
                                entry.SpecialInstructions = model.Attributes[x].AttrSpIn;
                                _db.SaveChanges();// Save Changes at end to save to row before loop creates new AttributeRelease()
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to add a AttributeRelease object to context or save changes to the database", e);
                    }
                }
                _db.SaveChanges();// data is saved to the server for all tables
                return RedirectToAction("technicaldetails", "confirmation");
            }
            _logger.LogInformation("ModelState.IsValid == FALSE");
            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
            _logger.LogDebug("ModelState errors: " + errors.ToString());
            // The following model attributes are set so that they can be accessed as view is reloaded.
            model.AttrMult = _attrMult;
            model.AttrDict = _attrDict;
            model.FailVal = true;
            return View(model);
        }
    }
}