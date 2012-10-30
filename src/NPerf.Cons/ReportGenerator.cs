using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;

namespace NPerf.Cons
{
    using NPerf.Core;
    using NPerf.Core.Collections;
    using NPerf.Report;
    using NPerf.Core.Tracer;

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ReportGenerator
    {
        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="args">The arguments to parse. Like in console application before.</param>
        public static void GenerateReportWithType(Type type)
        {
            try
            {
                // loading testers
                if (type == null)
                    throw new Exception("You did not specify a tester assembly");

                // load testers from current assembly
                PerfTesterCollection testers = new PerfTesterCollection();
                Console.WriteLine("Load tester assembly: {0}", Assembly.GetCallingAssembly().GetName());
                PerfTester.FromAssembly(
                      testers,
                      Assembly.GetCallingAssembly()
                      );

                if (testers.Count == 0)
                    throw new Exception("Could not find any tester class");

                // load tested
                foreach (PerfTester tester in testers)
                {
                    if (tester.IsIgnored)
                        continue;

                    Console.WriteLine("Load tested assembly: {0}", Assembly.GetCallingAssembly().FullName);
                    //Looking inside current
                    tester.LoadTestedTypesFromInner(Assembly.GetCallingAssembly());
                    //Looking inside referenced
                    foreach (var asm in Assembly.GetCallingAssembly().GetReferencedAssemblies())
                    {
                        tester.LoadTestedTypesFromInner(Assembly.Load(asm));
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (Type t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }

                }

                // run test
                ChartReport chart = new ChartReport(800, 400);
                chart.Colors.Map = ColorMap.Jet;
                ImageFormat im = ImageFormat.Png;

                foreach (PerfTester tester in testers)
                {
                    if (tester.IsIgnored)
                        continue;

                    TextWriterTracer tracer = new TextWriterTracer();
                    tracer.Attach(tester);

                    PerfTestSuite suite = tester.RunTests();

                    IDictionary bmps = chart.Render(suite);
                    foreach (DictionaryEntry de in bmps)
                    {
                        PerfTest test = (PerfTest)de.Key;
                        Bitmap bmp = (Bitmap)de.Value;
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
        public static PerfTestSuite GetReportDataWithType(Type type)
        {
            try
            {
                // loading testers
                if (type == null)
                    throw new Exception("You did not specify a tester assembly");

                // load testers from current assembly
                PerfTesterCollection testers = new PerfTesterCollection();
                Console.WriteLine("Load tester assembly: {0}", Assembly.GetCallingAssembly().GetName());
                PerfTester.FromAssembly(
                      testers,
                      Assembly.GetCallingAssembly()
                      );

                if (testers.Count == 0)
                    throw new Exception("Could not find any tester class");

                // load tested
                foreach (PerfTester tester in testers)
                {
                    if (tester.IsIgnored)
                        continue;

                    Console.WriteLine("Load tested assembly: {0}", Assembly.GetCallingAssembly().FullName);
                    //Looking inside current
                    tester.LoadTestedTypesFromInner(Assembly.GetCallingAssembly());
                    //Looking inside referenced
                    foreach (var asm in Assembly.GetCallingAssembly().GetReferencedAssemblies())
                    {
                        tester.LoadTestedTypesFromInner(Assembly.Load(asm));
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (Type t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }

                }

                foreach (PerfTester tester in testers)
                {
                    if (tester.IsIgnored)
                        continue;

                    TextWriterTracer tracer = new TextWriterTracer();
                    tracer.Attach(tester);

                    PerfTestSuite suite = tester.RunTests();
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
        /// Generates the report.
        /// </summary>
        /// <param name="args">The arguments to parse. Like in console application before.</param>
        public static void GenerateReport(string[] args)
        {
            try
            {
                ClArguments cl = new ClArguments();
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
                    throw new Exception("You did not specify a tester assembly");

                // load testers
                PerfTesterCollection testers = new PerfTesterCollection();
                foreach (String name in cl.Duplicates("ta"))
                {
                    Console.WriteLine("Load tester assembly: {0}", name);
                    PerfTester.FromAssembly(
                          testers,
                          Assembly.LoadFrom(name)
                          );
                }
                if (testers.Count == 0)
                    throw new Exception("Could not find any tester class");

                // load tested
                foreach (PerfTester tester in testers)
                {
                    if (tester.IsIgnored)
                        continue;

                    if (cl.ContainsDuplicate("tdfa"))
                    {
                        foreach (String name in cl.Duplicates("tdfa"))
                        {
                            Console.WriteLine("Load tested assembly: {0}", name);
                            tester.LoadTestedTypes(Assembly.LoadFrom(name));
                        }
                    }

                    if (cl.ContainsDuplicate("tdfap"))
                    {
                        foreach (String name in cl.Duplicates("tdfap"))
                        {
                            Console.WriteLine("Load tested assembly: {0}", name);
                            tester.LoadTestedTypes(Assembly.Load(name));
                        }
                    }

                    Console.WriteLine("Tested types:");
                    Console.WriteLine(tester.TestedTypes.Count + " " + tester.TesterType);
                    foreach (Type t in tester.TestedTypes)
                    {
                        Console.WriteLine("\t{0}", t.Name);
                    }

                    // removing types
                    if (cl.ContainsDuplicate("it"))
                    {
                        foreach (String name in cl.Duplicates("it"))
                        {
                            Console.Write("Ignoring type: <{0}>", name);
                            Type t = Type.GetType(name);
                            if (t == null)
                                Console.WriteLine(" - could not load type.");
                            else
                            {
                                if (tester.TestedTypes.Contains(t))
                                {
                                    Console.WriteLine(" removed");
                                    tester.TestedTypes.Remove(t);
                                }
                                else
                                    Console.WriteLine("-- could not remove {0}", t.Name);
                            }
                        }
                    }
                }

                // run test
                ChartReport chart = new ChartReport(800, 400);
                chart.Colors.Map = ColorMap.Jet;
                ImageFormat im = ImageFormat.Png;

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

                foreach (PerfTester tester in testers)
                {
                    if (tester.IsIgnored)
                        continue;

                    TextWriterTracer tracer = new TextWriterTracer();
                    tracer.Attach(tester);

                    PerfTestSuite suite = tester.RunTests();

                    if (cl.ContainsUnique("xml"))
                    {
                        using (FileStream file = File.OpenWrite(tester.TestedType.Name + "." + suite.Name + ".xml"))
                        {
                            StreamWriter writer = new StreamWriter(file);
                            suite.ToXml(writer);
                            writer.Close();
                        }
                    }

                    IDictionary bmps = chart.Render(suite);
                    foreach (DictionaryEntry de in bmps)
                    {
                        PerfTest test = (PerfTest)de.Key;
                        Bitmap bmp = (Bitmap)de.Value;
                        bmp.Save(tester.TestedType.Name + "." + test.Name + "." + im.ToString().ToLower(), im);
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
