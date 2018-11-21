using System;
using Xunit;
using StateEvaluation.BioColor;

namespace UnitTests
{
    public class ImageGeneratorTests
    {
        [Fact(DisplayName = "SE: Biocolor ImageGenerator RgbToCmyk")]
        public void RgbToCmyk()
        {
            //set var
            string RGB = "test color";
            int[] expectResult = new int[1] ;

            //action
            var result = ImageGenerator.RgbToCmyk(RGB);

            //checked result
            Assert.Equal(expectResult, result);

        }
    }
}
