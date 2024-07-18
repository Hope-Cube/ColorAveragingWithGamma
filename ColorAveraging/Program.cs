using System;
using System.Collections.Generic;
using System.Drawing;

namespace ColorAveraging
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Color> colors = new List<Color>() {
            Color.FromArgb(233, 45, 255),
            Color.FromArgb(56, 206, 252)
            };
            //Default gamma is 2
            Color color = AverageColor(colors, 2);
            //color = AverageColor(colors);

            Bitmap bmp = new Bitmap(1, 3);
            bmp.SetPixel(0, 0, colors[0]);
            bmp.SetPixel(0, 1, color);
            bmp.SetPixel(0, 2, colors[1]);
            bmp.Save("test.png");
        }
        static Color AverageColor(List<Color> colors, double gamma)
        {
            // Normalize and linearize color components
            List<(double, double, double)> linearValues = new List<(double, double, double)>();
            foreach (var color in colors)
            {
                double r = Math.Pow(color.R / 255.0, gamma);
                double g = Math.Pow(color.G / 255.0, gamma);
                double b = Math.Pow(color.B / 255.0, gamma);
                linearValues.Add((r, g, b));
            }

            // Compute average of linear color components
            double ravg = 0;
            double gavg = 0;
            double bavg = 0;
            foreach (var linear in linearValues)
            {
                ravg += linear.Item1;
                gavg += linear.Item2;
                bavg += linear.Item3;
            }
            ravg /= colors.Count;
            gavg /= colors.Count;
            bavg /= colors.Count;

            // Convert averaged linear values back to gamma-corrected values
            int ravgGamma = (int)(Math.Pow(ravg, 1 / gamma) * 255.0);
            int gavgGamma = (int)(Math.Pow(gavg, 1 / gamma) * 255.0);
            int bavgGamma = (int)(Math.Pow(bavg, 1 / gamma) * 255.0);

            return Color.FromArgb(ravgGamma, gavgGamma, bavgGamma);
        }
    }
}
