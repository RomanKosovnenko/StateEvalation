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
    }
}
