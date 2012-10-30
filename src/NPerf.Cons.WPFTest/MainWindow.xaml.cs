using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
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
        private PlotModel model1;
        private PlotModel model2;

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
        }

        public void CreateModel()
        {
            var tmp = new PlotModel("Simple example", "using OxyPlot");

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries("Series 1") { MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries("Series 2") { MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);

            // Axes are created automatically if they are not defined

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            model1 = tmp;

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BackgroundWorker _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerAsync();
        }

        // Worker Method
        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var plotData = NPerf.Cons.ReportGenerator.GetReportDataWithType(typeof(IDateRange));
            //Creating Model for Time
            var tmp = new PlotModel("Duration", "using OxyPlot and NPerf");
            //TO-DO: Need to select the test somehow
            var test = plotData.Tests[0];
            tmp.Title = "Duration (" + test.Name + ")";
            int typeIndex = 0;

            var runTotal = test.Runs[0];
            var series = (from PerfResult trun in runTotal.Results select new LineSeries(trun.TestedType) { MarkerType = MarkerType.Circle }).Cast<Series>().ToList();


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
            model1 = tmp;

            //Creating model for Memory
            var tmp1 = new PlotModel("Memory", "using OxyPlot and NPerf");
            typeIndex = 0;
            tmp1.Title = "Memory (" + test.Name + ")";
            var runTotal1 = test.Runs[0];
            var series1 = (from PerfResult trun in runTotal1.Results select new LineSeries(trun.TestedType) { MarkerType = MarkerType.Circle }).Cast<Series>().ToList();
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
                                                                                   run.Results[typeIndex].MemoryUsageMb));
                    }
                    typeIndex++;
                }
            }
            tmp1.Series = new System.Collections.ObjectModel.Collection<Series>(series1);

            model2 = tmp1;

            System.Windows.Threading.DispatcherOperation
           dispatcherOp = OxyPlotChartTime.Dispatcher.BeginInvoke(
           System.Windows.Threading.DispatcherPriority.Normal,
           new Action(
             delegate()
             {
                 OxyPlotChartTime.Model = model1;
             }
         ));
            System.Windows.Threading.DispatcherOperation
           dispatcherOp1 = OxyPlotChartTime.Dispatcher.BeginInvoke(
           System.Windows.Threading.DispatcherPriority.Normal,
           new Action(
             delegate()
             {
                 OxyPlotChartMemory.Model = model2;
             }
         ));

        }

    }
}
