using System.Diagnostics;

namespace Andy.X.MSSQL.Source.Connector.Core.Utilities.Logging
{
    public static class Logger
    {
        public static void LogInformation(string log)
        {
            Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH-mm-ss} andyx-mssql-source-connector [info]     |   {log}");
        }

        public static void LogWarning(string log)
        {
            var generalColor = Console.ForegroundColor;
            Trace.Write($"{DateTime.Now:yyyy-MM-dd HH-mm-ss} andyx-mssql-source-connector ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Trace.Write($"[warning]");
            Console.ForegroundColor = generalColor;
            Trace.WriteLine($"  |   {log}");
        }

        public static void LogError(string log)
        {
            var generalColor = Console.ForegroundColor;
            Trace.Write($"{DateTime.Now:yyyy-MM-dd HH-mm-ss} andyx-mssql-source-connector ");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.Write($"[error]");
            Console.ForegroundColor = generalColor;
            Trace.WriteLine($"    |   {log}");
        }

        public static void LogError(string log, string logWithRed)
        {
            var generalColor = Console.ForegroundColor;
            Trace.Write($"{DateTime.Now:yyyy-MM-dd HH-mm-ss} andyx-mssql-source-connector ");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.Write($"[error]");
            Console.ForegroundColor = generalColor;
            Trace.Write($"    |   {log}");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.WriteLine(logWithRed);
            Console.ForegroundColor = generalColor;
        }

        public static void ShowWelcomeText()
        {
            var generalColor = Console.ForegroundColor;
            Trace.WriteLine("                   Starting Buildersoft Andy X MSSQL Source Connector");
            Trace.WriteLine("                   Copyright (C) 2022 Buildersoft LLC");
            Trace.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.Write("  ###"); Console.ForegroundColor = generalColor; Trace.WriteLine("      ###");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.Write("    ###"); Console.ForegroundColor = generalColor; Trace.Write("  ###");
            Trace.WriteLine("       Andy X MSSQL Source Connector 3.0.0 Copyright (C) 2022 Buildersoft LLC. Andy X IO Framework v3.0.0-preview");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.Write("      ####         "); Console.ForegroundColor = generalColor; Trace.WriteLine("Licensed under the Apache License 2.0.  See https://bit.ly/3DqVQbx");
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.WriteLine("    ###  ###");
            Trace.Write("  ###      ###     "); Console.ForegroundColor = generalColor; Trace.WriteLine("Andy X Connectors is an open source distributed platform for change data capture. Start it up, point it at your databases, and your apps can start responding to all of the inserts, updates, and deletes that other apps commit to your databases");
            Trace.WriteLine("");


            Trace.WriteLine("                   Starting Buildersoft Buildersoft Andy X MSSQL Source Connector...");
            Trace.WriteLine("\n");
        }
    }
}
