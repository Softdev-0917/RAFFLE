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
using RAFFLE.Manager;
using RAFFLE.Utils;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Result : UiWindow
    {
        private BitmapImage img = null;
        public Result()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            //lblAdminPrice.Text = "Admin Price: " + Math.Round(ResultSchema.AdminPrice, 2).ToString();
            lblWinnerNumber.Text = "Winner: " + ResultSchema.WinnerNumber.ToString();
            lblWinnerPrice.Text = "Price: " + Math.Round(ResultSchema.WinnerPrice, 2).ToString();
            ResImg.Source = ResultSchema.Img;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.AppExit);
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.MainWindow);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DBMgr.InsertHistory();
            Builder.RaiseEvent(EventRaiseType.Result2History);
        }
    }
}
