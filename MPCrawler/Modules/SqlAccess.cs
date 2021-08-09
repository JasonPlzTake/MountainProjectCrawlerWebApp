using System;
using System.Collections.Generic;
using WebAppDataLib.Models;
using WebAppDataLib.BusinessLogic;
using WebAppDataLib.DataAccess;

namespace MpCrawler.Modules
{
    public static class SqlAccess
    {
        // Ths class includes functions to save crawled data to Azure database and read data from database as well
        // Use basic function from global library 'WebAppDataLib.DataAccess;'
        public static void SaveDataToSql(List<RouteInfo> routeInfoList)
        {
            // if routeInfoList is empty don't execute write to sql action
            if (routeInfoList.Count == 0)
            {
                Console.WriteLine("No data needs to be saved!");
                return;
            }

            // configure connection string with below information
            string dataSource = "tcp:routeinfo.database.windows.net,1433";
            string userId = "adminJS";
            string password = "JasonSun=";
            string catalog = "MP_RouteInfo"; // MP_RouteInfo
            string tableName = "MpCrawler";
            string connStr = SqlDataAccess.GetConnectionStr(dataSource, userId, password, catalog);

            // delete old table if exsit
            string sqlCommandDel = @"
                                IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MpCrawler]') AND type in (N'U'))
                                DROP TABLE [dbo].[MpCrawler]";
            SqlDataAccess.ExeSqlCommand(connStr, sqlCommandDel);

            // create a new table with specified table name
            string sqlCommandGenNewTable = @"
                                IF NOT EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = 'MpCrawler') 
                                CREATE TABLE MpCrawler(RouteName varchar(255),RouteGrade varchar(255), Location varchar(255), RouteLink varchar(255))";
            SqlDataAccess.ExeSqlCommand(connStr, sqlCommandGenNewTable);
            
            // save route information list into sql
            RouteInfoProcessor.SaveRouteInfoToSql(connStr, routeInfoList, tableName);
            Console.WriteLine("database has been updated! ");
        }
    }
}
