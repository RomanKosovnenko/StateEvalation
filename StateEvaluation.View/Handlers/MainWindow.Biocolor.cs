﻿using StateEvaluation.BioColor.Helpers;
using StateEvaluation.BioColor.Providers;
using StateEvaluation.BusinessLayer.BuissnesManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for Biocolor tab
    /// </summary>
    partial class MainWindow : Window
    {
        private const int STEP = 7;
        public static Dictionary<string, string> userIdBirthPairs;
        private BiocolorSettings biocolorSettings;
        private BiocolorBusinessManager biocolorBusinessManager;
        private BiocolorProvider biocolorProvider;
        private ImageGenerator imageGenerator;
        private List<BiocolorProvider.ColorRow> colors;

        private void Prew_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.MakeStep(-STEP);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.MakeStep(+STEP);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.GenerateImages();
        }

        private void DrawGraphs_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.DrawGraphs();
        }

        private void WindowRendered_Click(object sender, EventArgs e)
        {
            userIdBirthPairs = peopleBuissnesManager.GetUserIdBirthPairs();
            RestoreColors_Click(sender, null);
        }

        private void CMYK2RGB_Click(object sender, RoutedEventArgs e)
        {
            var color = colors.Find(c => c.Converter == sender);
            biocolorBusinessManager.GenerateRgbFromCmyk(color);
        }

        private void RestoreColors_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.RestoreColors(colors);
            biocolorBusinessManager.SetCmykValues(colors);
        }

        private void ResetColors_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Do you want to RESET colors BiocolorSettings?", "Reset BiocolorSettings", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                biocolorBusinessManager.ResetColors(colors);
                biocolorBusinessManager.SetCmykValues(colors);
            }
            InitializeComponent();
        }

        private void SaveColors_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.SaveColors(colors);
            RestoreColors_Click(sender, e);
        }
    }
}
