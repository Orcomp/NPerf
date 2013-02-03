namespace NPerf.Experiment
{
    using System;
    using System.Linq;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Framework.Interfaces;
    using System.Diagnostics;

    internal class Program
    {
        private static string channelName;

        public static void Main(string[] args)
        {
         //   Debugger.Launch();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            var startParameters = new SartParameters(args);

            channelName = startParameters.ChannelName;

            if (string.IsNullOrEmpty(channelName))
            {
                SendError(new Exception("Channel name for interprocess communication was not set."));
                return;
            }
            try
            {
                ExperimentScope.Start(startParameters);
            }
            catch (Exception ex)
            {
                SendError(ex);
            }

           // Console.ReadKey();
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex == null)
            {
                return;
            }

            SendError(ex);
        }

        private static void SendError(Exception ex)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                Console.Error.WriteLine(ex.ToString());
            }
            else
            {
                using (var mailBox = new ProcessMailBox(channelName))
                {
                    mailBox.Content = new ExperimentError { Descriptor = -1, };
                }
            }
        }
    }
}
