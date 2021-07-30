using System;
namespace AzureSql
{
    public class EmptyClass
    {
        public EmptyClass()
        {
            //try
            //{
            // Console.WriteLine(builder.ConnectionString);
            // string connStr = "Server=tcp:routeinfo.database.windows.net,1433;Initial Catalog=MP_RouteInfo;Persist Security Info=False;User ID=adminJS;Password={JasonSun=};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";  

            //
            //
            // below is for creating a table
            //using (connection)//builder.ConnectionString
            //{
            //    Console.WriteLine("\nQuery data example:");
            //    Console.WriteLine("=========================================\n");

            //    String sql = "CREATE TABLE Pt(PersonID int,LastName varchar(255),FirstName varchar(255),Address varchar(255),City varchar(255)); ";

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            // why we need using reader for generating a table
            //        }
            //    }
            //}
            //
            //
            // below is for write into collumn
            // INSERT INTO table_name (column1, column2, column3, ...)
            // VALUES(value1, value2, value3, ...);
            //
            // INSERT INTO table_name
            // VALUES(value1, value2, value3, ...);
            //
            // INSERT Persons (PersonID, LastName)
            // VALUES('001', 'Sun');
            //


            //define the insert sql command, here I insert data into the student table in azure db.
            //    string cmdText = @"insert into Persons (PersonID, LastName)
            //           values(@id, @lastname)";

            //    using (connection)
            //    {
            //        using (SqlCommand command = new SqlCommand(cmdText, connection))
            //        {
            //            connection.Open();
            //            command.Parameters.AddWithValue("@id", "0012");
            //            command.Parameters.AddWithValue("@lastname", "yang1");
            //            command.ExecuteNonQuery();
            //            connection.Close();
            //        }
            //    }

            //    Console.WriteLine("completed***");
            //    Console.ReadLine();
            //}
            //
            //
            //
            //
            // below is for reading
            //using (connection)
            //{
            //    string sql = @"SELECT TOP (10) [ProductID]
            //                  ,[Name]
            //                  ,[ProductNumber]
            //                  ,[Color]
            //                  ,[StandardCost]
            //                  ,[ListPrice]
            //                  ,[Size]
            //                  ,[Weight]
            //                  ,[ProductCategoryID]
            //                  ,[ProductModelID]
            //                  ,[SellStartDate]
            //                  ,[SellEndDate]
            //                  ,[DiscontinuedDate]
            //                  ,[ThumbNailPhoto]
            //                  ,[ThumbnailPhotoFileName]
            //                  ,[rowguid]
            //                  ,[ModifiedDate]
            //              FROM[SalesLT].[Product]";

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //        connection.Open();

            //        SqlDataReader reader = command.ExecuteReader();

            //        if (reader.HasRows)
            //        {
            //            while (reader.Read())
            //            {
            //                Console.WriteLine("{0}\t{1}", reader.GetInt32(0),
            //                    reader.GetString(1));
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("No rows found.");
            //        }
            //        reader.Close();
            //    }
            //
            //
            //}
            //catch (SqlException e)
            //{
            //    Console.WriteLine(e.ToString());
            //}

    }
}
}



//private string GenCommandText(string tableName, RouteInfo routeInfo)
//{
//    // Function:
//    //          this function is to generate command string
//    //          for writing single route info into a new row in sql table
//    string commandText = "INSERT INTO " + tableName + @"(RouteName, RouteGrade, Address,RouteLink)
//            VALUES (" + routeInfo.routeName + "," + routeInfo.routeGrade + "," + routeInfo.routeLocation + "," + routeInfo.routeLink);

//return commandText;
//        }