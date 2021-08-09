using System;
using System.Collections.Generic;
using WebAppDataLib.Models;
using WebAppDataLib.DataAccess;
using System.Text;

namespace WebAppDataLib.BusinessLogic
{
    public class RouteInfoProcessor
    {
        public static List<RouteInfo> SearchRoutesInfoFromSql(string connStr, SearchCriteria searchCriteria, string tableName)
        {
            // get SQL command string based on input search criteria
             string commandText = GenSqlReadCommandText(searchCriteria, tableName);
            List<RouteInfo> routeList = SqlDataAccess.ReadFromSql(connStr, commandText);
            return routeList;
        }

        public static void SaveRouteInfoToSql(string connStr, List<RouteInfo> routeInfoList, string tableName)
        {
            // Function:
            //          Save route informtion list data into Azure database table,
            //          1) Generate a long string including all commands for saving each route info for each row
            //          2) execute save to database command with given connection string
            //          3) update backup table by copying data from the current table
            //

            StringBuilder commandText = new StringBuilder();
            foreach (RouteInfo routeInfo in routeInfoList)
            {
                // generate command for each route
                commandText.Append(GenSaveToSqlCommandText(connStr, tableName, routeInfo));
                commandText.AppendLine();
            }
            // execute write to database function
            SqlDataAccess.WriteToSql(connStr, commandText.ToString());

            // update the backup table
            string copyFromBackUpTableCommand = "INSERT INTO MpCrawlerBackUp SELECT* FROM MpCrawler";
            SqlDataAccess.ExeSqlCommand(connStr, copyFromBackUpTableCommand);
        }

        private static string GenSqlReadCommandText(SearchCriteria searchCriteria, string tableName)
        {
            // Todo: verify default N/A works, 
            // location N/A searchCriteria.location = ""
            // gradeLow N/A searchCriteria.gradeLow = v0 (test!)
            // gradeHigh N/A searchCriteria.gradeHigh = v20 (test!)
            //
            string gradeLow = searchCriteria.gradeLow == "N/A"? "V-1" : searchCriteria.gradeLow;
            string gradeHigh = searchCriteria.gradeHigh == "N/A"? "V20" : searchCriteria.gradeHigh;

            string commandText = @"SELECT TOP (100) [RouteName],[RouteGrade],[Location],[RouteLink] FROM [dbo].[" +
                                tableName + 
                                "] WHERE RouteName LIKE '%" + 
                                searchCriteria.keywords + "%' AND Location LIKE '%" + 
                                searchCriteria.routeLocation + "%' AND RouteGrade >= '" + 
                                gradeLow + "' AND RouteGrade <= '" + gradeHigh + "'";

            return commandText;
        }

        private static string GenSaveToSqlCommandText(string connStr, string tableName, RouteInfo routeInfo)
        {
            // Function:
            //          this function is to generate command string
            //          for writing single route info into a new row in sql table
            string commandText = "INSERT INTO " + tableName + @" (RouteName, RouteGrade, Location, RouteLink)
                                  VALUES ('" + 
                                  routeInfo.routeName + "', '" + 
                                  routeInfo.routeGrade + "', '" + 
                                  routeInfo.routeLocation + "', '" + 
                                  routeInfo.routeLink + "'); ";

            return commandText;
        }
    }
}
