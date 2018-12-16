﻿using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StateEvaluation.BioColor.Helpers
{
    public class ColorConverter
    {
        public static Regex HEX = new Regex(@"^([0-9A-F]{2})([0-9A-F]{2})([0-9A-F]{2})$");
        public const float MAX = 0xFF;
        public const int MAX_HEX = 0xFF;

        /// <summary>
        /// Convert RGB string to CMYK array
        /// </summary>
        /// <param name="RGB">#[0-9A-F]{6}</param>
        /// <returns>Array of int[4] {C, M, Y, K}</returns>
        public static int[] RgbToCmyk(string RGB)
        {
            var match = HEX.Match(RGB.ToUpper());
            try
            {
                int R = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                int G = int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
                int B = int.Parse(match.Groups[3].Value, NumberStyles.HexNumber);
                return RgbToCmyk(R, G, B);
            }
            catch (System.FormatException)
            {
                return new int[] { 0, 0, 0, 0 };
            }
        }

        /// <summary>
        /// Convert RGB Color to CMYK array
        /// </summary>
        /// <param name="color">System.Drawing.Color</param>
        /// <returns>Array of int[4] {C, M, Y, K}</returns>
        public static int[] RgbToCmyk(Color color)
        {
            return RgbToCmyk(color.R, color.G, color.B);
        }

        /// <summary>
        /// Convert (R, G, B) to CMYK array
        /// </summary>
        /// <param name="R">Red (0-255)</param>
        /// <param name="G">Green (0-255)</param>
        /// <param name="B">Blue (0-255)</param>
        /// <returns>Array of int[4] {C, M, Y, K}</returns>
        public static int[] RgbToCmyk(int R, int G, int B)
        {
            if (R == 0 && G == 0 && B == 0)
            {
                return new int[] { 0, 0, 0, (int)MAX };
            }
            else
            {
                int K = MAX_HEX - System.Math.Max(R, System.Math.Max(G, B));
                int k = (int)(MAX * K / MAX_HEX);
                int c = (int)(MAX * (MAX_HEX - R - K) / (MAX_HEX - K));
                int m = (int)(MAX * (MAX_HEX - G - K) / (MAX_HEX - K));
                int y = (int)(MAX * (MAX_HEX - B - K) / (MAX_HEX - K));
                return new int[] { c, m, y, k };
            }
        }

        /// <summary>
        /// Convert (C, M, Y, K) to RGB array
        /// </summary>
        /// <param name="C">Cyan (0-255)/param>
        /// <param name="M">Magenta (0-255)</param>
        /// <param name="Y">Yellow (0-255)</param>
        /// <param name="K">Black (0-255)</param>
        /// <returns>Array of int[3] {R, G, B}</returns>
        public static int[] CmykToRgb(int C, int M, int Y, int K)
        {
            int R = (int)((1 - C / MAX) * (1 - K / MAX) * MAX_HEX);
            int G = (int)((1 - M / MAX) * (1 - K / MAX) * MAX_HEX);
            int B = (int)((1 - Y / MAX) * (1 - K / MAX) * MAX_HEX);
            return new int[] { R, G, B };
        }

        /// <summary>
        /// Natural Color mixing. Mix 3 colors into 1
        /// </summary>
        /// <param name="c1">Color</param>
        /// <param name="c2">Color</param>
        /// <param name="c3">Color</param>
        /// <returns>Color</returns>
        public static Color Mix(Color c1, Color c2, Color c3)
        {

            const int DIVIDER_3 = 3;
            const int DIVIDER_2 = 2;
            const int SPLACER = 128;

            int[] C1 = RgbToCmyk(c1);
            int[] C2 = RgbToCmyk(c2);
            int[] C3 = RgbToCmyk(c3);

            if (c1.A > SPLACER && c2.A > SPLACER && c3.A > SPLACER)
            {
                int[] RGB = CmykToRgb(
                    (C1[0] + C2[0] + C3[0]) / DIVIDER_3,
                    (C1[1] + C2[1] + C3[1]) / DIVIDER_3,
                    (C1[2] + C2[2] + C3[2]) / DIVIDER_3,
                    (C1[3] + C2[3] + C3[3]) / DIVIDER_3
                );
                return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
            }
            else if (c1.A > SPLACER && c2.A > SPLACER ||
                    c2.A > SPLACER && c3.A > SPLACER ||
                    c3.A > SPLACER && c1.A > SPLACER)
            {
                int[] RGB = CmykToRgb(
                    (C1[0] + C2[0] + C3[0]) / DIVIDER_2,
                    (C1[1] + C2[1] + C3[1]) / DIVIDER_2,
                    (C1[2] + C2[2] + C3[2]) / DIVIDER_2,
                    (C1[3] + C2[3] + C3[3]) / DIVIDER_2
                );
                return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
            }
            else
            {
                if (c1.A > SPLACER) return c1;
                else if (c2.A > SPLACER) return c2;
                else if (c3.A > SPLACER) return c3;
                else return Color.FromArgb(0, 0, 0, 0);
            }
        }
    }
}
