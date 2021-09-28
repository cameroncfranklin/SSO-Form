using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSORequestApplication.HelperClasses;
using SSORequestApplication.Models;


namespace SSORequestApplication.Controllers
{
    public class ConfirmationController : Controller
    {
        private ServicesRegistryContext _db;
        private readonly ILogger<SSORequestController> _logger;
        private readonly EmailHelper _emailHelper;
        // This is the base of the URL sent to the technical contact for the technical details form (without GUID)
        private readonly string _techDetailsURL;

        public ConfirmationController(ILogger<SSORequestController> logger, EmailHelper emailHelper, IConfiguration configuration)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected");

            _emailHelper = emailHelper;
            _db = new ServicesRegistryContext(configuration.GetConnectionString("SsoDb"));
            _techDetailsURL = configuration.GetSection("Links").GetSection("TechAccessLink").Value;
        }

        //Action for when the Requestor isn't the Technical Contact, and is sent to this view from the Create view
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult SSORequest()
        {
            //Pull integer out of TempData, is no longer in TempData
            int id = (int)TempData["id"];
            Ssorequest request = _db.Ssorequest.Find(id);
            request.ReqAccessTech = false;
            _db.SaveChanges();

            //Allows the user to have the name of their Application appear in the confirmation page
            ViewBag.AppName = request.AppName;

            //This EmailModel instance is for a confirmation email for the Requestor when they designated a Tech Contact
            var requestorEmail = new EmailModel(
                request.RequesterEmail, // To  
                $"SSO Request Received ({request.AppName})", // Subject
@$"Dear {request.RequesterName},

Thank you for submitting an SSO Request for the {request.AppName} application.


Thank you,
Development team", // Message

                false // IsBodyHTML
            );
            _emailHelper.SendEmail(requestorEmail);

            //This EmailModel is to alert the Tech Contact that they need to fill out the TechForm
            var techContactEmail = new EmailModel(
                request.TechnicalEmail, // To  
                request.RequesterEmail, // CC
                $"Pepperdine SSO Technical Information Request ({request.AppName})", // Subject

$@"Dear {request.TechnicalName},

{request.RequesterName} has identified you as the SSO technical contact for the {request.AppName}
application. Please provide protocol and attribute information about this application 
using the following link: 
{_techDetailsURL + request.Guid}

Thank you,
Development Team", // Message

                false // IsBodyHTML  
            );
            _emailHelper.SendEmail(techContactEmail); // uses SendEmailCC, a variant made that includes a CC

            return View(request);
        }

        [AllowAnonymous]
        //Confirmation view for the TechContact who is not the same person as the Requestor
        public IActionResult TechnicalDetails()
        {
            int id = (int)TempData["id"];
            TempData.Keep("id");// included if the user presses "back" in their browser, allows the page to load
            Ssorequest request = _db.Ssorequest.Find(id);
            ViewBag.AppName = request.AppName;
            EmailModel email;

            if (request.TechSame) // True if technical contact is the same as the requestor
            {
                email = new EmailModel(
                    request.TechnicalEmail, // To
                    $"SSO Request Received ({request.AppName})", // Subject

@$"Dear {request.TechnicalName},

Thank you for submitting an SSO Request for the {request.AppName} application. Someone from the
Application Development team will reach out to you to establish a timeline for completing your request.


Thank you,
Development Team",
                    false // IsBodyHTML  
                );
            } else if (request.ReqAccessTech) // else if the requestor isn't the tech but did access the techform
            {
                // This EmailModel instance is for a confirmation email for the Requestor when they accessed the techform
                // BUT didn't check the Tech is same as Req
                email = new EmailModel(
                    request.RequesterEmail, // To  
                    $"SSO Request Received ({request.AppName})", // Subject

$@"Dear {request.RequesterName},

Thank you for submitting an SSO Request for the {request.AppName} application. Someone from the
Application Development team will reach out to you to establish a timeline for completing your request.


Thank you,
Development Team",
                    false // IsBodyHTML  
                );
            } else//an email is sent assuming the user is solely the technical contact
            {
                email = new EmailModel(
                    request.TechnicalEmail, // To  
                    $"Pepperdine SSO Technical Information Request ({request.AppName})", // Subject  
$@"Dear {request.TechnicalName},

Thank you for providing the technical details for the {request.AppName} application.


Thank you,
Development Team", // Message  
                    false // IsBodyHTML  
                );
            }
            _emailHelper.SendEmail(email);
            request.Finished = true;
            _db.SaveChanges();
            return View();
        }
    }
}