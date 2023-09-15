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
using System.IO;
using System.Reflection;
using System.Globalization;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Setting : UiWindow
    {
        private BitmapImage img = null;
        private string imgPath = null;
        public Setting()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            txtTime.SelectedDate = DateTime.Now;
            txtTimePicker.Text = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            txtCount.Text = "1";
            txtPrice.Text = "1";
            txtRate.Text = "25";

            string executablePath = Assembly.GetExecutingAssembly().Location;
            string curDir = System.IO.Path.GetDirectoryName(executablePath);

            var uri = new Uri(curDir + "\\Invalid.png");
            img = new BitmapImage(uri);
            SetImg.Source = img;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SettingSchema.Time = txtTime.SelectedDate.Value.Date.ToShortDateString() + txtTimePicker.Text;
            SettingSchema.Rate = Convert.ToDouble(txtRate.Text);
            SettingSchema.Count = Convert.ToInt16(txtCount.Text);
            SettingSchema.Price = Convert.ToInt16(txtPrice.Text);
            SettingSchema.Location = txtLocation.Text;
            SettingSchema.Description = txtDescription.Text;

            if (getDateTimeFromString(SettingSchema.Time) <= DateTime.Now)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid time");
                return;
            }
            if (imgPath == null)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid image");
                return;
            }
            if (SettingSchema.Count <= 0)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid count");
                return;
            }
            if (SettingSchema.Price <= 0 || SettingSchema.Price > 5)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid price(1-5)");
                return;
            }
            if (SettingSchema.Rate <= 0 || SettingSchema.Rate > 25)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid rate(1-25)");
                return;
            }
            if (SettingSchema.Location == null || SettingSchema.Location == "")
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid location");
                return;
            }
            if (SettingSchema.Description == null || SettingSchema.Description == "")
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid description");
                return;
            }

            SettingSchema.Img = img;
            SettingSchema.ImgPath = imgPath;
            // close setting dialog
            Builder.RaiseEvent(EventRaiseType.MainWindow);
            Builder.uiMainWindow.UpdateState();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.MainWindow);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MsgHelper.ShowMessage(MsgType.AppExit, "");
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *JPEG; *.GIF; *PNG)| *.BMP; *.JPG; *.JPEG; *.GIF; *.PNG | All files(*.*) | *.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string fileName = openFileDialog.FileName;
                var uri = new Uri(fileName);
                img = new BitmapImage(uri);
                imgPath = fileName;
                SetImg.Source = img;
            }
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.MainWindow);
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // Update the selected time value when the OK button is clicked
            TimeSpan selectedTime = new TimeSpan((int)hourSlider.Value, (int)minuteSlider.Value, 0);
            txtTimePicker.Text = selectedTime.ToString(@"hh\:mm");
            timePickerPopup.IsOpen = false;
        }

        private void hourSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblHour.Text = Convert.ToInt16(hourSlider.Value) + "H:";
        }

        private void minuteSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblMinute.Text = Convert.ToInt16(minuteSlider.Value) + "M:";
        }

        private void btnCountUp_Click(object sender, RoutedEventArgs e)
        {
            // Increment the value when the up button is clicked
            int currentValue = int.Parse(txtCount.Text);
            txtCount.Text = (currentValue + 1).ToString();
        }

        private void btnCountDown_Click(object sender, RoutedEventArgs e)
        {
            // Decrement the value when the down button is clicked
            int currentValue = int.Parse(txtCount.Text);
            txtCount.Text = (currentValue - 1).ToString();
        }


        private void txtTimePicker_MouseEnter(object sender, MouseEventArgs e)
        {
            // Open the popup window when the text box is clicked
            timePickerPopup.IsOpen = true;
            timePickerPopup.StaysOpen = true;

            // Set the initial slider values to the current time
            DateTime currentTime = DateTime.Now;
            if (txtTimePicker.Text != "")
            {
                hourSlider.Value = Convert.ToInt16(txtTimePicker.Text.Split(':')[0]);
                minuteSlider.Value = Convert.ToInt16(txtTimePicker.Text.Split(':')[1]);
            }
            else
            {
                hourSlider.Value = currentTime.Hour;
                minuteSlider.Value = currentTime.Minute;
            }
        }

        private void txtTimePicker_MouseLeave(object sender, MouseEventArgs e)
        {
        }


        private void btnTimeCancel_Click(object sender, RoutedEventArgs e)
        {
            timePickerPopup.IsOpen = false;
        }
    }
}
