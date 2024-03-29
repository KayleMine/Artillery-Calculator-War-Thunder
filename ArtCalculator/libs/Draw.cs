using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ArtCalculator.libs
{
    public class Drawin
    {
        public void WriteGridLine(Graphics grph, Point first, Point second, String text)
        {
            using (Pen pen = new Pen(Color.Green, 2))
            {
                grph.DrawLine(pen, first, second);
            }
            if (!string.IsNullOrEmpty(text)) { 
            using (Font font = new Font("Arial", 10))
            using (SolidBrush brush = new SolidBrush(Color.Green))
            {
                grph.DrawString(text, font, brush, first.X, second.Y);
            }
            }
        }


        public void WriteDistanceLine(Graphics grph, Point first, Point second, params string[] texts)
        {
            Color lineColor = GetRandomColor();

            using (Pen pen = new Pen(lineColor, 2))
            {
                grph.DrawLine(pen, first, second);
            }

            // Concatenate all strings passed in the texts array
            string LineText = string.Join("\n", texts);

            Font font = new Font("Arial", 9);
            Color newColor = Color.FromArgb(95, Color.Black);
            SolidBrush blackBrush = new SolidBrush(newColor);
            SolidBrush textBrush = new SolidBrush(IncreaseBrightness(lineColor, 155));

            SizeF textSize = grph.MeasureString(LineText, font);

            // Calculate the midpoint of the line
            PointF midpoint = new PointF((first.X + second.X) / 2, (first.Y + second.Y) / 2);

            // Calculate the angle of the line
            float angle = (float)Math.Atan2(second.Y - first.Y, second.X - first.X);

            // Draw the text at the calculated position
            grph.TranslateTransform(midpoint.X, midpoint.Y);
            grph.RotateTransform(angle * 180 / (float)Math.PI);

            // Ensure the text always reads from left to right
            if (angle > Math.PI / 2 || angle < -Math.PI / 2)
            {
                grph.RotateTransform(180);
            }

            // Draw background box
            RectangleF backgroundRect = new RectangleF(-textSize.Width / 2, -textSize.Height / 2, textSize.Width, textSize.Height);
            grph.FillRectangle(blackBrush, backgroundRect);

            // Draw outline
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        grph.DrawString(LineText, font, blackBrush, i - textSize.Width / 2, j - textSize.Height / 2); // Adjusted position
                    }
                }
            }

            // Draw main text
            grph.DrawString(LineText, font, textBrush, -textSize.Width / 2, -textSize.Height / 2);
            grph.ResetTransform();

            font.Dispose();
            blackBrush.Dispose();
            textBrush.Dispose();
        }


        public Color GetRandomColor()
        {
            Random random = new Random();
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            Color randomColor = Color.FromArgb(red, green, blue);
            return randomColor;
        }
        public Color IncreaseBrightness(Color color, int increment)
        {
            int red = Math.Min(color.R + increment, 255);
            int green = Math.Min(color.G + increment, 255);
            int blue = Math.Min(color.B + increment, 255);
            return Color.FromArgb(red, green, blue);
        }
    }
}
