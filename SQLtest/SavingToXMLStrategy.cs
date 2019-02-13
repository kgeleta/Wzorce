using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class SavingToXMLStrategy : SavingStrategy
    {
        private string path;
        private string pathLocal;

        public SavingToXMLStrategy()
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.Length - 10);
            pathLocal = path;
            path += "/results.xml";
            pathLocal += "/resultsLocal.xml";
        }

        void SavingStrategy.SaveResult(QueryPerformanceResult result)
        {
            string text = File.ReadAllText(path);
            text = text.Replace("</results>", "<result>\n<ComputerName>" + Environment.MachineName + "</ComputerName>\n<LineNumber>" +
                result.LineNumber + "</LineNumber>\n<CpuTime>" + result.CpuTime + "</CpuTime>\n<ElapsedTime>" + result.ElapsedTime + 
                "</ElapsedTime>\n<BytesReceived>" + result.BytesReceived + "</BytesReceived>\n<RowsSelected>" + result.SelectRows +
                "</RowsSelected>\n</result>\n</results>");
            File.WriteAllText(path, text);
        }

        void SavingStrategy.SaveResult(LocalPerformanceResult result)
        {
            string text = File.ReadAllText(pathLocal);
            text = text.Replace("</results>", "<result>\n<ComputerName>" + Environment.MachineName + "</ComputerName>\n<LineNumber>" + 
                result.LineNumber +"</LineNumber>\n<CpuTime>" + result.CpuTime + "</CpuTime>\n<CpuUsage>" + result.CpuUsage + 
                "</CpuUsage>\n<ElapsedTime>" + result.ElapsedTime + "</ElapsedTime>\n</result>\n</results>");
            File.WriteAllText(pathLocal, text);
        }
    }
}
