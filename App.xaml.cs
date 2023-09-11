using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RAFFLE;
using RAFFLE.Manager;

namespace RAFFLE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DBMgr.LogInOutDB(true);
            Builder.RaiseEvent(EventRaiseType.Init);
            Builder.RaiseEvent(EventRaiseType.Login);
        }
    }
}
