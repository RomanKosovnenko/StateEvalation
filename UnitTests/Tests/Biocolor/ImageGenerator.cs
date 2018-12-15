using System;
using Xunit;
using StateEvaluation.BioColor;
using System.Text.RegularExpressions;

namespace UnitTests
{
    public class ImageGeneratorTests
    {
        [Fact(DisplayName = "SE: Biocolor ImageGenerator RgbToCmyk")]
        public void RgbToCmyk()
        {
            //set var
            string RGB = "FF0000";
            int[] expectResult = new int[4] { 0, 255, 255, 0 };
            
            //action
            var result = ImageGenerator.RgbToCmyk(RGB, new Regex(@"^([0-9A-F]{2})([0-9A-F]{2})([0-9A-F]{2})$"));

            //checked result
            Assert.Equal(4, result.Length);
            Assert.Equal(expectResult[0], result[0]);
            Assert.Equal(expectResult[1], result[1]);
            Assert.Equal(expectResult[2], result[2]);
            Assert.Equal(expectResult[3], result[3]);

        }
    }
}
