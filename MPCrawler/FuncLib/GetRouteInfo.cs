using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpCrawler
{
    // This class includes below functions for collecting route information from route webpage
    // It uses basic search functions defined in "FuncLib.HtmlFind"
    //
    // string GetRouteName(HtmlText htmlText)
    // string GetRouteGrade(HtmlText htmlText)
    // List<string> GetRouteLocation(HtmlText htmlText)

    public class GetRouteInfo
    {
        public static string GetRouteName(HtmlText htmlText)
        {
            // Function:
            //          target section is as below example:
            //          < title > Climb Horse Fly, Olympics & amp; Pacific Coast < / title >
            //          1) find the first title and split at first ','
            //          2) remove "<title> Climb"
            //
            // Todo:
            //          how to handle some char like " 's "is &#039;
            //          exp. https://www.mountainproject.com/route/106806101/kellys-fly
            //
            return HtmlFind.HtmlFindAllTags(htmlText.htmlText, "title")[0].Split(',')[0]["<title>Climb ".Length..]; 
        }


        public static string GetRouteGrade(HtmlText htmlText)
        {
            //Function:

            //target area is as below example:

            //    < h2 class ="inline-block mr-2" >
            //      < span class ='rateYDS' > V6
            //          <a href="https://www.mountainproject.com/international-climbing-grades" class ="font-body" >
            //              < span class ="small" > YDS
            //              < / span>
            //          < / a >
            //      < / span >
            //      < span class ='rateFont' > 7A
            //          <a href="https://www.mountainproject.com/international-climbing-grades" class ="font-body" >
            //              < span class ="small" > Font
            //              < / span>
            //          < / a >
            //      < / span >
            //    < / h2 >
            
            List<string> gradeSection =  HtmlFind.HtmlFindAllTags(htmlText.htmlText, "h2", "inline-block mr-2");
            string routeGradeLine = HtmlFind.HtmlFindAllTags(htmlText.htmlText, "span", "class='rateYDS'")[0];
            return routeGradeLine.Split("'rateYDS'>")[1].Split(" <a href=")[0];
        }

        public static string GetRouteLocation(HtmlText htmlText)
        {
            // Function:
            //          search all<a> tag which shall include location link following below sequence
            //          [parent location1 link, parent location2 link, ..., finest area location link, boulder link]
            // Todo:
            //          are there any location name originally with '-' ?
            //
            //    Target location area is as below example:
            //
            //    < div class ="mb-half small text-warm" >
            //        < a href="https://www.mountainproject.com/route-guide" > All Locations< / a>
            //        & gt;
            //        < a href="https://www.mountainproject.com/area/105708966/washington" > Washington < / a >
            //        &gt;
            //        < a href="https://www.mountainproject.com/area/108471374/central-west-cascades-seattle" > Central - W Casca & hellip; < / a >
            //        & gt;
            //        < a href="https://www.mountainproject.com/area/108471672/skykomish-valley" > Skykomish Valley< / a>
            //        & gt;
            //        < a href="https://www.mountainproject.com/area/105805788/gold-bar-boulders" > Gold Bar Boulders< / a >
            //        & gt;
            //        < a href="https://www.mountainproject.com/area/105970461/zekes-trail-boulders" > Zeke &  # 039;s Trail Bo&hellip;</a>
            //        &gt;
            //        < a href="https://www.mountainproject.com/area/118994021/jaws-boulder" > Jaws Boulder< / a>
            //    < / div >

            StringBuilder locationStr = new StringBuilder();
            string locationSection = HtmlFind.HtmlFindAllTags(htmlText.htmlText, "div", "class=\"mb-half small text-warm\"")[0];
            List<string> aTagList = HtmlFind.HtmlFindAllTags(locationSection, "a");
            int index = 0;

            foreach (string aTag in aTagList)
            {
                index += 1;
                if (index <= 1)
                {
                    continue;
                }
                string locationUrl = HtmlFind.GetHyperLinks(aTag, "href=\"");
                locationStr.Append(locationUrl.Split("/").Last().Replace('-', ' ')); // take the last section and replace '-' with space
                locationStr.Append("->");
            }

            locationStr.Remove(locationStr.Length - 2, 2); // remove last ->

            return locationStr.ToString();
        }
}
}
