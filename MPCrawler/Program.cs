using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using WebAppDataLib.Models;
//using WebAppDataLib.DataAccess;
//using WebAppDataLib.BusinessLogic;
//using MpCrawler.Modules;

namespace MpCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            // Project Folder Structure:
            //
            //              1) ClassDefine:
            //                             Class only used for crawler.
            //                             Other class used also in Web is referenced from global project (WebAppDataLib)
            //
            //              2) FuncLib: 
            //                              Re-usable functions
            //                              (Such as http tag search, route information search from html text ...)
            //
            //              3) Modules:
            //                              functions defined based on business logic 
            //                              (Such as
            //                                      crawling route information including route name, route grade, route location
            //                                      connected to sql create a table if table not found, save route info into database)
            //                              
            //
            // Project Scope: 
            //
            //              1) specify the state that crawler would look at for all bouldering problems
            //              2) collect all bouldering routes in that state 
            //              3) save collected route information into Azure database
            //              note: deploy to Azure Server and execute once a day
            //
            // TODO:
            //
            //              1) how to prevent data loss during crawlering? save each grading routes? how to handle the duplicated routes? 
            //              2) timeout exit? 
            //              3) present having password in thge code
            //              4) how do we know if database is updated? add update time to each route
            //              5) route name with "'s", "-" ... non-english/numerical letter

            string locationCode = "105708966";//"107005337"; // "108471474";//"108471397";; 
            List<RouteInfo> routeDataList = Modules.Crawler.MpBoulderRouteCrawler(locationCode);
            Modules.SqlAccess.SaveDataToSql(routeDataList);
            Console.WriteLine("main ends");
        }
    }
}