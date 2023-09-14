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
using System.Drawing.Printing;
using System.Drawing;
using System.Globalization;
using Wpf.Ui.Extensions;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        private bool bThreadStatus = false;

        private DispatcherTimer timer = new DispatcherTimer();
        private string sImpluse;
        private int curProgress = 0;
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            lblCount.Text = "Count: " + SettingSchema.Count.ToString();
            lblEndTime.Text = "Time: " + SettingSchema.Time == null ? getDateTimeFromString(SettingSchema.Time).ToString() : null;
            lblPrice.Text = "Price: " + SettingSchema.Price.ToString();
            Img.Source = SettingSchema.Img;

            bThreadStatus = false;
            timer.Interval = TimeSpan.FromSeconds(1); // Set the interval to 1 second
            timer.Tick += Timer_Tick; // Set the event handler
            prgThread.IsIndeterminate = false;
            Update();
        }

        public void UpdateState()
        {
            lblCount.Text = "Count: " + SettingSchema.Count.ToString();
            lblEndTime.Text = "Time: " + getDateTimeFromString(SettingSchema.Time).ToString();
            lblPrice.Text = "Price: " + SettingSchema.Price.ToString();
            Img.Source = SettingSchema.Img;
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

        private void PrintText(string text)
        {
            // Create a new PrintDocument object
            PrintDocument document = new PrintDocument();

            // Set the document name and print handler
            document.DocumentName = "Printing Test";
            document.PrintPage += (sender, e) =>
            {
                // Add your custom printing logic here
                // This event is triggered for each page that needs to be printed

                // Example: Print some text
                e.Graphics.DrawString(text, new Font("Arial", 24), System.Drawing.Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
            };

            // Start printing the document to the default printer
            document.Print();
            document.EndPrint += (sender, e) =>
            {
            };

        }

        private DateTime getDateTimeFromString(string dateString)
        {
            string inputFormat = "M/d/yyyyHH:mm";
            string outputFormat = "M/d/yyyy h:mm:ss tt";

            DateTime dateTime;
            string formattedDateTime = "";

            if (DateTime.TryParseExact(dateString, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                formattedDateTime = dateTime.ToString(outputFormat);
                //Console.WriteLine(formattedDateTime);
            }
            else
            {
                //Console.WriteLine("Invalid date format");
            }
            return Convert.ToDateTime(formattedDateTime);
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            if (!bThreadStatus)
            {
                timer.Start();
                bThreadStatus = !bThreadStatus;
                Builder.uiUserBoard.Update();
            }
            else
            {
                timer.Stop();
                bThreadStatus = !bThreadStatus;
            }
            Update();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            DateTime endTime = getDateTimeFromString(SettingSchema.Time);
            txtImpluse.Focus();
            if (curTime >= endTime) //  && curProgress <= SettingSchema.Count
            {
                // Code to be executed at each interval
                lblCurState.Text = SettingSchema.Count.ToString();
                Random random = new Random();
                timer.Stop();
                ResultSchema.WinnerNumber = (int)(SettingSchema.Count * random.NextDouble());
                ResultSchema.WinnerPrice = curProgress * SettingSchema.Price * (1 - SettingSchema.Rate / 100);
                ResultSchema.AdminPrice = curProgress * SettingSchema.Price * (SettingSchema.Rate / 100);
                ResultSchema.Img = SettingSchema.Img;
                ResultSchema.Remain = SettingSchema.Count - curProgress;
                Builder.RaiseEvent(EventRaiseType.Result);
                prgThread.IsIndeterminate = false;
                return;
            }
            if (curProgress < SettingSchema.Count)
            {
                // Print text
                if (sImpluse != "" && sImpluse != null && sImpluse.Length > 0)
                {
                    sImpluse = sImpluse.Substring(1);
                    txtImpluse.Text = sImpluse;
                    curProgress++;
                    PrintText("No: " + curProgress + "\nLocation: " + SettingSchema.Location + "\nDescription: " + SettingSchema.Description);
                }
                lblCurState.Text = curProgress + " / " + SettingSchema.Count;
            }            

            lblCurTime.Text = "Current Time: " + DateTime.Now.ToString();
            prgThread.IsIndeterminate = true;
            Update();
        }


        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.History);
        }

        private void txtImpluse_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            txtImpluse.Text += "+";
            sImpluse = txtImpluse.Text;
        }
    }
}
