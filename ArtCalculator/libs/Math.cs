using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ArtCalculator.libs
{
    public class Calc
    {
        public double CalculateDistance(Point p1, Point p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public double CalculateAzimuth(Point p1, Point p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;

            double radians = Math.Atan2(dy, dx);

            double degrees = radians * (180 / Math.PI);
            degrees = (degrees + 360) % 360;

            degrees += 90;
            degrees %= 360;

            return degrees;
        }

        public double CalculateDistanceInGame(double gameGridSize, double gridDist, double targetDistance)
        {
            return (gameGridSize / gridDist) * targetDistance;
        }

    }
}
