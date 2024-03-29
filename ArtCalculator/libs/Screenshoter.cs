using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtCalculator.libs
{
    public static class Scrnsht
    {
        public static void CaptureAndDisplayScreenshot(Panel panel, int widthDivisor, int heightDivisor, int zoom)
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            int right = bounds.Width;
            int bottom = bounds.Height;
            int left = right - (bounds.Width / widthDivisor);
            int top = bottom - (bounds.Height / heightDivisor);

            Bitmap screenshot = new Bitmap(bounds.Width / zoom, bounds.Height / zoom);

            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(left, top, 0, 0, screenshot.Size);
            }

            panel.BackgroundImage = screenshot;
        }
    }
}
