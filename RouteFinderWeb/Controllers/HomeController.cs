using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebAppDataLib.Models;
using WebAppDataLib.DataAccess;
using WebAppDataLib.BusinessLogic;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult RouteFinder()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ShowRouteInfoFromSql(SearchModel searchCriteria)
        {
            
            string tableName = "MpCrawler";

            // Transform search criteria collected from Web interface to Search criteria defined in WebAppDataLib
            SearchCriteria searchCr = new SearchCriteria();
            searchCr.routeLocation = searchCriteria.location;
            searchCr.gradeLow = searchCriteria.gradingLow;
            searchCr.gradeHigh = searchCriteria.gradingHigh;
            searchCr.keywords = searchCriteria.keyWords;

            // Use search criteria to get route info list from Azure database
            string connStr = SqlDataAccess.GetConnectionStr();
            List<RouteInfoModel> routeInfoViewList = new List<RouteInfoModel>();
            List<RouteInfo> routeInfoList = RouteInfoProcessor.SearchRoutesInfoFromSql(connStr, searchCr, tableName);

            // Transform sql read data to web presenting data
            foreach (RouteInfo routeInfo in routeInfoList)
            {
                RouteInfoModel rInfo = new RouteInfoModel();
                rInfo.routeName = routeInfo.routeName;
                rInfo.routeGrade = routeInfo.routeGrade;
                rInfo.routeLocation = routeInfo.routeLocation;
                rInfo.routeLink = routeInfo.routeLink;
                routeInfoViewList.Add(rInfo);
            }

            return View(routeInfoViewList);
        }


        public IActionResult ShowAllRoutes()
        {
            // page to request all routes from sql 
            // create html connection and command text
            string connStr = SqlDataAccess.GetConnectionStr();
            string commandText = @"SELECT TOP (1000) [RouteName]
                                  ,[RouteGrade]
                                  ,[Location]
                                  ,[RouteLink]
                                    FROM[dbo].[MpCrawler]";
            List<RouteInfoModel> routeInfoViewList = new List<RouteInfoModel>();
            List<RouteInfo> routeInfoList = SqlDataAccess.ReadFromSql(connStr, commandText);

            // transform sql read data to web shown data
            foreach (RouteInfo routeInfo in routeInfoList)
            {
                RouteInfoModel rInfo = new RouteInfoModel();
                rInfo.routeName = routeInfo.routeName;
                rInfo.routeGrade = routeInfo.routeGrade;
                rInfo.routeLocation = routeInfo.routeLocation;
                rInfo.routeLink = routeInfo.routeLink;

                routeInfoViewList.Add(rInfo);
            }

            return View(routeInfoViewList);
        }
    }
}
