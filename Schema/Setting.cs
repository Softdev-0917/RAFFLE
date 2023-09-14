using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RAFFLE.Schema
{
    public static class SettingSchema
    {
        private static string m_Time;
        private static int m_Count;
        private static double m_Rate;
        private static double m_Price;
        private static BitmapImage m_Img;
        private static string m_ImgPath;
        private static string m_Location;
        private static string m_Description;

        public static string Time { get => m_Time; set => m_Time = value; }
        public static int Count { get => m_Count; set => m_Count = value; }
        public static double Rate { get => m_Rate; set => m_Rate = value; }
        public static double Price { get => m_Price; set => m_Price = value; }
        public static BitmapImage Img { get => m_Img; set => m_Img = value; }
        public static string ImgPath { get => m_ImgPath; set => m_ImgPath = value; }
        public static string Location { get => m_Location; set => m_Location = value; }
        public static string Description { get => m_Description; set => m_Description = value; }
    }
}
