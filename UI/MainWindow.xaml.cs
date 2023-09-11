using RAFFLE.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using System.Printing;
using RAFFLE.Utils;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        private bool bThreadStatus = false;

        private DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            lblCount.Text = "Count: " + SettingSchema.Count.ToString();
            lblTime.Text = "Time: " + SettingSchema.Time;
            lblPrice.Text = "Price: " + SettingSchema.Price.ToString();
            prgThread.Minimum = 0;
            Img.Source = SettingSchema.Img;

            bThreadStatus = false;
            timer.Interval = TimeSpan.FromSeconds(1); // Set the interval to 1 second
            timer.Tick += Timer_Tick; // Set the event handler
            Update();
        }

        private void Update()
        {
            if (bThreadStatus)
            {
                btnStart.Content = "Stop";
            }
            else
            {
                btnStart.Content = "Start";
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.Setting);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Builder.RaiseEvent(EventRaiseType.AppExit);
            MsgHelper.ShowMessage(MsgType.AppExit, "");
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void GetPrinters()
        {
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueueCollection printQueues = printServer.GetPrintQueues();

            foreach (PrintQueue printQueue in printQueues)
            {
                string printerName = printQueue.Name;
                // Add code here to store or display the printer names
                // You can use emoji to express your mood or attitude about the printers 🖨️📠🎨
            }
        }

        private void PrintDocument(string printerName, string documentPath)
        {
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueue printQueue = printServer.GetPrintQueue(printerName);

            // Create a PrintDialog to handle printing
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintQueue = printQueue;

            // Print the document
            //printDialog.PrintVisual();

            // You can handle print completion, errors, etc. using the PrintQueue's events
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            prgThread.Maximum = SettingSchema.Count;
            if (!bThreadStatus)
            {
                timer.Start();
                bThreadStatus = !bThreadStatus;
            }
            else
            {
                timer.Stop();
                bThreadStatus = !bThreadStatus;
            }
            Update();



            // start thread
            // Create a new thread
            //Thread thread = new Thread(() =>
            //{
            //    // Perform some background work
            //    timer.Start();

            //    // Update the TextBox control on the UI thread
            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        // prgThread.Value += 1;
            //        lblCount.Text = "";
            //    });
            //});

            //// Start the thread
            //thread.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Code to be executed at each interval
            prgThread.Value += 10;
            if (prgThread.Value >= SettingSchema.Count)
            {
                lblCurState.Text = SettingSchema.Count.ToString();
                Random random = new Random();
                timer.Stop();
                ResultSchema.WinnerNumber = (int)(SettingSchema.Count * random.NextDouble());
                ResultSchema.WinnerPrice = ResultSchema.WinnerNumber * SettingSchema.Price * (1 - SettingSchema.Rate / 100);
                ResultSchema.AdminPrice = ResultSchema.WinnerNumber * SettingSchema.Price * (SettingSchema.Rate / 100);
                ResultSchema.Img = SettingSchema.Img;
                Builder.RaiseEvent(EventRaiseType.Result);
            }
            lblCurState.Text = prgThread.Value.ToString() + " / " + SettingSchema.Count;

            Update();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.History);
        }
    }
}
