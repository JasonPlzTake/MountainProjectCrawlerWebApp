using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WebAppDataLib.Models
{
    public class ConnectionStr
    {
            public string connectStr;
            SqlConnection connection;

            public ConnectionStr(string dataSource, string userID, string password, string catalog)
            {
                // 
                //builder.DataSource = "tcp:routeinfo.database.windows.net,1433";
                //builder.UserID = "adminJS";
                //builder.Password = "JasonSun=";
                //builder.InitialCatalog = "MP_RouteInfo";
                //
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = dataSource;
                builder.UserID = userID;
                builder.Password = password;
                builder.InitialCatalog = catalog;
                connectStr = builder.ConnectionString;
                connection = new SqlConnection(connectStr);
            }
        }
}
