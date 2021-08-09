using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace WebAppDataLib.Models
{
    public class SqlConn
    {
        public SqlConnection connection;
        private string connStr;
        public  SqlConn(string dataSource, string userID, string password, string catalog)
        {
            // 
            //builder.DataSource = "tcp:routeinfo.database.windows.net,1433";
            //builder.UserID = "adminJS";
            //builder.Password = "JasonSun=";
            //builder.InitialCatalog = "TestSql";//MP_RouteInfo
            //
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = dataSource;
            builder.UserID = userID;
            builder.Password = password;
            builder.InitialCatalog = catalog;
            connStr = builder.ConnectionString;
            connection = new SqlConnection(connStr);
        }
    }
}
