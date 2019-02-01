using System;
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
        private const string insertQueryLocal = "INSERT INTO Performance.DBLocal (LineNumber, CpuTime, CpuUsage, ElapsedTime, AddTime)" +
            " VALUES (@LineNumber, @CpuTime, @CpuUsage, @ElapsedTime, CURRENT_TIMESTAMP);";

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
            SqlCommand command = new SqlCommand(insertQueryLocal, connection);
            command.Parameters.AddWithValue("@LineNumber", result.LineNumber);
            command.Parameters.AddWithValue("@CpuTime", result.CpuTime);
            command.Parameters.AddWithValue("@CpuUsage", result.CpuUsage);
            command.Parameters.AddWithValue("@ElapsedTime", result.ElapsedTime);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
