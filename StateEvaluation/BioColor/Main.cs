﻿using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StateEvaluation.BioColor
{
    internal static class Main
    {
        // BioColor 
        private static Grid _myGrid;
        private static DatePicker _birthDate, _nowDate;

        private const int RangeRed = 23, RangeGreen = 28, RangeBlue = 33;
        private const int HalfHexFf = 128, Height = 480, Zero = 0;
        private static readonly int Mid = Settings.Default.mid,
               Square = Settings.Default.square;
        private static readonly double Alpha = Settings.Default.alpha;

        private static readonly int IntRed = Settings.Default.Int_Red,
            IntGreen = Settings.Default.Int_Green,
            IntBlue = Settings.Default.Int_Blue;

        private static string _pathRed, _pathGreen, _pathBlue;
        private static string _initDate;

        public static void InitBioColor(Grid myGrid, DatePicker birthDate, DatePicker nowDate)
        {
            _myGrid = myGrid;
            _birthDate = birthDate;
            _nowDate = nowDate;
            Restore();
        }

        private static void Save()
        {
            Settings.Default.Path_Red = _pathRed;
            Settings.Default.Path_Green = _pathGreen;
            Settings.Default.Path_Blue = _pathBlue;
            Settings.Default.Init_date = _initDate;
            Settings.Default.Save();
        }

        private static void Restore()
        {
            _pathRed = Settings.Default.Path_Red;
            _pathGreen = Settings.Default.Path_Green;
            _pathBlue = Settings.Default.Path_Blue;
            _initDate = Settings.Default.Init_date;
        }
        public static void MakeStep(int step)
        {
            _nowDate.Text = Convert.ToDateTime(_nowDate.Text).AddDays(step).ToString(CultureInfo.CurrentCulture);
            DrawPicture();
        }
        public static void DrawGraphs()
        {
            DrawPicture();
        }

        private static void DrawPicture()
        {
            Save();
            try
            {
                DateTime dt = Convert.ToDateTime(_birthDate.Text);
                DateTime nw = Convert.ToDateTime(_nowDate.Text);
                int delta = (int)((nw.Subtract(dt)).TotalDays);

                int width = 1000 + Mid;
                Function.DrawClear(_myGrid);
                Function.GetCanvasImage(_pathRed, IntRed, delta, _myGrid);
                Function.GetCanvasImage(_pathGreen, IntGreen, delta, _myGrid);
                Function.GetCanvasImage(_pathBlue, IntBlue, delta, _myGrid);

                Bitmap r = new Bitmap(_pathRed);
                Bitmap g = new Bitmap(_pathGreen);
                Bitmap b = new Bitmap(_pathBlue);
                Bitmap w = new Bitmap(width, Height);

                for (int x = Zero; x < width; ++x)
                {
                    for (int y = Height / 2 - 1; Zero < y; --y)
                    {
                        int xPixel = (x + delta * Square - Mid);
                        System.Drawing.Color rColor = r.GetPixel((xPixel + r.Width) % r.Width, y);
                        System.Drawing.Color gColor = g.GetPixel((xPixel + g.Width) % g.Width, y);
                        System.Drawing.Color bColor = b.GetPixel((xPixel + b.Width) % b.Width, y);
                        if (
                            rColor.A < HalfHexFf && gColor.A < HalfHexFf ||
                            rColor.A < HalfHexFf && bColor.A < HalfHexFf ||
                            gColor.A < HalfHexFf && bColor.A < HalfHexFf
                            ) break;
                        w.SetPixel(x, y, Function.ColorMix(rColor, gColor, bColor));

                    }
                    for (int y = Height / 2 ; y < Height; ++y)
                    {
                        int xPixel = (x + delta * Square - Mid);
                        System.Drawing.Color rColor = r.GetPixel((xPixel + r.Width) % r.Width, y);
                        System.Drawing.Color gColor = g.GetPixel((xPixel + g.Width) % g.Width, y);
                        System.Drawing.Color bColor = b.GetPixel((xPixel + b.Width) % b.Width, y);
                        if (
                            rColor.A < HalfHexFf && gColor.A < HalfHexFf ||
                            rColor.A < HalfHexFf && bColor.A < HalfHexFf ||
                            gColor.A < HalfHexFf && bColor.A < HalfHexFf
                            ) break;
                        w.SetPixel(x, y, Function.ColorMix(rColor, gColor, bColor));

                    }
                }
                MemoryStream memoryStream = new MemoryStream();
                w.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage theImage = new BitmapImage();
                {
                    theImage.BeginInit();
                    theImage.CacheOption = BitmapCacheOption.OnLoad;
                    theImage.StreamSource = memoryStream;
                    theImage.EndInit();
                }

                ImageBrush myImageBrush = new ImageBrush(theImage);
                Canvas myCanvas = new Canvas
                {
                    Width = theImage.Width,
                    Height = theImage.Height,
                    Background = myImageBrush,
                    Opacity = Alpha,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                _myGrid.Children.Add(myCanvas);
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect Date!");
            }

        }
        public static void Menu()
        {
            SettingsEdit s = new SettingsEdit();
            s.ShowDialog();
            s.Get();
            Restore();
        }
        public static void Generate()
        {
            ImageGenerator.Generate(RangeRed);
            ImageGenerator.Generate(RangeGreen);
            ImageGenerator.Generate(RangeBlue);
            MessageBox.Show("Generated!");
            // Application.Current.Shutdown(); 
            // myGrid.Children.Add();
        }
    }
}
