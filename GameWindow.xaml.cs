using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace ___MathGame2___
{
    public partial class GameWindow : Window
    {
        private readonly List<string> selectedOperations;
        private readonly Random random = new();
        private int questionNumber = 0;
        private int correctAnswers = 0;
        private int incorrectAnswers = 0;
        private int totalQuestions = 10;

        private double currentStars = 0;
        private DispatcherTimer timer;
        private int elapsedSeconds = 0;

        private double currentResult;

        private ObservableCollection<GameResult> scores;

        public GameWindow(bool sum, bool sub, bool mul, bool div, bool pow, bool root, ObservableCollection<GameResult> scoresCollection)
        {
            InitializeComponent();

            scores = scoresCollection;

            selectedOperations = new List<string>();
            if (sum) selectedOperations.Add("+");
            if (sub) selectedOperations.Add("-");
            if (mul) selectedOperations.Add("*");
            if (div) selectedOperations.Add("/");
            if (pow) selectedOperations.Add("^");
            if (root) selectedOperations.Add("√");

            StartTimer();
            NextQuestion();
            ApplyBackground();
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                elapsedSeconds++;
                UpdateStars();
            };
            timer.Start();
        }

        private void NextQuestion()
        {
            if (questionNumber >= totalQuestions)
            {
                timer.Stop();
                MessageBox.Show($"Juego terminado!\nCorrectas: {correctAnswers}\nIncorrectas: {incorrectAnswers}\nEstrellas: {(int)currentStars}",
                                "Resultados", MessageBoxButton.OK, MessageBoxImage.Information);
                EndGame();
                return;
            }

            questionNumber++;
            txtQuestionNumber.Text = $"{questionNumber}/{totalQuestions}";
            txtCorrect.Text = correctAnswers.ToString();
            txtIncorrect.Text = incorrectAnswers.ToString();
            txtAnswer.Text = "";

            string op = selectedOperations[random.Next(selectedOperations.Count)];
            double a = random.Next(1, 21);
            double b = random.Next(1, 21);

            switch (op)
            {
                case "+": currentResult = a + b; txtOperation.Text = $"{a} + {b}"; break;
                case "-": currentResult = a - b; txtOperation.Text = $"{a} - {b}"; break;
                case "*": currentResult = a * b; txtOperation.Text = $"{a} × {b}"; break;
                case "/": b = random.Next(1, 21); currentResult = Math.Round(a / b, 2); txtOperation.Text = $"{a} ÷ {b}"; break;
                case "^": currentResult = Math.Pow(a, 2); txtOperation.Text = $"{a} ^ 2"; break;
                case "√": currentResult = Math.Round(Math.Sqrt(a), 2); txtOperation.Text = $"√{a}"; break;
            }

            UpdateStars();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAnswer.Text, out double userAnswer))
            {
                if (Math.Abs(userAnswer - currentResult) < 0.01)
                    correctAnswers++;
                else
                    incorrectAnswers++;

                NextQuestion();
            }
            else
            {
                MessageBox.Show("Introduce un número válido", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateStars()
        {
            double baseStars = correctAnswers * 2.5;
            double timePenalty = elapsedSeconds / 10.0;
            currentStars = Math.Max(0, Math.Min(25, baseStars - timePenalty));

            icStars.Items.Clear();
            for (int i = 0; i < (int)currentStars; i++)
                icStars.Items.Add("★");
        }

        private void EndGame()
        {
            int stars = (int)currentStars;
            int points = correctAnswers * 10; // ejemplo de puntuación

            var config = ConfigManager.LoadConfig();
            config.TotalStars += stars;  // sumamos las que ganó el jugador
            ConfigManager.SaveConfig(config);

            ResultManager.SaveResult(new GameResult
            {
                Player = "Jugador",
                Score = points,
                Stars = stars
            });

            this.Close();
        }

        private void ApplyBackground()
        {
            var config = ConfigManager.LoadConfig();

            try
            {
                this.Background = (Brush)new BrushConverter().ConvertFromString(config.CurrentBackground);
            }
            catch
            {
                // Color inválido o no definido, usar fondo por defecto
                this.Background = (Brush)new BrushConverter().ConvertFromString("DarkBlue");
            }
        }
    }
}
