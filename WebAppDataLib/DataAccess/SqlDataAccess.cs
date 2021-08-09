using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebAppDataLib.Models;

namespace WebAppDataLib.DataAccess
{  
    public class SqlDataAccess
    {
        // This class includes basic functions for connecting to database, read/write data, execute quary command

        public static string GetConnectionStr(string dataSource, string userID, string password, string catalog)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = dataSource;
            builder.UserID = userID;
            builder.Password = password;
            builder.InitialCatalog = catalog;
            return builder.ConnectionString;
        }

        public static string GetConnectionStr()
        {
            // default configuration for the Azure database
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:routeinfo.database.windows.net,1433";
            builder.UserID = "adminJS";
            builder.Password = "JasonSun=";
            builder.InitialCatalog = "MP_RouteInfo";
            return builder.ConnectionString;
        }

        public static void ExeSqlCommand(string connStr, string commandText)
        {
            // execute any command string with the connection string provided.
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr)) //builder.ConnectionString
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static List<RouteInfo> ReadFromSql(string connStr, string commandText)
        {
            List<RouteInfo> routeInfoListFromSql = new List<RouteInfo>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                RouteInfo routeInfoReadFromSql = new RouteInfo(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                                routeInfoListFromSql.Add(routeInfoReadFromSql);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        reader.Close();
                    }
                }

                return routeInfoListFromSql;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return routeInfoListFromSql;
            }
        }

        public static void WriteToSql(string connStr, string commandText)
        {
            // Function:
            //         write data to Azure database table,
            //         in case save to current table fails, copy the backup table

            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
  
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                // copy the backup table content to the current table
                string copyFromBackUpTableCommand = "INSERT INTO MpCrawler SELECT* FROM MpCrawlerBackUp";
                ExeSqlCommand(connStr, copyFromBackUpTableCommand);

                // error reporting
                Console.WriteLine("error detected while writing to sql database! copy back table data!");
                Console.WriteLine(e.ToString());
            }
        }

        //public static List<RouteInfo> ReadFromSql(string commandText, string connectionStr)
        //{
        //    List<RouteInfo> routeInfoListFromSql = new List<RouteInfo>();
        //    SqlConnection connection = new SqlConnection(connectionStr);
        //    try
        //    {
        //        using (connection)
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, connection))
        //            {
        //                connection.Open();

        //                SqlDataReader reader = command.ExecuteReader();

        //                if (reader.HasRows)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        RouteInfo routeInfoReadFromSql = new RouteInfo(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
        //                        routeInfoListFromSql.Add(routeInfoReadFromSql);
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("No rows found.");
        //                }
        //                reader.Close();
        //            }
        //        }

        //        return routeInfoListFromSql;
        //    }
        //    catch (SqlException e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        return routeInfoListFromSql;
        //    }
        //}
    }
}
