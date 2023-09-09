using System;
using System.Collections.Generic;
using System.Text;

namespace RicCommon.Infrastructure
{
    public static class RandomHexColor
    {
        static Random random = new Random();
        public static string Generate()
        {
            // Generate random values for Red, Green, and Blue components
            byte red = (byte)random.Next(256);    // 0-255
            byte green = (byte)random.Next(256);  // 0-255
            byte blue = (byte)random.Next(256);   // 0-255

            // Format the RGB values as a hexadecimal color string
            string hexColor = $"#{red:X2}{green:X2}{blue:X2}";

            return hexColor;
        }

    }
}
