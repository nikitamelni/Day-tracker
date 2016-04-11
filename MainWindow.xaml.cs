using System;
using System.Windows;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object lockingVar = new object();

        protected System.Timers.Timer mainTimer = new System.Timers.Timer();
        System.Diagnostics.Stopwatch StopwatchForTask { get; set; }
        public System.Timers.Timer MainTimer
        {
            get
            {
                return mainTimer;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            mainTimer = new System.Timers.Timer(1000);
            StopwatchForTask = new System.Diagnostics.Stopwatch();
        }

        public void RunEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                lblOutputTime.Content = StopwatchForTask.Elapsed.ToString("hh':'mm':'ss");
                double etaVal;
                double.TryParse(inptTextEta.Text, out etaVal);
                etaVal = TimeSpan.FromHours(etaVal).TotalSeconds;
                if ((etaVal > StopwatchForTask.Elapsed.Hours) && (etaVal != 0) && (StopwatchForTask.Elapsed.TotalSeconds != 0))
                {
                    var value = (StopwatchForTask.Elapsed.TotalSeconds / etaVal) * 100;
                    prgrsBarEta.Value = value;
                }
            }));
        }

        protected void btnStart_Copy_Click(object sender, RoutedEventArgs e)
        {
            StopwatchForTask.Stop();
            StopwatchForTask.Reset();
            mainTimer.Stop();
            lblOutputTime.Content = "00:00:00";
        }

        protected void btnStartAndPause_Click(object sender, RoutedEventArgs e)
        {
            if (!MainTimer.Enabled)
            {
                MainTimer.Elapsed += new System.Timers.ElapsedEventHandler(RunEvent);
                MainTimer.Enabled = true;
                StopwatchForTask.Start();
            }
            else
            {
                StopwatchForTask.Stop();
                MainTimer.Stop();
                lblOutputTime.Content += " Paused";
            }
        }

        protected void inptTaskName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (inptTaskName.Text.Equals("Put your task name here"))
            {
                inptTaskName.Text = "";
            }
        }

        protected void inptTaskName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(inptTaskName.Text))
            {
                inptTaskName.Text = "Put your task name here";
            }
        }
    }
}
