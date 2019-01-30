using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace SQLtest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DESKTOP-T44BHH8\\SQLEXPRESS"; //"BMO\\SQLEXPRESS"; // DESKTOP-T44BHH8\SQLEXPRESS
                builder.UserID = "rejurhf";
                builder.Password = "4815162342";
                builder.InitialCatalog = "AdventureWorks2017";

                Console.WriteLine("Connecting to sql");
                SqlConnection connection = new SqlConnection(builder.ConnectionString);

                //ResultSaver resultSaver = new ResultSaver(new SavingToConsoleStrategy());
                ResultSaver resultSaver = new ResultSaver(new SavingToDBStrategy());

                SqlQueryExecution queryExecution = new SqlQueryExecution(connection);
                queryExecution.ResultSaver = resultSaver;


                queryExecution.Execute("select top 10 BusinessEntityID, FirstName, LastName from Person.Person;", 31);
                queryExecution.Execute("select * from Person.Person;", 32);
                queryExecution.Execute("select * from HumanResources.Employee where BusinessEntityID = any(select BusinessEntityID from Person.Person where PersonType = 'EM');", 33);
                queryExecution.Execute("select * from Sales.Customer", 34);
                queryExecution.Execute("select * from Sales.CreditCard", 35);



            }
            catch(SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("All done");
            Console.ReadKey(true);
        }
    }
}
