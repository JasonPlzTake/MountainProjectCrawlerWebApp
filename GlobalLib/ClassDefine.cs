namespace GlobalLib
{
    public class RouteInfo
    {
        // this class includes route info
        // and method to access route info by visiting route website
        public string routeName;
        public string routeLink;
        public string routeGrade;
        public string routeLocation;

        public RouteInfo(string routeName, string routeGrade, string routeLocation, string routeLink)
        {
            // todo: this function can be customized/configured if not all below information is needed
            this.routeLink = routeLink;
            this.routeName = routeName;
            this.routeGrade = routeGrade;
            this.routeLocation = routeLocation;
        }
    }
}