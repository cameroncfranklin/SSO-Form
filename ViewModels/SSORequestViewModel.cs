using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSORequestApplication.ViewModels
{
    public class SSORequestViewModel
    {
        [Required]
        public string AppName { get; set; }

        [Required(ErrorMessage = "Please enter a Launch Date")]
        [Display(Name = "Launch Date")]
        public string LaunchDate { get; set; }

        [Display(Name = "Requester Name")]
        public string RequesterName { get; set; }

        [Required]
        [Display(Name = "Requester Phone")]
        [RegularExpression(@"[2-9]\d{2}-\d{3}-\d{4}|\([2-9]\d{2}\)\s\d{3}-\d{4}",
            ErrorMessage = "Please enter a valid phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string RequesterPhone { get; set; }

        [Display(Name = "Requester Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string RequesterEmail { get; set; }

        [Display(Name = "Requester Department")]
        public string RequesterDepartment { get; set; }

        [Required]
        [Display(Name = "Tech Contact Name")]
        public string TechnicalName { get; set; }

        [Required]
        [Display(Name = "Tech Contact Company")]
        public string TechnicalCompany { get; set; }

        [Required]
        [Display(Name = "Tech Contact Phone")]
        [RegularExpression(@"[2-9]\d{2}-\d{3}-\d{4}|\([2-9]\d{2}\)\s\d{3}-\d{4}",
            ErrorMessage = "Please enter a valid phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string TechnicalPhone { get; set; }

        [Required]
        [Display(Name = "Tech Contact Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string TechnicalEmail { get; set; }

        [Required]
        [Display(Name = "Admin Contact Name")]
        public string AdminName { get; set; }

        [Required]
        [Display(Name = "Admin Contact Company")]
        public string AdminCompany { get; set; }

        [Required]
        [Display(Name = "Admin Contact Phone")]
        [RegularExpression(@"[2-9]\d{2}-\d{3}-\d{4}|\([2-9]\d{2}\)\s\d{3}-\d{4}",
            ErrorMessage = "Please enter a valid phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string AdminPhone { get; set; }

        [Required]
        [Display(Name = "Admin Contact Email")]
        [EmailAddress(ErrorMessage = "Invalid Admin Contact email address")]
        public string AdminEmail { get; set; }

        [Required]
        [Display(Name = "Restricted Data")]
        public string RestrictedData { get; set; }

        [Required]
        [Display(Name = "Memorandum of Understanding")]
        public string Memorandum { get; set; }

        [Required]
        public string Reviewed { get; set; }

        [Required]
        public string Protocol { get; set; }

        public bool TechSame { get; set; }

        public bool AdminSame { get; set; }

        public bool ReqAccessTech { get; set; }
    }
}
