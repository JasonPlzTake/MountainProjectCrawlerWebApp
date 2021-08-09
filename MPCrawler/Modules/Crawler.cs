using System;
using System.Collections.Generic;
using System.Text;
using WebAppDataLib.Models;


namespace MpCrawler.Modules
{
    public static class Crawler
    {
        // The MAIN crawler function 'MpBoulderRouteCrawler'
        // 1) first call 'GenSearchUrlList' to 'generate all search url for specified area'
        // 2) For each search url, collect all qualified route links
        // 3) For each route link, visit route page to collect route information and saved it into a list
        //
        // Note:
        //       1) Since the search page can only show first 1000 result,
        //          the search url is generated per each grade to prevent the qualified routes over 1000,
        //          which means for one state, it has 18 search url reprsenting vB - v17
        //       2) For routes with grade like v0-v1, it can be found in two search url, in this way,
        //          using a hash set containing route links to monitor if the current route is visited.
        //

        public static List<RouteInfo> MpBoulderRouteCrawler(string locationCode)
        {
            // Function:
            //          Collect route informaton for the state specified (locationCode)
            //
            //          1) Since each url page includes 1000 routes as max, splite state route
            //             search based on grade ensures each url includes less than 1000 problem. 
            //
            //          2) Error message will be given if one url page includes 1000 routes.
            //
            //          3) hash set is used to check if same route exsit in different url since
            //             route grading can be v10-v11 for instance.
            //
            // Inputs: 
            //          locationCode (ID represents a state)
            //
            
            List<RouteInfo> routeInfoList = new List<RouteInfo>();
            HashSet<String> linkSet = new HashSet<string>();

            // get search url list for the specified location for each v grade
            List<string> urlList = GenSearchUrlList(locationCode);

            // index is used for exception error message to indicate which grade exceed 1000 route limit
            int index = 0;
            foreach(string url in urlList)
            {
                // call crawling function for each search url
                CrawlePerGrade(url, routeInfoList, linkSet, index);
                index++;
            }

            return routeInfoList;
        }
        

        private static List<string> GenSearchUrlList(string locationCode)
        {
            // Function:
            //          due to the 1000 route number list on mountain project search page.
            //          This function is to devided routes from one state into several batches categrized by grade
            //         
            // Input:
            //          location code for the state
            //
            // Note: 
            //          the number specified for each grade is based on the definition from mountain project website

            List<String> gradeList = new List<string> {"20000", // vB
                                                       "20050", // v0
                                                       "20150", // v1
                                                       "20250", // v2
                                                       "20350", // v3
                                                       "20450", // v4
                                                       "20550", // v5
                                                       "20650", // v6
                                                       "20750", // v7                                                  
                                                       "20850", // v8
                                                       "20950", // v9
                                                       "21050", // v10
                                                       "21150", // v11
                                                       "21250", // v12
                                                       "21350", // v13
                                                       "21450", // v14
                                                       "21550", // v15
                                                       "21650", // v16
                                                       "21750"};// v17
            List<string> urlList = new List<string>();

            for (int i = 1; i < gradeList.Count; i++)
            {
                // generate search url based on grade and location
                // for searching grade Vn for instance, min grade shall be V(n-1) and grade max shall be V(n)

                string searchUrl = "https://www.mountainproject.com/route-finder?selectedIds=" +
                                    locationCode +
                                    "&type=boulder&diffMinrock=1800&diffMinboulder=" +
                                    gradeList[i-1] +
                                    "&diffMinaid=70000&diffMinice=30000&diffMinmixed=50000&diffMaxrock=5500&diffMaxboulder=" +
                                    gradeList[i] +
                                    @"&diffMaxaid=75260&diffMaxice=38500&diffMaxmixed=60000&is_trad_climb=1&is_sport_climb=1&is_top_rope=1&stars=0&pitches=0&sort1=popularity+desc&sort2=rating&viewAll=1";
                
                urlList.Add(searchUrl);
            }

            return urlList;
        }

        private static void CrawlePerGrade(string url, List<RouteInfo> routeInfoList, HashSet<string> routeLinkSet, int searchGrade)
        {

            // Function: 
            //          1) get each route page link from the 'url' page from inputs 
            //             (In search url page, 'a' tag with class as "text-black route-row" is the route info page for each route)
            //
            //          2) collect route name, route grade, route location, ruote link and saved in routeInfoList by visiting each route link 
            //
            //          3) Error message:
            //                          A. if location code is incorrect, throw an error message
            //                          B. If 1000 routes link is found in the search page, throw an error message.
            //                             (if search page has "All Locations", it means location code is not correct
            //                             a more accurate check could be find tag name and keywords as below:
            //                             "<span id="single-area-picker-name">All Locations</span>"
            //
            // Inputs:
            //         url :          the url shall include list of route links from mountain project route finder engine
            //         routeInfoList: it contains route info class defined in WebAppDataLib.Models
            //         routeLinkSet:  it contains route links which has been visited from lower grading url
            //         searchGrade:   used for error reporting to indicate which grade has more than 1000 routes
            //

            /// Get each route page
            HtmlText html = new HtmlText(url);
            List<string> routeLinkTagList = HtmlFind.HtmlFindAllTags(html.htmlText, "a", "class=\"text-black route-row\"");


            /// ERROR A:          
            if (html.htmlText.Contains("All Locations"))
            {
                throw new ArgumentException("Incorrect Location Code!");
            }
            /// ERROR B:    
            if (routeLinkTagList.Count == 1000)
            {
                throw new ArgumentException("Route Number at Grade V" + searchGrade + " exceed 1000!");
            }


            /// Crawling 
            Console.WriteLine("Collect " + routeLinkTagList.Count + " routes information...");
            foreach (string routeLinkTag in routeLinkTagList)
            {
                string routeLink = HtmlFind.GetHyperLinks(routeLinkTag, "href=");

                // skip visited routes
                if (routeLinkSet.Contains(routeLink)) 
                {                  
                    continue;
                }
                else
                {                   
                    HtmlText route = new HtmlText(routeLink);
                    // ERROR:
                    // below fault can be pre-detected by error reporting in 
                    if (route.htmlText.Contains("The page you're looking for does not exist"))
                    {
                        throw new InvalidOperationException("Page Not Found!");
                    }

                    // Collect route information from each route page
                    RouteInfo routeInfo = new RouteInfo(GetRouteInfo.GetRouteName(route),
                                                        GetRouteInfo.GetRouteGrade(route),
                                                        GetRouteInfo.GetRouteLocation(route), 
                                                        routeLink);
                    routeInfoList.Add(routeInfo);
                    routeLinkSet.Add(routeLink);
                    // Console.WriteLine("processing route : " + routeName + " ...");
                }
            }
        }
    }
}
