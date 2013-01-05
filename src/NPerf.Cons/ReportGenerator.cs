namespace NPerf.Cons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using NPerf.Core;
    using NPerf.Core.Tracer;
    using NPerf.Report;

    using ColorMap = NPerf.Report.ColorMap;

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ReportGenerator
    {
        private int currentTestNumber;

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="args">The arguments to parse. Like in console application before.</param>
        public void GenerateReportWithType(Type type)
        {
            try
            {
                // loading testers
                if (type == null)
                {
                    throw new Exception("You did not specify a tester assembly");
                }

                // load testers from current assembly
                IList<PerfTester> testers = new List<PerfTester>();
                LoadTesters(testers, Assembly.GetCallingAssembly());

                if (testers.Count == 0)
                {
                    throw new Exception("Could not find any tester class");
                }

                // load tested
                foreach (var tester in testers)
                {
                    tester.IsRunDescriptorValueOveridden = false;
                    if (tester.IsIgnored)
                    {
                        continue;
                    }

                    Console.WriteLine("Load tested assembly: {0}", Assembly.GetCallingAssembly().FullName);

                    // Looking inside current
                    tester.LoadTestedTypesFromInner(Assembly.GetCallingAssembly());

                    // Looking inside referenced
                    foreach (var asm in Assembly.GetCallingAssembly().GetReferencedAssemblies())
                    {
                        tester.LoadTestedTypesFromInner(Assembly.Load(asm));
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (var t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }
                }

                // run test
                var chart = new ChartReport(800, 400);
                chart.Colors.Map = ColorMap.Jet;
                var im = ImageFormat.Png;

                foreach (var tester in testers)
                {
                    if (tester.IsIgnored)
                    {
                        continue;
                    }

                    var tracer = new TextWriterTracer();
                    tracer.Attach(tester);

                    var suite = tester.RunTests();

                    var bmps = chart.Render(suite);
                    foreach (DictionaryEntry de in bmps)
                    {
                        var test = (PerfTest)de.Key;
                        var bmp = (Bitmap)de.Value;
                        bmp.Save(tester.TestedType.Name + "." + test.Name + "." + im.ToString().ToLower(), im);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the report data for test Type.
        /// </summary>
        /// <param name="type">The type of tested object.</param>
        public PerfTestSuite GetReportDataWithType(Type type)
        {
            try
            {
                // loading testers
                if (type == null)
                {
                    throw new Exception("You did not specify a tester assembly");
                }

                // load testers from current assembly
                IList<PerfTester> testers = new List<PerfTester>();
                LoadTesters(testers, Assembly.GetCallingAssembly());

                if (testers.Count == 0)
                {
                    throw new Exception("Could not find any tester class");
                }

                // load tested
                foreach (var tester in testers)
                {
                    tester.IsRunDescriptorValueOveridden = false;
                    if (tester.IsIgnored)
                    {
                        continue;
                    }

                    Console.WriteLine("Load tested assembly: {0}", Assembly.GetCallingAssembly().FullName);

                    // Looking inside current
                    tester.LoadTestedTypesFromInner(Assembly.GetCallingAssembly());

                    // Looking inside referenced
                    foreach (var asm in Assembly.GetCallingAssembly().GetReferencedAssemblies())
                    {
                        tester.LoadTestedTypesFromInner(Assembly.Load(asm));
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (var t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }

                }

                foreach (var tester in testers)
                {
                    if (tester.IsIgnored)
                    {
                        continue;
                    }

                    var tracer = new TextWriterTracer();
                    tracer.Attach(tester);

                    var suite = tester.RunTests();
                    return suite;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Gets the report data for test Type.
        /// </summary>
        /// <param name="type">The type of tested object.</param>
        /// <param name="startValue">The start value.</param>
        /// <param name="step">The step.</param>
        /// <param name="countOfRuns">The count of runs.</param>
        /// <returns></returns>
        public void GetReportDataWithType(Type type,int startValue, int step,  int countOfRuns)
        {
            try
            {
                // loading testers
                if (type == null)
                {
                    throw new Exception("You did not specify a tester assembly");
                }

                // load testers from current assembly
                IList<PerfTester> testers = new List<PerfTester>();
                LoadTesters(testers, Assembly.GetCallingAssembly());

                if (testers.Count == 0)
                {
                    throw new Exception("Could not find any tester class");
                }

                // load tested
                foreach (var tester in testers)
                {
                    tester.IsRunDescriptorValueOveridden = false;
                    if (tester.IsIgnored)
                    {
                        continue;
                    }

                    Console.WriteLine("Load tested assembly: {0}", Assembly.GetCallingAssembly().FullName);

                    // Looking inside current
                    tester.LoadTestedTypesFromInner(Assembly.GetCallingAssembly());

                    // Looking inside referenced
                    foreach (var asm in Assembly.GetCallingAssembly().GetReferencedAssemblies())
                    {
                        tester.LoadTestedTypesFromInner(Assembly.Load(asm));
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (var t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }
                }

                this.RunTests(testers, startValue, step, countOfRuns);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the report data for specified assemblies.
        /// </summary>
        /// <param name="testerAssemblies">The assemblies tester types.</param>
        /// <param name="testedAssemblies">The assemblies tested types.</param>
        /// <param name="startValue">The start value.</param>
        /// <param name="step">The step.</param>
        /// <param name="countOfRuns">The count of runs.</param>
        /// <exception cref="System.Exception">You did not specify a tester assembly(s)</exception>
        public void GetReportDataWithAssemblies(Assembly[] testerAssemblies, Assembly[] testedAssemblies, int startValue, int step, int countOfRuns)
        {
            try
            {
                if ((testerAssemblies == null) || (testerAssemblies.Length == 0))
                {
                    throw new Exception("You did not specify a tester assembly(s)");
                }

                if ((testedAssemblies == null) || (testedAssemblies.Length == 0))
                {
                    throw new Exception("You did not specify a tested assembly(s)");
                }

                IList<PerfTester> testers = new List<PerfTester>();
                LoadTesters(testers, testerAssemblies);

                if (testers.Count == 0)
                {
                    throw new Exception("Could not find any tester class");
                }
                
                LoadTestedTypes(testedAssemblies, testers);

                this.RunTests(testers, startValue, step, countOfRuns);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RunTests(IEnumerable<PerfTester> testers, int startValue, int step, int countOfRuns)
        {
            foreach (var tester in testers)
            {
                tester.IsRunDescriptorValueOveridden = true;
                if (tester.IsIgnored)
                {
                    continue;
                }

                if (startValue > 0)
                {
                    tester.TestStart = startValue;
                }

                if (step > 0)
                {
                    tester.TestStep = step;
                }

                if (countOfRuns > 0)
                {
                    tester.TestCount = countOfRuns;
                }

                tester.ResultsChange += this.tester_ResultsChange;
                var tracer = new TextWriterTracer();
                tracer.Attach(tester);

                var suite = tester.RunTests();
            }
        }

        private static void LoadTestedTypes(Assembly[] testedAssemblies, IEnumerable<PerfTester> testers)
        {
            foreach (var tester in testers)
            {
                tester.IsRunDescriptorValueOveridden = false;
                if (tester.IsIgnored)
                {
                    continue;
                }

                Console.WriteLine("Loading tested types for: {0}", tester.TesterType);

                foreach (var testedAssembly in testedAssemblies)
                {
                    Console.WriteLine("Load tested assembly: {0}", testedAssembly);
                    tester.LoadTestedTypesFromInner(testedAssembly);
                }

                Console.WriteLine("Tested types:");
                Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                foreach (var t in tester.TestedTypes)
                {
                    Console.WriteLine("\t{0}", t.Name);
                }
            }
        }

        private static void LoadTesters(IList<PerfTester> testers, Assembly[] testerAssemblies)
        {
            foreach (var testerAssembly in testerAssemblies)
            {
                LoadTesters(testers, testerAssembly);
            }
        }

        private static void LoadTesters(IList<PerfTester> testers, Assembly testerAssembly)
        {
            Console.WriteLine("Load tester assembly: {0}", testerAssembly.GetName());
            PerfTester.FromAssembly(testers, testerAssembly);
        }

        public class ResultsChangedEventArgs : EventArgs
        {
            public PerfTestSuite CurrentResults { get; private set; }

            public ResultsChangedEventArgs(PerfTestSuite results)
            {
                this.CurrentResults = results;
            }
        }

        //Delegate
        public delegate void ResultsChangedHandler(object sender, ResultsChangedEventArgs results);

        //Event
        public event ResultsChangedHandler ResultsChange;

        //Method for firing Event
        protected void OnResultsChange(object sender, ResultsChangedEventArgs results)
        {
            // Check if there are any Subscribers  
            if (this.ResultsChange != null)
            {      // Call the Event   
                this.ResultsChange(this, results);
            }
        }

        void tester_ResultsChange(object sender, PerfTester.ResultsChangeEventArgs results)
        {
            //Updating Charts
            this.OnResultsChange(this, new ResultsChangedEventArgs(results.CurrentResults));
        }

        public double CustomRun(int testIndex)
        {
            return this.currentTestNumber * (testIndex + 1);
        }

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="args">The arguments to parse. Like in console application before.</param>
        public void GenerateReport(string[] args)
        {
            try
            {
                var cl = new ClArguments();
                cl.AddParameter(new ClParameter("testera", "ta", "Tester Assembly", false, false));
                cl.AddParameter(new ClParameter("testeda", "tdfa", "Tested Assembly", false, false));
                cl.AddParameter(new ClParameter("testedapartial", "tdfap", "Tested Assembly Parital Name", false, false));
                cl.AddParameter(new ClParameter("ignoredtype", "it", "Ingored types", false, false));
                cl.AddParameter(new ClParameter("verbose", "v", "Verbose", true, true));
                cl.AddParameter(new ClParameter("outputtype", "ot", "Output type", true, false));
                cl.AddParameter(new ClParameter("xml", "x", "Output as XML", true, true));

                cl.Parse(args);
                cl.ProcessHelp();


                // loading testers
                if (!cl.ContainsDuplicate("ta"))
                {
                    throw new Exception("You did not specify a tester assembly");
                }

                // load testers
                IList<PerfTester> testers = new List<PerfTester>();
                foreach (var name in cl.Duplicates("ta"))
                {
                    Console.WriteLine("Load tester assembly: {0}", name);
                    PerfTester.FromAssembly(testers, Assembly.LoadFrom(name));
                }

                if (testers.Count == 0)
                {
                    throw new Exception("Could not find any tester class");
                }

                // load tested
                foreach (var tester in testers.Where(tester => !tester.IsIgnored))
                {
                    if (cl.ContainsDuplicate("tdfa"))
                    {
                        foreach (string name in cl.Duplicates("tdfa"))
                        {
                            Console.WriteLine("Load tested assembly: {0}", name);
                            tester.LoadTestedTypes(Assembly.LoadFrom(name));
                        }
                    }

                    if (cl.ContainsDuplicate("tdfap"))
                    {
                        foreach (string name in cl.Duplicates("tdfap"))
                        {
                            Console.WriteLine("Load tested assembly: {0}", name);
                            tester.LoadTestedTypes(Assembly.Load(name));
                        }
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (var t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }

                    // removing types
                    if (!cl.ContainsDuplicate("it"))
                    {
                        continue;
                    }

                    foreach (var name in cl.Duplicates("it"))
                    {
                        Console.Write("Ignoring type: <{0}>", name);
                        var t = Type.GetType(name);
                        if (t == null)
                        {
                            Console.WriteLine(" - could not load type.");
                        }
                        else
                        {
                            if (tester.TestedTypes.Contains(t))
                            {
                                Console.WriteLine(" removed");
                                tester.TestedTypes.Remove(t);
                            }
                            else
                            {
                                Console.WriteLine("-- could not remove {0}", t.Name);
                            }
                        }
                    }
                }

                // run test
                var chart = new ChartReport(800, 400);
                chart.Colors.Map = ColorMap.Jet;
                var im = ImageFormat.Png;

                if (cl.ContainsUnique("ot"))
                {
                    switch (cl.Unique("ot").ToLower())
                    {
                        case "png":
                            im = ImageFormat.Png;
                            break;
                        case "gif":
                            im = ImageFormat.Gif;
                            break;
                        case "jpeg":
                            im = ImageFormat.Jpeg;
                            break;
                        case "bmp":
                            im = ImageFormat.Bmp;
                            break;
                        case "emf":
                            im = ImageFormat.Emf;
                            break;
                    }
                }

                foreach (var tester in testers.Where(tester => !tester.IsIgnored))
                {
                    var tracer = new TextWriterTracer();
                    tracer.Attach(tester);

                    var suite = tester.RunTests();

                    if (cl.ContainsUnique("xml"))
                    {
                        using (var file = File.OpenWrite(tester.TestedType.Name + "." + suite.Name + ".xml"))
                        {
                            var writer = new StreamWriter(file);
                            suite.ToXml(writer);
                            writer.Close();
                        }
                    }

                    var bmps = chart.Render(suite);
                    foreach (DictionaryEntry de in bmps)
                    {
                        var test = (PerfTest)de.Key;
                        var bmp = (Bitmap)de.Value;
                        bmp.Save(
                            string.Format("{0}.{1}.{2}", tester.TestedType.Name, test.Name, im.ToString().ToLower()), im);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
