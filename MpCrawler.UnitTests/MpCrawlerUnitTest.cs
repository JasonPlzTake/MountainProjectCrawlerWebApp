using Microsoft.VisualStudio.TestTools.UnitTesting;
using MpCrawler.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using WebAppDataLib.Models;

namespace MpCrawler.UnitTests
{
    /// <summary>
    /// TODO: Unit Tests will be moved to correspondent sub-folders
    /// 
    /// </summary>

    [TestClass]
    public class MpCrawler_Module_Crawler_UnitTest
    {
        /// <summary>
        /// Below are module testing
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Incorrect Location Code!")]
        public void MpBoulderRouteCrawler_IncorrectLocationCode_CrawleNothing()
        {
            // Test Scope:
            //          1) Test when location code is incorrect or not provided
            //          2) if incorrect location code is given, if it is not '404 page', it would show top 1000 all routes from all locations

            // Prepare input and expectation
            List<string> locationCodeList = new List<string> { "", "0" };

            // Execute Test
            foreach (string location in locationCodeList)
            {
                Crawler.MpBoulderRouteCrawler(location);
            }
        }

        [TestMethod]
        public void MpBoulderRouteCrawler_NormalCase_ReturnRouteInfoList()
        {
            // Prepare input and expectation
            string locationCode = "107005337";

            // Generate expectation map
            Hashtable expectMap = new Hashtable();
            GenExpectationMap(expectMap);

            // Execute Test
            List<RouteInfo> resultList = Crawler.MpBoulderRouteCrawler(locationCode);

            // Assessment
            CrawlerAssessment(expectMap, resultList);
        }

        private void GenExpectationMap(Hashtable expectMap)
        {
            // define expectation hash table,
            // using hash table in case the order in the  result list is cdifferent from expectation
            // TODO: a more comprehensive test case can use mountain project export excel sheet for verification

            RouteInfo route1 = new RouteInfo("Jug Head",
                                "V0",
                                "washington->okanogan->burge mountain tonasket->burge boulders",
                                "https://www.mountainproject.com/route/113500461/jug-head");
            RouteInfo route2 = new RouteInfo("R to L Traverse",
                                            "V-easy",
                                            "washington->okanogan->burge mountain tonasket->burge boulders",
                                            "https://www.mountainproject.com/route/115154754/r-to-l-traverse");
            RouteInfo route3 = new RouteInfo("Ledgible",
                                            "V-easy",
                                            "washington->okanogan->burge mountain tonasket->burge boulders",
                                            "https://www.mountainproject.com/route/113500446/ledgible");
            RouteInfo route4 = new RouteInfo("Traverse Left",
                                            "V-easy",
                                            "washington->okanogan->burge mountain tonasket->burge boulders",
                                            "https://www.mountainproject.com/route/113500557/traverse-left");
            RouteInfo route5 = new RouteInfo("The Shark Fin",
                                            "V1",
                                            "washington->okanogan->burge mountain tonasket->burge boulders",
                                            "https://www.mountainproject.com/route/113500429/the-shark-fin");
            RouteInfo route6 = new RouteInfo("The Wave",
                                            "V1",
                                            "washington->okanogan->burge mountain tonasket->burge boulders",
                                            "https://www.mountainproject.com/route/113500415/the-wave");
            expectMap.Add(route1.routeName, route1);
            expectMap.Add(route2.routeName, route2);
            expectMap.Add(route3.routeName, route3);
            expectMap.Add(route4.routeName, route4);
            expectMap.Add(route5.routeName, route5);
            expectMap.Add(route6.routeName, route6);
        }

        private void CrawlerAssessment(Hashtable expectMap, List<RouteInfo> resultList)
        {
            foreach (RouteInfo route in resultList)
            {
                string expRouteName = route.routeName;
                if (expectMap.ContainsKey(expRouteName))
                {
                    RouteInfo expectRouteInfo = (RouteInfo)expectMap[expRouteName];
                    if (!route.routeGrade.Equals(expectRouteInfo.routeGrade))
                    {
                        // add indication message as needed for debugging
                        Console.WriteLine("Grade not match! See route:" + expRouteName);
                        Console.WriteLine("Expected:" + expectRouteInfo.routeGrade + ", Actual:" + route.routeGrade);
                        Assert.Fail();
                    }

                    if (!route.routeLocation.Equals(expectRouteInfo.routeLocation))
                    {
                        // add indication message as needed for debugging
                        Console.WriteLine("Location not match! See route:" + expRouteName);
                        Console.WriteLine("Expected:" + expectRouteInfo.routeLocation + ", Actual:" + route.routeLocation);
                        Assert.Fail();
                    }

                    if (!route.routeLink.Equals(expectRouteInfo.routeLink))
                    {
                        // add indication message as needed for debugging
                        Console.WriteLine("Link not match! See route:" + expRouteName);
                        Console.WriteLine("Expected:" + expectRouteInfo.routeLink + ", Actual:" + route.routeLink);
                        Assert.Fail();
                    }
                }
                else
                {
                    // add indication message as needed for debugging
                    Assert.Fail();
                }
            }
        }
    }


    public class MpCrawler_Module_SqlAccess_UnitTest
    {
    }

    /// <summary>
    /// Below are unit test
    /// 
    /// </summary>
    public class MpCrawler_FuncLib_GetRouteInfo_UnitTest
    {
    }

    public class MpCrawler_FuncLib_HtmlFind_UnitTest
    {
    }

}
