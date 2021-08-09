using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class SearchModel
    {
        public string location { get; set; }
        public string gradingLow { get; set; }
        public string gradingHigh { get; set; }
        public string keyWords { get; set; }
    }
}
