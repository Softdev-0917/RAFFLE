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

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : UiWindow
    {
        private BitmapImage img = null;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string pin = DBMgr.ReadPIN(username);
            if (txtPassword.Password != pin)
            {
                Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
                messageBox.Title = "Error";

                messageBox.Content = new TextBlock
                {
                    Margin = new Thickness(0, 8, 0, 0),
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 20,
                    Text = "Invalid PIN..."
                };
                messageBox.Height = 170;
                messageBox.Topmost = false;
                messageBox.ButtonRightName = "Exit";
                messageBox.ButtonLeftName = "Retry";
                messageBox.ShowTitle = true;
                messageBox.ButtonLeftClick += (_, _) => messageBox.Hide();
                messageBox.ButtonRightClick += (_, _) => Builder.RaiseEvent(EventRaiseType.AppExit);
                messageBox.ShowDialog();
            }
            else
            {
                Builder.RaiseEvent(EventRaiseType.LoginSuccess);
            }
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string username = txtUsername.Text;
                string pin = DBMgr.ReadPIN(username);
                if (txtPassword.Password != pin)
                {
                    Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
                    messageBox.Title = "Error";

                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 20,
                        Text = "Invalid PIN..."
                    };
                    messageBox.Height = 170;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Exit";
                    messageBox.ButtonLeftName = "Retry";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => messageBox.Hide();
                    messageBox.ButtonRightClick += (_, _) => Builder.RaiseEvent(EventRaiseType.AppExit);
                    messageBox.ShowDialog();
                }
                else
                {
                    Builder.RaiseEvent(EventRaiseType.LoginSuccess);
                }
            }
        }
    }
}
