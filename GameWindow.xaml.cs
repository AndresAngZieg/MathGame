using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace __WPFMathGame__
{
    public partial class GameWindow : Window
    {
        private readonly List<string> selectedOperations;
        private readonly Random random = new();
        private int score;
        private int correctAnswers;
        private int questionCount;
        private readonly DateTime startTime;
        private readonly DispatcherTimer timer;
        private readonly ScoreManager scoreManager;
        private const int TotalQuestions = 10;

        public GameWindow(List<string> operations)
        {
            InitializeComponent();
            selectedOperations = operations;
            startTime = DateTime.Now;
            scoreManager = new ScoreManager();

            UpdateScoreDisplay();

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) => timerDisplay.Text = $"Tiempo: {(DateTime.Now - startTime):mm\\:ss}";
            timer.Start();

            GenerateNewOperation();
        }

        private void GenerateNewOperation()
        {
            if (questionCount++ >= 10) { EndGame(); return; }

            int remaining = TotalQuestions - questionCount + 1;
            remainingDisplay.Text = $"Preguntas restantes: {remaining}";

            int num1 = random.Next(1, 111);
            int num2 = random.Next(1, 111);
            string operation = selectedOperations[random.Next(selectedOperations.Count)];

            Dictionary<string, string> symbols = new()
            {
                { "+", "+" }, { "-", "-" }, { "*", "×" }, { "/", "÷" },
                { "Raíces", "√" }, { "Potencias", "^" }
            };

            num2 = operation switch
            {
                "Raíces" => 0,
                "Potencias" => random.Next(1, 6),
                _ => num2
            };

            operationDisplay.Text = operation == "Raíces" ? $"√{num1} = ?" : $"{num1} {symbols[operation]} {num2} = ?";
            feedbackDisplay.Text = "";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string operationText = operationDisplay.Text;

            // Si la operación es una raíz cuadrada, el formato será diferente
            string operation = ""; // Para almacenar la operación (√, +, -, etc)
            int num1 = 0;
            int num2 = 0;

            // Detectar la operación en el texto
            if (operationText.Contains("√"))
            {
                operation = "√";
                string numberPart = operationText.Split('=')[0].Replace("√", "").Trim();
                num1 = int.Parse(numberPart);
            }
            else
            {
                // Para las operaciones convencionales (+, -, *, /, ^)
                string[] parts = operationText.Split(' ');

                operation = parts[1]; // Aquí obtenemos el operador: +, -, *, /, ^
                num1 = int.Parse(parts[0]); // Primer número
                num2 = int.Parse(parts[2]); // Segundo número
            }

            // Calcular la respuesta correcta según la operación
            int correctAnswer = operation switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "×" => num1 * num2,
                "÷" => num1 / num2,
                "√" => (int)Math.Floor(Math.Sqrt(num1)), // Raíz cuadrada, tomar el valor entero inferior
                "^" => (int)Math.Floor(Math.Pow(num1, num2)), // Potencia, tomar el valor entero inferior
                _ => 0
            };

            // Validación de la entrada para evitar el FormatException
            if (int.TryParse(answerInput.Text, out int userAnswer))
            {
                if (userAnswer == correctAnswer)
                {
                    correctAnswers++;
                    score++;
                    feedbackDisplay.Text = "¡Correcto!";
                }
                else
                {
                    score--;
                    feedbackDisplay.Text = $"Incorrecto. La respuesta era: {correctAnswer}";
                }
            }
            else
            {
                feedbackDisplay.Text = "Por favor ingresa un número válido.";
            }

            UpdateScoreDisplay();
            GenerateNewOperation();
            answerInput.Clear();
        }





        private void UpdateScoreDisplay()
        {
            scoreDisplay.Text = $"Puntos: {score}\nTop Scores:\n" + string.Join("\n", scoreManager.GetTopScores());
        }

        private void EndGame()
        {
            timer.Stop();
            double finalScore = correctAnswers / (DateTime.Now - startTime).TotalSeconds * 100000;

            scoreManager.SaveScore(finalScore);
            UpdateScoreDisplay();

            MessageBox.Show($"Juego terminado.\nPuntaje final: {finalScore:F2}");

            new MainWindow().Show();
            Close();
        }
    }
}
