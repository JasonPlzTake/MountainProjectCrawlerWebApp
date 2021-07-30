using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GlobalLib;

namespace AzureSql
{
    /// <summary>
    /// 
    /// </summary>
    /// Todo: try catch make sense to each function? 
    public class AzureSqlConn
    {
        public string connectStr;
        SqlConnection connection;

        //public AzureSqlConn(string connectStr)
        //{
        //    this.connectStr = connectStr;
        //    connection = new SqlConnection(connectStr);
        //}


        public AzureSqlConn(string dataSource, string userID, string password, string catalog)
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
            connectStr = builder.ConnectionString;
            connection = new SqlConnection(connectStr);
        }

        /// <summary>
        ///     Test that the server is connected
        /// </summary>
        // <param name="connectionString">The connection string</param>
        // <returns>true if the connection is opened</returns>
        // Todo: is it better to using static function?

        public bool IsServerConnected()
        {
            using (connection)
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }


        public bool IsTableExsit(string tableName)
        {
            try
            {
                string commandText = "SELECT COUNT(*) FROM "+ tableName +";";
                try
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.ExecuteScalar();
                        connection.Close();
                    }
                    return true;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    connection.Close();
                    return false;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                connection.Close();
                return false;
            }
        }


        public void AddDelTable(string commandText)
        {
            // Todo: this function can be any command based function
            using (connection)//builder.ConnectionString
            {
                //using (SqlCommand command = new SqlCommand(commandText, connection))
                //{
                //    connection.Open();
                //    using (SqlDataReader reader = command.ExecuteReader())
                //    {
                //        // Todo: why we need using reader for generating a table ? For calling a command? 
                //    }
                //}
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void readFrom(string commandText)
        {
            try
            {
                using (connection)
                {
                    // Todo: how to access the read and save it for evaluation and present to UX
                    //
                    // string commandText = @"SELECT TOP (10) [ProductID]
                    //              ,[Name]
                    //              ,[ProductNumber]
                    //              ,[Color]
                    //              ,[StandardCost]
                    //              ,[ListPrice]
                    //              ,[Size]
                    //              ,[Weight]
                    //              ,[ProductCategoryID]
                    //              ,[ProductModelID]
                    //              ,[SellStartDate]
                    //              ,[SellEndDate]
                    //              ,[DiscontinuedDate]
                    //              ,[ThumbNailPhoto]
                    //              ,[ThumbnailPhotoFileName]
                    //              ,[rowguid]
                    //              ,[ModifiedDate]
                    //          FROM[SalesLT].[Product]";
                    //
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}", reader.GetInt32(0),
                                    reader.GetString(1));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        reader.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }


        public void writeTo(string tableName, List<RouteInfo> routeInfoList)
        {
            try
            {
                // below is for write into collumn
                //
                // Method 1: using parameters
                //string cmdText = @"insert into "+ tableName + " values(@routeName, @routeGrade, @routeLocation, @routeLink)";

                //using (connection)
                //{

                //    foreach (RouteInfo routeInfo in routeInfoList)
                //    {
                //        using (SqlCommand command = new SqlCommand(cmdText, connection))
                //        {
                //            connection.Open();
                //            command.Parameters.AddWithValue("@routeName", routeInfo.routeName);
                //            command.Parameters.AddWithValue("@routeGrade", routeInfo.routeGrade);
                //            command.Parameters.AddWithValue("@routeLocation", routeInfo.routeLocation[0]); // how do we want to save location as list
                //            command.Parameters.AddWithValue("@routeLink", routeInfo.routeLink);
                //            command.ExecuteNonQuery();
                //            connection.Close();
                //        }
                //    }  
                //}
                //Console.WriteLine("completed***");
                //Console.ReadLine();
                //
                //
                // Method2:
                //string commandText = @"INSERT INTO MpCrawler (
                //                    RouteName,
                //                    RouteGrade,
                //                    Address,
                //                    RouteLink
                //                )
                //                VALUES
                //                    (
                //                        '2019 Summer Promotion',
                //                        'v5',
                //                        '20190601',
                //                        '20190901'
                //                    ),
                //                    (
                //                        '2019 Winter Promotion',
                //                        'v5',
                //                        '20191201',
                //                        '20200101'
                //                    )";
                //using (connection)
                //{
                //    connection.Open();
                //    using (SqlCommand command = new SqlCommand(commandText, connection))
                //    {

                //        command.ExecuteNonQuery();

                //    }
                //    connection.Close();

                //}

                //  Method3:
                using (connection)
                {
                    connection.Open();
                    foreach (RouteInfo routeInfo in routeInfoList)
                    {
                        string commandText = GenCommandText(tableName, routeInfo);
                        using (SqlCommand command = new SqlCommand(commandText, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private string GenCommandText(string tableName, RouteInfo routeInfo)
        {
            // Function:
            //          this function is to generate command string
            //          for writing single route info into a new row in sql table
            string commandText = "INSERT INTO " + tableName + @" (RouteName, RouteGrade, Address,RouteLink)
                                  VALUES ('" + routeInfo.routeName + "', '" + routeInfo.routeGrade + "', '" + routeInfo.routeLocation + "', '" + routeInfo.routeLink + "'); ";

            return commandText;
        }
    }
}

