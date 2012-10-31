using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NPerf.Cons.WPFTest.Types;
using NPerf.Core;
using OxyPlot;

namespace NPerf.Cons.WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool testsInProgress;

        public MainWindow()
        {
            InitializeComponent();
            //CreateModel();
            //OxyPlotChartTime.Model = model1;
            // Instantiate the writer  
            TextWriter _writer = new TextBoxStreamWriter(OutputTextbox);
            // Redirect the out Console stream  
            Console.SetOut(_writer);
            Console.WriteLine("Now redirecting console output to the text box");
            testsInProgress = false;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!testsInProgress)
            {
                //Removing charts
                ChartsGrid.Children.Clear();
                ChartsGrid.RowDefinitions.Clear();

                var _backgroundWorker = new BackgroundWorker();
                _backgroundWorker.DoWork += _backgroundWorker_DoWork;
                _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
                _backgroundWorker.RunWorkerAsync(AxisParameter.IsChecked.Value ? "true" : "false");
                testsInProgress = true;
            }
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.Threading.DispatcherOperation
                   dispatcherOp1 = LayoutRoot.Dispatcher.BeginInvoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       new Action(
                           delegate()
                           {
                               testsInProgress = false;
                           }
                           ));
        }

        // Worker Method
        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var plotData = NPerf.Cons.ReportGenerator.GetReportDataWithType(typeof (IDateRange));

            //Preparing Grid
            System.Windows.Threading.DispatcherOperation
                dispatcherOp = ChartsGrid.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(
                        delegate()
                            {
                                

                                //Adding components
                                var plot1 = new OxyPlot.Wpf.Plot() { Height = 500 };
                                plot1.Name = "chart0_0";
                                plot1.SetValue(Grid.RowProperty, 0);
                                plot1.SetValue(Grid.ColumnProperty, 0);
                                plot1.BorderBrush = new SolidColorBrush(Colors.Black);
                                plot1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                var split = new GridSplitter() {Width = 4};
                                var plot2 = new OxyPlot.Wpf.Plot() { Height = 500 };
                                plot2.Name = "chart0_1";
                                plot2.SetValue(Grid.RowProperty, 0);
                                plot2.SetValue(Grid.ColumnProperty, 1);
                                plot2.BorderBrush = new SolidColorBrush(Colors.Black);
                                plot2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                ChartsGrid.Children.Add(plot1);
                                ChartsGrid.Children.Add(split);
                                ChartsGrid.Children.Add(plot2);
                                ChartsGrid.Height = 700;
                                ChartsGrid.RowDefinitions.Add(new RowDefinition(){});
                                if (plotData.Tests.Count > 1)
                                {
                                    for (int i = 1; i < plotData.Tests.Count; i++)
                                    {
                                        var plot3 = new OxyPlot.Wpf.Plot() { Height = 500 };
                                        plot3.Name = String.Format("chart{0}_0", i);
                                        plot3.SetValue(Grid.RowProperty, i);
                                        plot3.SetValue(Grid.ColumnProperty, 0);
                                        plot3.BorderBrush = new SolidColorBrush(Colors.Black);
                                        plot3.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                        var split1 = new GridSplitter() {Width = 4};
                                        split1.SetValue(Grid.RowProperty, i);
                                        var plot4 = new OxyPlot.Wpf.Plot() { Height = 500 };
                                        plot4.Name = String.Format("chart{0}_1", i);
                                        plot4.SetValue(Grid.RowProperty, i);
                                        plot4.SetValue(Grid.ColumnProperty, 1);
                                        plot4.BorderBrush = new SolidColorBrush(Colors.Black);
                                        plot4.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                        ChartsGrid.Children.Add(plot3);
                                        ChartsGrid.Children.Add(split1);
                                        ChartsGrid.Children.Add(plot4);
                                        ChartsGrid.RowDefinitions.Add(new RowDefinition() { });
                                    }
                                    ChartsGrid.Height = plotData.Tests.Count * 500 + plotData.Tests.Count * 4;
                                }
                                ChartsGrid.UpdateLayout();
                            }));
            //TO-DO: Need to select the test somehow
            for (int testIndex = 0; testIndex < plotData.Tests.Count; testIndex++)
            {
                var test = plotData.Tests[testIndex];
                //Creating Model for Time
                var tmp = new PlotModel("Duration", "using OxyPlot and NPerf");
                if ((e.Argument as string).Equals("true"))
                {
                    tmp.Axes.Add(new LogarithmicAxis(AxisPosition.Right,"Logarithmic"));
                }
                tmp.Title = "Duration (" + test.Name + ")";
                int typeIndex = 0;
                int testIndexInner = testIndex;

                var runTotal = test.Runs[0];
                var series =
                    (from PerfResult trun in runTotal.Results
                     select new LineSeries(trun.TestedType) {MarkerType = MarkerType.Circle}).Cast<Series>().ToList();


                foreach (var run in test.Runs)
                {
                    typeIndex = 0;
                    foreach (var trun in run.Results)
                    {
                        if (typeIndex < run.Results.Count)
                        {
                            //Series name: run.Results[index].testedtype
                            // Value -> run.Results[index]
                            (series[typeIndex] as LineSeries).Points.Add(new DataPoint(run.Value,
                                                                                       run.Results[typeIndex].Duration));
                        }
                        typeIndex++;
                    }
                }
                tmp.Series = new System.Collections.ObjectModel.Collection<Series>(series);

                //Creating model for Memory
                var tmp1 = new PlotModel("Memory", "using OxyPlot and NPerf");
                if ((e.Argument as string).Equals("true"))
                {
                    tmp1.Axes.Add(new LogarithmicAxis(AxisPosition.Right, "Logarithmic"));
                }
                typeIndex = 0;
                tmp1.Title = "Memory (" + test.Name + ")";
                var runTotal1 = test.Runs[0];
                var series1 =
                    (from PerfResult trun in runTotal1.Results
                     select new LineSeries(trun.TestedType) {MarkerType = MarkerType.Circle}).Cast<Series>().ToList();
                foreach (var run in test.Runs)
                {
                    typeIndex = 0;
                    foreach (var trun in run.Results)
                    {
                        if (typeIndex < run.Results.Count)
                        {
                            //Series name: run.Results[index].testedtype
                            // Value -> run.Results[index]
                            (series1[typeIndex] as LineSeries).Points.Add(new DataPoint(run.Value,
                                                                                        run.Results[typeIndex].
                                                                                            MemoryUsageMb));
                        }
                        typeIndex++;
                    }
                }
                tmp1.Series = new System.Collections.ObjectModel.Collection<Series>(series1);

                System.Windows.Threading.DispatcherOperation
                    dispatcherOp1 = ChartsGrid.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(
                            delegate()
                                {
                                    //Setting models
                                    foreach (var el in ChartsGrid.Children)
                                    {
                                        if (el is OxyPlot.Wpf.Plot && (el as OxyPlot.Wpf.Plot).Name.Equals(String.Format("chart{0}_0", testIndexInner)))
                                        {
                                            (el as OxyPlot.Wpf.Plot).Model = tmp;
                                            break;
                                        }
                                    }
                                    foreach (var el in ChartsGrid.Children)
                                    {
                                        if (el is OxyPlot.Wpf.Plot && (el as OxyPlot.Wpf.Plot).Name.Equals(String.Format("chart{0}_1", testIndexInner)))
                                        {
                                            (el as OxyPlot.Wpf.Plot).Model = tmp1;
                                            break;
                                        }
                                    }
                                }
                            ));
            }
        }

    }
}
