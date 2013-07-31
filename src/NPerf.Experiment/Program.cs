namespace NPerf.Experiment
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Core.Tools;

    internal class Program
    {
        private static string channelName;

        public static void Main(string[] args)
        {
#if DEBUG
            //Debugger.Launch();
#endif
            AppDomain.CurrentDomain.AssemblyLoad += AssembliesManager.Loaded;
            AppDomain.CurrentDomain.AssemblyResolve += AssembliesManager.Resolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            var startParameters = new SartParameters(args);

            channelName = startParameters.ChannelName;

            AssembliesManager.LoadAssembly(startParameters.TesterAssembly);

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
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
/*#if DEBUG
            Debugger.Launch();
#endif*/

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
                using (var mailBox = new ProcessMailBox(channelName, TimeSpan.FromSeconds(5)))
                {
                    mailBox.Content = new ExperimentError(ex);
                }
            }
        }
    }
}
