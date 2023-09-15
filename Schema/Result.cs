using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RAFFLE.Schema
{
    public static class ResultSchema
    {
        private static int m_WinnerNumber;
        private static double m_WinnerPrice;
        private static double m_AdminPrice;
        private static BitmapImage m_Img;

        public static int WinnerNumber { get => m_WinnerNumber; set => m_WinnerNumber = value; }
        public static double WinnerPrice { get => m_WinnerPrice; set => m_WinnerPrice = value; }
        public static double AdminPrice { get => m_AdminPrice; set => m_AdminPrice = value; }
        public static BitmapImage Img { get => m_Img; set => m_Img = value; }
    }
}
