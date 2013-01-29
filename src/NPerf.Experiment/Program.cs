namespace NPerf.Experiment
{
    using System;
    using System.Linq;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Framework.Interfaces;

    internal class Program
    {
        private static string channelName;

        public static void Main(string[] args)
        {
            /*
             * -suiteLib {0} -suiteType {1} -testMethod {2} -subjectAssm {3} -subjecType {4} -channelName {5} -start {6} -step {7} -end {8}
             */

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            var startParameters = new SartParameters(args);

            channelName = startParameters.ChannelName;

            if (string.IsNullOrEmpty(channelName))
            {
                SendError(new Exception("Mailbox name for interprocess communication was not set."));
                return;
            }

            ExperimentScope.Start(startParameters);
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
                using (var mailBox = new ProcessMailBox(channelName, 1024))
                {
                    mailBox.Content = new ExperimentError { Descriptor = -1, };
                }
            }
        }
    }
}
