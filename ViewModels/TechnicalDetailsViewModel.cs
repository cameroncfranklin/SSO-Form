using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSORequestApplication.ViewModels
{
    public class TechnicalDetailsViewModel
    {// Documentation: https://docs.google.com/document/d/1kfQrGrDTuxpPMHkoXXUEVAEV-Qw0f5Fg73bddhgTAcU/edit?pli=1#heading=h.nttxcx2b0q6
        public Dictionary<string, string> AttrDict { get; set; }
        public float AttrMult { get; set; }
        public string AppName { get; set; }

        public string RequesterName { get; set; }

        public bool ReqAccessTech { get; set; }

        [Required] // Required in order to ensure the SAML section is filled out.
        public string SAMLInfo { get; set; }

        [Required]
        [Display(Name = "Base Development URL")]
        public string BaseUrlDev { get; set; }

        [Required]
        [Display(Name = "Base Production URL")]
        public string BaseUrlProd { get; set; }

        [Required]
        [Display(Name = "ACS Development URL")]
        public string AcsUrlDev { get; set; }

        [Required]
        [Display(Name = "ACS Production URL")]
        public string AcsUrlProd { get; set; }

        [Required]
        [Display(Name = "Entity ID Development URL")]
        public string EntityUrlDev { get; set; }

        [Required]
        [Display(Name = "Entity ID Production URL")]
        public string EntityUrlProd { get; set; }

        [Required]
        public string Protocol { get; set; }

        [Required]
        public bool ProdAndDev { get; set; }

        [Required]
        [Display(Name = "Developer Metadata")]
        public string MetadataDev { get; set; }

        [Required]
        [Display(Name = "Production Metadata")]
        public string MetadataProd { get; set; }

        [Required]
        [Display(Name = "SAML SP XML Prod")]
        public string MetadataXMLProd { get; set; }

        [Required]
        [Display(Name = "SAML SP XML Dev")]
        public string MetadataXMLDev { get; set; }

        [Required]
        [Display(Name = "Unique ID")]
        public string UniqueId { get; set; }

        [Required]
        [Display(Name = "Unique ID Attribute")]
        public string UniqueIdAttr { get; set; }

        [Display(Name = "Unique ID Special Instructions")]
        public string UniqueIdSpIn { get; set; }

        [Required]
        public bool MoreAttr { get; set; }

        public IList<ReleaseInfo> Attributes { get; set; }
        public bool FailVal { get; set; }
    }

    public class ReleaseInfo
    {
        public string Attr { get; set; }
        public string AttrRel { get; set; }
        public string AttrSpIn { get; set; }
    }
}
