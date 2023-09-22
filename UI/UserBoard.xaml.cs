using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Wpf.Ui.Controls;
using RAFFLE.Schema;
using Microsoft.Win32;
using RAFFLE.Utils;
using RAFFLE.Manager;
using System.Globalization;
using System.Windows.Threading;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserBoard : UiWindow
    {
        private BitmapImage img = null;
        private string sImpluse;
        DispatcherTimer timer = new DispatcherTimer();
        public UserBoard()
        {
            InitializeComponent();
            Initialze();
        }

        public void Update()
        {
            timer.Start();
            lblEndTime.Text = "End Time: " + getDateTimeFromString(SettingSchema.Time).ToString();
            lblPrice.Text = "Price: " + SettingSchema.Price;
            Img.Source = SettingSchema.Img;
            lblLocation.Text = "" + SettingSchema.Location;
            lblDescription.Text = "" + SettingSchema.Description;
            prgThread.IsIndeterminate = true;
            //lblWinner.Text = "Winner: None";
        }

        public void EndState()
        {
            timer.Stop();
            prgThread.IsIndeterminate = false;
            if (ResultSchema.WinnerPrice == 0)
            {
                prgThread.Progress = 100;
                //lblWinner.Text = "Winner: None";
                return;
            }
            prgThread.Progress = 100;
            //lblWinner.Text = "Winner: " + ResultSchema.WinnerNumber;            
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

        public void Initialze()
        {
            prgThread.IsIndeterminate = false;
            lblEndTime.Text = "End Time: " + getDateTimeFromString(SettingSchema.Time).ToString();
            lblPrice.Text = "Price: " + SettingSchema.Price;
            lblLocation.Text = "Location: " + SettingSchema.Location;
            lblDescription.Text = "Description: " + SettingSchema.Description;
            //lblWinner.Text = "Winner: None";
            lblWinnerPrice.Text = "Prize: 0";
            Img.Source = SettingSchema.Img;
            timer.Interval = TimeSpan.FromSeconds(1); // Set the interval to 1 second
            timer.Tick += Timer_Tick; // Set the event handler
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblCurTime.Text = "Current Time: " + DateTime.Now.ToString();
            lblWinnerPrice.Text = "Prize: " + ResultSchema.WinnerPrice;
            txtImpluse.Focus();
            if (sImpluse != "" && sImpluse != null && sImpluse.Length > 0)
            {
                txtImpluse.Text = sImpluse;
                sImpluse = sImpluse.Substring(1);
                //txtImpluse.Text = sImpluse;
                ThreadMgr.curProgress++;
                ResultSchema.WinnerPrice = ThreadMgr.curProgress * SettingSchema.Price * (1 - SettingSchema.Rate / 100);
                ThreadMgr.PrintText(ThreadMgr.curProgress + "\n" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), 14);
            }
        }

        private void txtImpluse_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Key.NumPad9)
            {
                txtImpluse.Text += "+";
                sImpluse = txtImpluse.Text;
            }
        }
    }
}
