using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace AzureSql
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("hello");
            //try
            //{
                // Todo: present having password in thge code
                //string dataSource = "tcp:routeinfo.database.windows.net,1433";
                //string userId = "adminJS";
                //string password = "JasonSun=";
                //string catalog = "TestSql"; // MP_RouteInfo
                //string tableName = "MpCrawler";

                //AzureSqlConn sqlConn = new AzureSqlConn(dataSource, userId, password, catalog);

                // Todo: if a table is not there, create a new one
                //sqlConn.AddDelTable("CREATE TABLE MpCrawler(RouteName varchar(255),RouteGrade varchar(255),Address varchar(255), RouteLink varchar(255));");
                //
                // test write funciton
                //List<RouteInfo> routeInfoList = new List<RouteInfo>();// get from crewler project as the API between sql and crawler
                //routeInfoList.Add(new RouteInfo("www.sfs", "whattttt", "v5", "address is not applicable"));
                //routeInfoList.Add(new RouteInfo("www.sxxx", "no000", "v10", "address is not applicable"));
                //sqlConn.writeTo(tableName, routeInfoList);
            //}
            //catch (SqlException e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
            //Console.ReadLine();
        }
    }
}