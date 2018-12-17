using Xunit;
using StateEvaluation.BioColor.Helpers;
using Color = System.Drawing.Color;

namespace UnitTests
{
    public class ColorConverterTests
    {
        #region RgbToCmyk
        [Fact(DisplayName = "SE: Biocolor ColorConverter RgbToCmyk")]
        public void RgbToCmyk()
        {
            //set vars
            string RGB = "FF0000";
            byte[] expectedResult = new byte[4] { 0, 255, 255, 0 };

            //action
            var result = ColorConverter.RgbToCmyk(RGB);

            //check result
            Assert.Equal(4, result.Length);
            Assert.Equal(expectedResult[0], result[0]);
            Assert.Equal(expectedResult[1], result[1]);
            Assert.Equal(expectedResult[2], result[2]);
            Assert.Equal(expectedResult[3], result[3]);
        }

        [Fact(DisplayName = "SE: Biocolor ColorConverter RgbToCmyk (black)")]
        public void RgbToCmykBlack()
        {
            //set vars
            byte R = 0, G = 0, B = 0;
            byte[] expectedResult = new byte[4] { 0, 0, 0, 255 };

            //action
            var result = ColorConverter.RgbToCmyk(R, G, B);

            //check result
            Assert.Equal(4, result.Length);
            Assert.Equal(expectedResult[0], result[0]);
            Assert.Equal(expectedResult[1], result[1]);
            Assert.Equal(expectedResult[2], result[2]);
            Assert.Equal(expectedResult[3], result[3]);
        }

        [Fact(DisplayName = "SE: Biocolor ColorConverter RgbToCmyk (error)")]
        public void RgbToCmykError()
        {
            //set vars
            string RGB = "RandomString";
            byte[] expectedResult = new byte[4] { 0, 0, 0, 0 };

            //action
            var result = ColorConverter.RgbToCmyk(RGB);

            //check result
            Assert.Equal(4, result.Length);
            Assert.Equal(expectedResult[0], result[0]);
            Assert.Equal(expectedResult[1], result[1]);
            Assert.Equal(expectedResult[2], result[2]);
            Assert.Equal(expectedResult[3], result[3]);
        }
        #endregion

        #region CmykToRgb
        [Fact(DisplayName = "SE: Biocolor ColorConverter CmykToRgb")]
        public void CmykToRgb()
        {
            //set vars
            byte C = 75, M = 0, Y = 168, K = 58;
            byte[] expectedResult = new byte[3] { 139, 197, 67 };

            //action
            var result = ColorConverter.CmykToRgb(C, M, Y, K);

            //check result
            Assert.Equal(3, result.Length);
            Assert.Equal(expectedResult[0], result[0]);
            Assert.Equal(expectedResult[1], result[1]);
            Assert.Equal(expectedResult[2], result[2]);
        }
        #endregion

        #region Mix
        [Fact(DisplayName = "SE: Biocolor ColorConverter Mix 3 colors")]
        public void Mix3()
        {
            //set vars
            Color Color1 = Color.FromArgb(0xFF, 0x78, 0xA4, 0xF6);
            Color Color2 = Color.FromArgb(0xFF, 0x90, 0x4E, 0x9A);
            Color Color3 = Color.FromArgb(0xFF, 0x1D, 0xE8, 0x3A);
            Color eColor = Color.FromArgb(0xFF, 0x6D, 0x99, 0x9E);

            //action
            var result = ColorConverter.Mix(Color1, Color2, Color3);

            //check result
            Assert.Equal(eColor, result);
        }

        [Fact(DisplayName = "SE: Biocolor ColorConverter Mix 2 colors")]
        public void Mix2()
        {
            //set vars
            Color Color1 = Color.FromArgb(0xFF, 0x78, 0xA4, 0xF6);
            Color Color2 = Color.FromArgb(0xFF, 0x90, 0x4E, 0x9A);
            Color Color3 = Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF);
            Color eColor = Color.FromArgb(0xFF, 0x8E, 0x75, 0xC8);

            //action
            var result = ColorConverter.Mix(Color1, Color2, Color3);

            //check result
            Assert.Equal(eColor, result);
        }

        [Fact(DisplayName = "SE: Biocolor ColorConverter Mix 1 color")]
        public void Mix1()
        {
            //set vars
            Color Color1 = Color.FromArgb(0xFF, 0x78, 0xA4, 0xF6);
            Color Color2 = Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF);
            Color Color3 = Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF);
            Color eColor = Color.FromArgb(0xFF, 0x78, 0xA4, 0xF6);

            //action
            var result = ColorConverter.Mix(Color1, Color2, Color3);

            //check result
            Assert.Equal(eColor, result);
        }
        #endregion
    }
}
