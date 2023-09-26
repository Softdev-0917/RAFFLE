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
    public partial class UserBoard : UiWindow
    {
        private BitmapImage img = null;
        private string sImpluse;
        DispatcherTimer timer_raffle = new DispatcherTimer();
        DispatcherTimer timer_clock = new DispatcherTimer();
        private int ClockCount;
        public UserBoard()
        {
            InitializeComponent();
            Initialze();
        }
        public void Update()
        {
            timer_raffle.Start();
            lblEndTime.Text = "End Time: " + getDateTimeFromString(SettingSchema.Time).ToString();
            lblPrice.Text = "Price: " + SettingSchema.Price;
            Img.Source = SettingSchema.Img;
            lblLocation.Text = "" + SettingSchema.Location;
            lblDescription.Text = "" + SettingSchema.Description;
            prgThread.IsIndeterminate = true;
        }

        public void EndState()
        {
            timer_raffle.Stop();
            prgThread.IsIndeterminate = false;
            if (ResultSchema.WinnerPrice == 0)
            {
                prgThread.Progress = 100;
                return;
            }
            prgThread.Progress = 100;   
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
            }
            else
            {
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
            lblWinnerPrice.Text = "Winner Prize: 0";
            Img.Source = SettingSchema.Img;
            timer_raffle.Interval = TimeSpan.FromSeconds(ThreadMgr.timerSpc); // Set the interval to 1 second
            timer_raffle.Tick += Timer_Tick; // Set the event handler
            ClockCount = 3;
            lblTitle.Text = ClockCount.ToString();

            timer_clock.Interval = TimeSpan.FromSeconds(1);
            timer_clock.Tick += TimerClock_Tick;
            timer_clock.Start();
        }
        private void TimerClock_Tick(object sender, EventArgs e)
        {
            lblCurTime.Text = "Current Time: " + DateTime.Now.ToString();
            if (ClockCount > 0)
            {
                ClockCount--;
                lblTitle.Text = ClockCount.ToString();
            } else if (ClockCount == 0)
            {
                ClockCount--;
                lblTitle.Text = "Started!";
            } else
            {
                lblTitle.Text = "";
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            txtImpluse.Focus();
            if (sImpluse != "" && sImpluse != null && sImpluse.Length > 0)
            {
                ResultSchema.WinnerPrice = ThreadMgr.curProgress * SettingSchema.Price * (1 - SettingSchema.Rate / 100);
                ThreadMgr.PrintText("Ticket " + ThreadMgr.curProgress + "\n" + SettingSchema.Time + "\n" + SettingSchema.Location + "\n" + SettingSchema.Description, 14);
                txtImpluse.Text = sImpluse;
                sImpluse = sImpluse.Substring(1);
                txtImpluse.Text = sImpluse;
                ThreadMgr.curProgress++;
                lblWinnerPrice.Text = "Winner Prize: " + ResultSchema.WinnerPrice;
            }
        }

        private void txtImpluse_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
            char typedChar = e.Text[0];
            if (typedChar == '9')
            {
                txtImpluse.Text += "+";
                sImpluse = txtImpluse.Text;
            }
        }

        //private void txtImpluse_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.Handled = true;

        //    //int ascii = KeyInterop.VirtualKeyFromKey(e.Key);
        //    if (e.Key.ToString() == "9")
        //    {
        //        txtImpluse.Text += "+";
        //        sImpluse = txtImpluse.Text;
        //    }
        //}
    }
}
