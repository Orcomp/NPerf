namespace NPerf.Core
{
    using System;
    using System.Linq;
    using Fasterflect;
    using NPerf.Core.Monitoring;
    using NPerf.Framework;
    using System.Collections.Generic;
using System.Diagnostics;

    /// <summary>
    /// Summary description for PerfTester.
    /// </summary>
    public class PerfTester
    {
        private readonly System.Reflection.MethodInfo runDescriptor = null;

        private readonly System.Reflection.MethodInfo setUp = null;

        private readonly System.Reflection.MethodInfo tearDown = null;

        private readonly System.Reflection.MethodInfo[] methods;

        private readonly TimeMonitor timer;        

        private readonly MemoryMonitor memorizer;

        private PerformanceCounter cpuCounter;

        private PerformanceCounter memCounter;

        public PerfTester(Type testerType, PerfTesterAttribute attr)
        {
            if (testerType == null)
            {
                throw new ArgumentNullException("testerType");
            }

            if (attr == null)
            {
                throw new ArgumentNullException("attr");
            }

            this.TesterType = testerType;
            this.TestedType = attr.TestedType;
            this.TestCount = attr.TestCount;
            this.Description = attr.Description;
            this.FeatureDescription = attr.FeatureDescription;

            this.cpuCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
            this.memCounter = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);

            // get run descriptor
            this.runDescriptor =
                this.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfRunDescriptorAttribute))
                    .FirstOrDefault(
                        m => m.ReturnType == typeof(double) && m.HasParameterSignature(new[] { typeof(int) }));

            // get set up
            this.setUp =
                this.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfSetUpAttribute))
                    .FirstOrDefault(
                        m => m.ReturnType == typeof(void) && m.HasParameterSignature(new[] { typeof(int), this.TestedType }));

            // get tear down
            this.tearDown =
                this.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfTearDownAttribute))
                    .FirstOrDefault(
                        m => m.ReturnType == typeof(void) && m.HasParameterSignature(new[] { this.TestedType }));

            // get test method
            this.methods =
                this.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfTestAttribute))
                    .Where(m => m.ReturnType == typeof(void) && m.HasParameterSignature(new[] { this.TestedType }))
                    .ToArray();

            this.TestedTypes = new List<Type>();
            this.timer = new TimeMonitor();
            this.memorizer = new MemoryMonitor();
        }

        public Type TesterType { get; private set; }

        public Type TestedType { get; private set; }

        public string Description { get; private set; }

        public string FeatureDescription { get; private set; }

        #region Custom Runs

        public int TestCount { get; set; }

        public int TestStart { get; set; }

        public int TestStep { get; set; }

        public bool IsRunDescriptorValueOveridden { get; set; }

        #endregion


        public IList<Type> TestedTypes { get; private set; }

        public bool IsIgnored
        {
            get
            {
                return this.TesterType.HasAttribute<PerfIgnoreAttribute>();
            }
        }

        public string IgnoreMessage
        {
            get
            {
                var attr = this.TesterType.Attribute<PerfIgnoreAttribute>();
                return attr.Message;
            }
        }

        #region Events

        public event PerfTestEventHandler StartTest;

        protected void OnStartTest(PerfTest test)
        {
            if (this.StartTest != null)
            {
                this.StartTest(this, new PerfTestEventArgs(test));
            }
        }

        public event PerfTestEventHandler FinishTest;

        protected void OnFinishTest(PerfTest test)
        {
            if (this.FinishTest != null)
            {
                this.FinishTest(this, new PerfTestEventArgs(test));
            }
        }

        public event PerfTestEventHandler IgnoredTest;

        protected void OnIgnoredTest(PerfTest test)
        {
            if (this.IgnoredTest != null)
            {
                this.IgnoredTest(this, new PerfTestEventArgs(test));
            }
        }

        public event PerfTestRunEventHandler StartRun;

        protected void OnStartRun(PerfTestRun run)
        {
            if (this.StartRun != null)
            {
                this.StartRun(this, new PerfTestRunEventArgs(run));
            }
        }

        public event PerfTestRunEventHandler FinishRun;

        protected void OnFinishRun(PerfTestRun run)
        {
            if (this.FinishRun != null)
            {
                this.FinishRun(this, new PerfTestRunEventArgs(run));
            }
        }

        #endregion

        public void LoadTestedTypes(System.Reflection.Assembly a)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            this.TestedTypes =
                a.GetExportedTypes()
                 .Where(
                     t =>
                     this.TestedType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                     && t.Constructor(Type.EmptyTypes) != null)
                 .ToList();
            /*(this.TestedType.IsInterface
                     ? a.GetExportedTypes().Where(t => this.TestedType.IsAssignableFrom(t) && !t.IsAbstract)
                     : a.GetExportedTypes().Where(t => t.IsInstanceOfType(this.TestedType) && !t.IsAbstract)).ToList();*/
        }

        public void LoadTestedTypesFromInner(System.Reflection.Assembly a)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            this.TestedTypes =
                a.GetTypes()
                 .Where(
                     t =>
                     this.TestedType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                     && t.Constructor(Type.EmptyTypes) != null)
                 .ToList();
            /*(this.TestedType.IsInterface
                     ? a.GetTypes().Where(t => this.TestedType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                     : a.GetTypes().Where(t => t.IsInstanceOfType(this.TestedType) && !t.IsAbstract)).ToList();*/
        }

        #region Custom Event

        /// <summary>
        /// Our Cutsom Events arguments that has current Results of the Tests.
        /// </summary>
        public class ResultsChangeEventArgs : EventArgs
        {
            public PerfTestSuite CurrentResults { get; internal set; }

            public ResultsChangeEventArgs(PerfTestSuite results)
            {
                CurrentResults = results;
            }
        }

        //Delegate
        public delegate void ResultsChangeHandler(object sender, ResultsChangeEventArgs results);

        //Event
        public event ResultsChangeHandler ResultsChange;

        // Method for firing Event
        protected void OnResultsChange(object sender, ResultsChangeEventArgs results)
        {
            // Check if there are any Subscribers  
            if (ResultsChange != null)
            {
                // Call the Event   
                ResultsChange(this, results);
            }
        }

        #endregion

        public PerfTestSuite RunTests()
        {
            var suite = new PerfTestSuite(this.TesterType, this.Description, this.FeatureDescription);

            for (var testIndex = 0; testIndex < this.methods.Length; testIndex++)
            {
                var test = this.methods[testIndex];
                var testResult = new PerfTest(test);

                // Adding from the start
                suite.Tests.Add(testResult);

                if (testResult.IsIgnored)
                {
                    this.OnIgnoredTest(testResult);
                    suite.Tests.Add(testResult);
                    continue;
                }

                this.OnStartTest(testResult);

                for (var runIndex = 0; runIndex < this.TestCount; ++runIndex)
                {
                    var run =
                        new PerfTestRun(
                            this.IsRunDescriptorValueOveridden
                                ? this.TestStart + runIndex * this.TestStep
                                : this.RunDescription(runIndex));

                    this.OnStartRun(run);

                    // for each instanced type,
                    foreach (var t in this.TestedTypes)
                    {
                        try
                        {
                            // jitting if first run of the test
                            if (runIndex == 0)
                            {
                                this.RunTest(-1, t, test, false);
                            }

                            // calling
                            this.RunTest(runIndex, t, test, true);

                            // save results
                            run.Results.Add(new PerfResult(t, this.timer.Duration, this.memorizer.Usage));
                        }
                        catch (Exception ex)
                        {
                            run.FailedResults.Add(new PerfFailedResult(t, ex));
                        }
                    }

                    this.OnFinishRun(run);
                    testResult.Runs.Add(run);

                    // Adding to suite
                    suite.Tests[testIndex] = testResult;
                    
                    // Invoking Event Handler
                    this.OnResultsChange(this, new ResultsChangeEventArgs(suite));
                }

                this.OnFinishTest(testResult);
                
                // suite.Tests[testIndex] = testResult;
            }

            return suite;
        }

        private void RunTest(int testIndex, Type testedType, System.Reflection.MethodInfo method, bool monitor)
        {
            // create instance
            var tested = testedType.CreateInstance(Type.EmptyTypes);
            
            // test 
            var tester = this.TestedType.CreateInstance(Type.EmptyTypes);
            this.SetUp(testIndex, tester, tested);

            // clean memory
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // starts monitoring
            if (monitor)
            {
                this.memorizer.Start();
                this.timer.Start();
            }

            method.Call(tester, tested);

            // stop monitoring
            if (monitor)
            {
                this.timer.Stop();
                this.memorizer.Stop();
            }

            // tear down
            this.TearDown(tester, tested);
        }

        #region Static Helpers

        public static void FromAssembly(IList<PerfTester> testers, System.Reflection.Assembly a)
        {
            Console.WriteLine("FromAssembly enter");
            if (testers == null)
            {
                throw new ArgumentNullException("testers");
            }

            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            foreach (var t in a.GetExportedTypes())
            {
                if (t.HasAttribute<PerfTesterAttribute>())
                {
                    Console.WriteLine("PerfTesterAttribute {0}", t);
                    var attr = t.Attribute<PerfTesterAttribute>();
                    Console.WriteLine("PerfTesterAttribute {0}", attr.Description);
                    var tester = new PerfTester(t, attr);

                    testers.Add(tester);
                }
                else
                {
                    Console.WriteLine("No custom attr");
                }
            }
        }

        public static IList<PerfTester> FromAssembly(System.Reflection.Assembly a)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            var testers = new List<PerfTester>();
            FromAssembly(testers, a);

            return testers;
        }

        #endregion

        #region Protected

        protected double RunDescription(int testIndex)
        {
            if (this.runDescriptor == null)
            {
                return testIndex;
            }
            
            var tester = this.TestedType.CreateInstance(Type.EmptyTypes);
            return (double)this.runDescriptor.Call(tester, testIndex);
        }

        protected void SetUp(int testIndex, object tester, object tested)
        {
            if (tester == null)
            {
                throw new ArgumentNullException("tester");
            }

            if (tested == null)
            {
                throw new ArgumentNullException("tested");
            }

            if (this.setUp != null)
            {
                this.setUp.Call(tester, testIndex, tested);
            }
        }

        protected void TearDown(object tester, object tested)
        {
            if (tester == null)
            {
                throw new ArgumentNullException("tester");
            }

            if (tested == null)
            {
                throw new ArgumentNullException("tested");
            }

            if (this.tearDown != null)
            {
                this.tearDown.Call(tester, tested);
            }
        }

        #endregion
    }
}
