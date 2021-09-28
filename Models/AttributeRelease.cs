using System;
using System.Collections.Generic;

namespace SSORequestApplication.Models
{
    public partial class AttributeRelease
    {
        public int Pk { get; set; }
        public int? Id { get; set; }
        public string Attribute { get; set; }
        public string Release { get; set; }
        public string SpecialInstructions { get; set; }
    }
}
