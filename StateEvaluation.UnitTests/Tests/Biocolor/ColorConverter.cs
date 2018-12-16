using System;
using Xunit;
using StateEvaluation;
using StateEvaluation.BioColor.Helpers;
using Color = System.Drawing.Color;

namespace UnitTests
{
    public class ColorConverterTests
    {
        [Fact(DisplayName = "SE: Biocolor ColorConverter RgbToCmyk")]
        public void RgbToCmyk()
        {
            //set vars
            string RGB = "FF0000";
            int[] expectedResult = new int[4] { 0, 255, 255, 0 };

            //action
            var result = ColorConverter.RgbToCmyk(RGB);

            //check result
            Assert.Equal(4, result.Length);
            Assert.Equal(expectedResult[0], result[0]);
            Assert.Equal(expectedResult[1], result[1]);
            Assert.Equal(expectedResult[2], result[2]);
            Assert.Equal(expectedResult[3], result[3]);
        }

        [Fact(DisplayName = "SE: Biocolor ColorConverter CmykToRgb")]
        public void CmykToRgb()
        {
            //set vars
            int C = 75, M = 0, Y = 168, K = 58;
            int[] expectedResult = new int[3] { 139, 197, 67 };

            //action
            var result = ColorConverter.CmykToRgb(C, M, Y, K);

            //check result
            Assert.Equal(3, result.Length);
            Assert.Equal(expectedResult[0], result[0]);
            Assert.Equal(expectedResult[1], result[1]);
            Assert.Equal(expectedResult[2], result[2]);
        }

        [Fact(DisplayName = "SE: Biocolor ColorConverter Mix")]
        public void Mix()
        {
            //set vars
            Color Color1 = Color.FromArgb(255, 0, 0);
            Color Color2 = Color.FromArgb(0, 255, 0);
            Color Color3 = Color.FromArgb(0, 0, 255);
            Color expectedResult = Color.FromArgb(84, 84, 84);

            //action
            var result = ColorConverter.Mix(Color1, Color2, Color3);

            //check result
            Assert.Equal(result, expectedResult);
        }
    }
}
