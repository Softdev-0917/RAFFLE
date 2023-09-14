﻿using RAFFLE.Schema;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RAFFLE.Manager
{
    public static class DBMgr
    {
        public static SQLiteConnection sqliteConn = new SQLiteConnection("Data Source=db_raffle.db;Version=3;New=True;Compress=True");
        /// <summary>
		/// true: login, false: logout
		/// </summary>
		/// <param name="bLogInOut"></param>
		/// <returns></returns>
		public static bool LogInOutDB(bool bLogInOut)
        {
            bool bResult = false;
            if (bLogInOut)
            {
                try
                {
                    sqliteConn.Open();
                    bResult = true;
                }
                catch (Exception ex)
                {
                    bResult = false;
                    MessageBox.Show(ex.Message);
                }
                return bResult;
            }
            else
            {
                try
                {
                    sqliteConn.Close();
                    bResult = true;
                }
                catch (Exception)
                {
                    bResult = false;
                }

                return bResult;
            }

        }

        public static string ReadPIN(string username)
        {
            string pin = "";
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_reader;
            sqlite_cmd = sqliteConn.CreateCommand();
            sqlite_cmd.CommandText = String.Format("select password from tbl_user where username = '{0}'", username);
            sqlite_reader = sqlite_cmd.ExecuteReader();
            while (sqlite_reader.Read())
            {
                pin = sqlite_reader.GetString(0);
            }
            return pin;
        }

        public static void InsertHistory()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = String.Format("Insert into tbl_history (time, count, rate, price, winner_number, winner_price, admin_price, remain, location, description, image) values ('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}', '{9}', @0);", SettingSchema.Time, SettingSchema.Count, SettingSchema.Rate, SettingSchema.Price, ResultSchema.WinnerNumber, ResultSchema.WinnerPrice, ResultSchema.AdminPrice, ResultSchema.Remain, SettingSchema.Location, SettingSchema.Description);
            cmd.CommandText = query;
            byte[] bytesImage;

            System.Drawing.Image img = new Bitmap(SettingSchema.ImgPath);
            bytesImage = ImageToByte(img, System.Drawing.Imaging.ImageFormat.Jpeg);
            SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
            param.Value = bytesImage;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
        }

        public static List<HistoryEntity> LoadHistoryData()
        {
            List<HistoryEntity> lstHistory = new List<HistoryEntity>();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "Select * FROM tbl_history";
            cmd.CommandText = query;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HistoryEntity historyEntity = new HistoryEntity();
                historyEntity.Time = reader.GetString(1);
                historyEntity.Count = reader.GetInt32(2);
                historyEntity.Rate = reader.GetFloat(3);
                historyEntity.Price = reader.GetFloat(4);
                historyEntity.WinnerNumber = reader.GetInt32(6);
                historyEntity.WinnerPrice = reader.GetFloat(7);
                historyEntity.AdminPrice = reader.GetFloat(8);
                historyEntity.Remain = reader.GetInt16(9);
                historyEntity.Location = reader.GetString(10);
                historyEntity.Description = reader.GetString(11);
                lstHistory.Add(historyEntity);
            }

            return lstHistory;
        }

        public static void ClearHistoryData()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "DELETE FROM tbl_history";
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }

        public static byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }
        //public Image Base64ToImage(string base64String)
        public static Image ByteToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }
    }
}
