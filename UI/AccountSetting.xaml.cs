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
using Wpf.Ui.Extensions;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AccountSetting : UiWindow
    {
        private BitmapImage img = null;
        public AccountSetting()
        {
            InitializeComponent();
        }


        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // validate input value
            // get user name
            string username = txtUsername.Text;
            int bUsername = DBMgr.validateUsername(username);
            if (bUsername < 0)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid Username(don't exist)");
                return;
            }

            string pin = DBMgr.ReadPIN(username);
            if (pin != txtCurPIN.Password) 
            {
                MsgHelper.ShowMessage(MsgType.Other, "Password Invalid");
                return;
            }

            if ((txtNewPIN.Password != txtNewPINConfirm.Password) || (txtNewPIN.Password == "" || txtNewPINConfirm.Password == ""))
            {
                MsgHelper.ShowMessage(MsgType.Other, "Password don't match");
                return;
            }

            string new_username = txtNewUsername.Text == "" ? username : txtNewUsername.Text;
            string new_pin = txtNewPIN.Password;

            DBMgr.UpdatePIN(username, new_pin, new_username);

            Builder.RaiseEvent(EventRaiseType.AccountSetting_Closed);
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.AccountSetting_Closed);
        }
    }
}
