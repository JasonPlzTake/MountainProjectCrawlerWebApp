using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppDataLib.Models
{
    public class RouteInfo
    {
        // if in the future writing from web app then it shall extended with {get; set;}
        public string routeName;
        public string routeGrade;
        public string routeLocation;
        public string routeLink;

        public RouteInfo(string routeName, string routeGrade, string routeLocation, string routeLink)
        {
            this.routeName = routeName;
            this.routeGrade = routeGrade;
            this.routeLocation = routeLocation;
            this.routeLink = routeLink;
        }
    }
}
