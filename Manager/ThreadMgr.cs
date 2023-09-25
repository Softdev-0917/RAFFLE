using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using RAFFLE.UI;

namespace RAFFLE.Manager
{
    public static class ThreadMgr
    {
        public static int curProgress = 0;
        public static int timerSpc = 5;
        public static void PrintText(string text, int fontsize)
        {
            // Create a new PrintDocument object
            PrintDocument document = new PrintDocument();

            document.DocumentName = "Printing Test";
            document.DefaultPageSettings.PaperSize = new PaperSize("Custom", cmToPixels(10f), cmToPixels(10.5f));
            document.DefaultPageSettings.Margins = new Margins(cmToPixels(3f), 0, 0, 0);

            document.PrintPage += (sender, e) =>
            {
                // Get the size of the text
                SizeF textSize = e.Graphics.MeasureString(text, new Font("Arial", fontsize));

                // Calculate the position where the text should be drawn, centered horizontally and vertically
                float x = e.MarginBounds.Left + (e.MarginBounds.Width - textSize.Width) / 2;
                float y = e.MarginBounds.Top + (e.MarginBounds.Height - textSize.Height) / 2;

                // Draw the text on the graphics surface
                e.Graphics.DrawString(text, new Font("Arial", fontsize), System.Drawing.Brushes.Black, x, y);
            };

            // Start printing the document to the default printer
            document.Print();
            document.EndPrint += (sender, e) =>
            {
            };

        }

        // Helper function to convert centimeters to pixels
        private static int cmToPixels(float cm)
        {
            const float inchToCm = 2.54f;
            const int dpi = 96; // Assuming 96 DPI

            return (int)(cm * dpi / inchToCm);
        }
    }
    
}
