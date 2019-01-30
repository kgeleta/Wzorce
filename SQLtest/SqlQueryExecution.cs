using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQLtest
{
    class SqlQueryExecution
    {
        private SqlConnection connection;
        private const String statisticsOn = "set statistics time on;";
        private static string message;
        private ResultSaver resultSaver;

        internal ResultSaver ResultSaver { get => resultSaver; set => resultSaver = value; }

        public SqlQueryExecution(SqlConnection _connection)
        {
            connection = _connection;
            connection.StatisticsEnabled = true;
        }

        public void Execute(String sqlQuery, int lineNumber)
        {
            connection.Open();
            connection.ResetStatistics();
            
            SqlCommand command = new SqlCommand(statisticsOn + sqlQuery, connection);

            connection.InfoMessage += TrackInfo;
            command.ExecuteNonQuery();
            
            QueryPerformanceResult performanceResult = new QueryPerformanceResult();
            performanceResult.LineNumber = lineNumber;
            performanceResult.setTime(message);
            IDictionary currentStatistics = connection.RetrieveStatistics();
            performanceResult.BytesReceived = (long)currentStatistics["BytesReceived"];
            performanceResult.SelectRows = (long)currentStatistics["SelectRows"];

            resultSaver.SaveResult(performanceResult);

            connection.Close();
        }

        internal static void TrackInfo(object sender, SqlInfoMessageEventArgs e)
        {
            message = e.Message;
        }
    }
}
