using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GlobalLib;
using AzureSql;

namespace MpCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            // todo: handle search result above 1000
            //
            string url = "https://www.mountainproject.com/route-finder?selectedIds=105708966&type=boulder&diffMinrock=1800&diffMinboulder=21150&diffMinaid=70000&diffMinice=30000&diffMinmixed=50000&diffMaxrock=5500&diffMaxboulder=21250&diffMaxaid=75260&diffMaxice=38500&diffMaxmixed=60000&is_trad_climb=1&is_sport_climb=1&is_top_rope=1&stars=0&pitches=0&sort1=area&sort2=rating";
            HtmlText html = new HtmlText(url);
            // find route link section
            List<string> routeLinkTagList = HtmlFind.HtmlFindAllTags(html.htmlText, "a", "class=\"text-black route-row\"");
            List<RouteInfo> routeInfoList = new List<RouteInfo>();

            foreach (string routeLinkTag in routeLinkTagList)
            {
                string routeLink = HtmlFind.GetHyperLinks(routeLinkTag, "href=");

                HtmlText route = new HtmlText(routeLink);
                
                string routeName = GetRouteInfo.GetRouteName(route);
                string routeGrade = GetRouteInfo.GetRouteGrade(route);
                string routeLocation = GetRouteInfo.GetRouteLocation(route);

                RouteInfo routeInfo = new RouteInfo(routeName, routeGrade, routeLocation, routeLink);
                routeInfoList.Add(routeInfo);
                Console.WriteLine("processing route : " + routeName + " ...");
            }
            Console.WriteLine("total " + routeLinkTagList.Count + "routes");

            // build connection and save to sql database
            // Todo: present having password in thge code
            string dataSource = "tcp:routeinfo.database.windows.net,1433";
            string userId = "adminJS";
            string password = "JasonSun=";
            string catalog = "TestSql"; // MP_RouteInfo
            string tableName = "MpCrawler";
            // Create table if not exist
            try
            {
                AzureSqlConn sqlConn = new AzureSqlConn(dataSource, userId, password, catalog);
                // Todo: if a table is not there, create a new one
                // Todo: bug is not fixed. how to create a table and add row with one connection initalization
                sqlConn.AddDelTable("IF NOT EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = 'MpCrawler') CREATE TABLE MpCrawler(RouteName varchar(255),RouteGrade varchar(255),Address varchar(255), RouteLink varchar(255));");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            // add rows into
            try
            {
                AzureSqlConn sqlConn = new AzureSqlConn(dataSource, userId, password, catalog);
                sqlConn.writeTo(tableName, routeInfoList);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            //Console.ReadLine();
            Console.WriteLine("main ends");
        }
    }
}

// WORKSPACE:
//
// how do we determine is url is responding and content is valid
// string url = "https://www.mountainproject.com/route-guide"; // route-guide
// string url = "https://www.mountainproject.com/route/106806101/kellys-fly"; // single route page
//
// string tagName = "a";
//string className = "tab";
//List<string> res = new List<string>();
//List<string> linkList = new List<string>();
//res = html.HtmlFindAllTags(tagName);
////res = html.HtmlFindAllTags(tagName, className);
//linkList = html.GetHyperLinks(res, "www.mountainproject.com/route");
//foreach (string s in linkList)
//    Console.WriteLine(s);
//string routeName = GetRouteInfo.GetRouteName(html);
//string routeGrade = GetRouteInfo.GetRouteGrade(html);

//List<string> routeLocation = GetRouteInfo.GetRouteLocation(html);
//foreach (string s in routeLocation)
//    Console.WriteLine(s);