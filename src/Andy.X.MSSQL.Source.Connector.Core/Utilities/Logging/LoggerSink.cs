﻿using Andy.X.MSSQL.Source.Connector.IO.Locations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andy.X.MSSQL.Source.Connector.Core.Utilities.Logging
{
    public class LoggerSink
    {
        public LoggerSink()
        {
            if (Directory.Exists(AppLocations.LogsDirectory()) != true)
            {
                Directory.CreateDirectory(AppLocations.LogsDirectory());
            }
        }

        public void InitializeSink()
        {
            Trace.Listeners.Clear();

            TextWriterTraceListener twtl = new TextWriterTraceListener(AppLocations.GetLogConfigurationFile(),
                AppDomain.CurrentDomain.FriendlyName);
            twtl.Name = "TextLogger";
            twtl.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime;

            ConsoleTraceListener ctl = new ConsoleTraceListener(false);
            ctl.TraceOutputOptions = TraceOptions.DateTime;

            Trace.Listeners.Add(twtl);
            Trace.Listeners.Add(ctl);
            Trace.AutoFlush = true;
        }
    }
}
