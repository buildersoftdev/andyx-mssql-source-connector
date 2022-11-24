using Andy.X.MSSQL.Source.Connector.Core.Services;
using Andy.X.MSSQL.Source.Connector.Core.Utilities.Logging;
using System;

namespace Andy.X.MSSQL.Source.Connector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoggerSink loggerSink;
            GlobalService globalService;

            // Sinking Console logs to log files
            loggerSink = new LoggerSink();
            loggerSink.InitializeSink();
            
            Logger.ShowWelcomeText();

            //loading services
            globalService = new GlobalService();
            
            Logger.LogInformation("Andy X MSSQL Source Connector is ready");


            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
