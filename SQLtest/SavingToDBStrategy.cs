﻿using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class SavingToDBStrategy : SavingStrategy
    {
        private SqlConnection connection;
        private const string insertQuery = "INSERT INTO Performance.DB (LineNumber, CpuTime, ElapsedTime, BytesReceived, RowsSelected, AddTime)" +
            " VALUES (@LineNumber, @CpuTime, @ElapsedTime, @BytesReceived, @RowsSelected, CURRENT_TIMESTAMP);";

        public SavingToDBStrategy()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            
            //builder.DataSource = "DESKTOP-T44BHH8\\SQLEXPRESS";
            //builder.UserID = "rejurhf";
            //builder.Password = "4815162342";

            builder.DataSource = "BMO\\SQLEXPRESS";
            builder.UserID = "krzysiek";
            builder.Password = "password";
            
            builder.InitialCatalog = "kgeleta";
            this.connection = new SqlConnection(builder.ConnectionString);
        }

        void SavingStrategy.SaveResult(QueryPerformanceResult result)
        {
            SqlCommand command = new SqlCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@LineNumber", result.LineNumber);
            command.Parameters.AddWithValue("@CpuTime", result.CpuTime);
            command.Parameters.AddWithValue("@ElapsedTime", result.ElapsedTime);
            command.Parameters.AddWithValue("@BytesReceived", result.BytesReceived);
            command.Parameters.AddWithValue("@RowsSelected", result.SelectRows);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        void SavingStrategy.SaveResult(LocalPerformanceResult result)
        {
            Console.WriteLine("Not implemented");
        }
    }
}
