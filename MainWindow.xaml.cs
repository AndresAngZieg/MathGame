using System;
using System.Collections.Generic;
using System.Windows;

namespace __WPFMathGame__
{
    public partial class MainWindow : Window
    {
        private readonly ScoreManager scoreManager = new();

        public MainWindow()
        {
            InitializeComponent();
            scoreList.ItemsSource = scoreManager.GetTopScores();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            var selectedOperations = new List<string>();

            if (checkAdd.IsChecked == true) selectedOperations.Add("+");
            if (checkSubtract.IsChecked == true) selectedOperations.Add("-");
            if (checkMultiply.IsChecked == true) selectedOperations.Add("*");
            if (checkDivide.IsChecked == true) selectedOperations.Add("/");
            if (checkSqrt.IsChecked == true) selectedOperations.Add("Raíces");
            if (checkPower.IsChecked == true) selectedOperations.Add("Potencias");

            var gameWindow = new GameWindow(selectedOperations);
            gameWindow.Show();
            Close();
        }
    }
}
