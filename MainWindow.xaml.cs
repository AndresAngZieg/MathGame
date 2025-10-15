using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace ___MathGame2___
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<GameResult> scores;
        public MainWindow()
        {
            InitializeComponent();
            scores = new ObservableCollection<GameResult>(ResultManager.LoadResults());
            dgScores.ItemsSource = scores;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            dgScores.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<GameResult>(ResultManager.LoadResults());

            var config = ConfigManager.LoadConfig();
            txtStars.Text = $"⭐ Estrellas: {config.TotalStars}";

            var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(config.CurrentBackground);
            this.Background = brush;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (lstOperations.SelectedItems.Count == 0)
            {
                MessageBox.Show("Debes seleccionar al menos una operación para jugar.",
                                "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool sum = lstOperations.SelectedItems.Cast<ListBoxItem>().Any(i => i.Content.ToString() == "Suma (+)");
            bool sub = lstOperations.SelectedItems.Cast<ListBoxItem>().Any(i => i.Content.ToString() == "Resta (-)");
            bool mul = lstOperations.SelectedItems.Cast<ListBoxItem>().Any(i => i.Content.ToString() == "Multiplicación (×)");
            bool div = lstOperations.SelectedItems.Cast<ListBoxItem>().Any(i => i.Content.ToString() == "División (÷)");
            bool pow = lstOperations.SelectedItems.Cast<ListBoxItem>().Any(i => i.Content.ToString() == "Potencias");
            bool root = lstOperations.SelectedItems.Cast<ListBoxItem>().Any(i => i.Content.ToString() == "Raíces");

            GameWindow game = new GameWindow(sum, sub, mul, div, pow, root, scores);
            game.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ShopWindow shop = new ShopWindow();
            shop.ShowDialog();

            var config = ConfigManager.LoadConfig();
            txtStars.Text = $"⭐ Estrellas: {config.TotalStars}";
            var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(config.CurrentBackground);
            this.Background = brush;
        }

    }
}