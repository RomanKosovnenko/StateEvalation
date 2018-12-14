using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace StateEvaluationDLL.DataStructures
{
    public static class ColorOperations
    {
        public static Color Sum(Color color1, Color color2)
        {
            Color rezultColor = Color.FromArgb((Byte)(color1.A + color2.A),
            (Byte)(color1.R + color2.R),
            (Byte)(color1.G + color2.G),
            (Byte)(color1.B + color2.B));

            return rezultColor;
        }

        public static int NextColorNumber(int currentColorNumber, int maxColorCount)
        {
            ++currentColorNumber;
            if (currentColorNumber == (maxColorCount + 1)) { currentColorNumber = 1; }
            return currentColorNumber;
        }

        public static int Plus(int color, int number, int maxColorCount)
        {
            color += number;
            if (maxColorCount < color) { color -= maxColorCount; }
            return color;
        }

        public static int Minus(int color, int number, int maxColorCount)
        {
            color -= number;
            if (color < 0) { color += maxColorCount; }
            return color;
        }
    }
}
