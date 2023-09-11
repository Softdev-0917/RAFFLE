using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using RAFFLE.Utils;
using RAFFLE;

namespace RAFFLE.Utils
{
    public static class MsgHelper
    {
        public static bool ShowMessage(MsgType msgType, string msgTxt)
        {
            bool bRes = false;
            Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();

            switch (msgType)
            {
                case MsgType.AppExit:
                    messageBox.Title = "Warnig";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 18,
                        Text = "Are you sure exit?"
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Yes";
                    messageBox.ButtonLeftName = "No";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) => Builder.RaiseEvent(EventRaiseType.AppExit);
                    break;
                case MsgType.Other:
                    messageBox.Title = "Error";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        Text = msgTxt,
                        Width = 400,
                        Height = 200
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Exit";
                    messageBox.ButtonLeftName = "Retry";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) => Builder.RaiseEvent(EventRaiseType.AppExit);

                    break;
                case MsgType.DocSelRemove:
                    messageBox.Title = "Warnig";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 18,
                        Text = msgTxt
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Yes";
                    messageBox.ButtonLeftName = "No";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) => { messageBox.Hide(); bRes = false; };

                    break;
                case MsgType.PhotoRemove:
                    messageBox.Title = "Warnig";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 18,
                        Text = msgTxt
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Yes";
                    messageBox.ButtonLeftName = "No";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) => { messageBox.Hide(); bRes = false; };

                    break;
                default:
                    break;
            }

            messageBox.ShowDialog();

            return bRes;
        }


    }
}
