using System;
using System.Collections.Generic;

namespace SSORequestApplication.Models
{
    public partial class Ssorequest
    {
        public int Id { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string AppName { get; set; }
        public string LaunchDate { get; set; }
        public string RequesterName { get; set; }
        public string RequesterPhone { get; set; }
        public string RequesterEmail { get; set; }
        public string RequesterDepartment { get; set; }
        public string TechnicalName { get; set; }
        public string TechnicalPhone { get; set; }
        public string TechnicalEmail { get; set; }
        public string TechnicalCompany { get; set; }
        public string AdminName { get; set; }
        public string AdminPhone { get; set; }
        public string AdminEmail { get; set; }
        public string AdminCompany { get; set; }
        public string Memorandum { get; set; }
        public string RestrictedData { get; set; }
        public string Reviewed { get; set; }
        public string Protocol { get; set; }
        public string BaseUrlDev { get; set; }
        public string BaseUrlProd { get; set; }
        public string AcsUrlDev { get; set; }
        public string AcsUrlProd { get; set; }
        public string EntityUrlProd { get; set; }
        public string EntityUrlDev { get; set; }
        public string MetadataDev { get; set; }
        public string MetadataProd { get; set; }
        public string MetadataXmlprod { get; set; }
        public string MetadataXmldev { get; set; }
        public string Samlinfo { get; set; }
        public string UniqueId { get; set; }
        public string UniqueIdAttr { get; set; }
        public string UniqueIdSpIn { get; set; }
        public bool ProdAndDev { get; set; }
        public string Guid { get; set; }
        public bool Finished { get; set; }
        public bool AdminAccess { get; set; }
        public bool Processed { get; set; }
        public bool TechSame { get; set; }
        public bool AdminSame { get; set; }
        public bool ReqAccessTech { get; set; }
        public bool MoreAttr { get; set; }
    }
}
