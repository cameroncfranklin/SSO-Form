using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSORequestApplication.Models
{
    public class DisplayModel
    {
        public int NumRequests { get; set; }
        public IList<Ssorequest> Requests { get; set; }
    }
}
