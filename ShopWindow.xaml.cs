using System.Windows;
using System.Windows.Media;

namespace ___MathGame2___
{
    public partial class ShopWindow : Window
    {
        private GameConfig config;

        public ShopWindow()
        {
            InitializeComponent();
            config = ConfigManager.LoadConfig();
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Mostrar estrellas
            txtStarsShop.Text = $"⭐ Estrellas: {config.TotalStars}";

            // Actualizar estilo de botones según si ya están desbloqueados
            BtnBlue.Background = config.UnlockedBackgrounds.Contains("LightBlue") ? Brushes.LightBlue : Brushes.Gray;
            BtnGreen.Background = config.UnlockedBackgrounds.Contains("LightGreen") ? Brushes.LightGreen : Brushes.Gray;
            BtnRed.Background = config.UnlockedBackgrounds.Contains("LightCoral") ? Brushes.LightCoral : Brushes.Gray;
            BtnPurple.Background = config.UnlockedBackgrounds.Contains("Purple") ? Brushes.Purple : Brushes.Gray;
            BtnYellow.Background = config.UnlockedBackgrounds.Contains("Yellow") ? Brushes.Yellow : Brushes.Gray;

            // Opcional: marcar el fondo actual
            if (config.CurrentBackground == "LightBlue") BtnBlue.BorderBrush = Brushes.Gold;
            else BtnBlue.BorderBrush = Brushes.Black;

            if (config.CurrentBackground == "LightGreen") BtnGreen.BorderBrush = Brushes.Gold;
            else BtnGreen.BorderBrush = Brushes.Black;

            if (config.CurrentBackground == "LightCoral") BtnRed.BorderBrush = Brushes.Gold;
            else BtnRed.BorderBrush = Brushes.Black;

            if (config.CurrentBackground == "Purple") BtnPurple.BorderBrush = Brushes.Gold;
            else BtnPurple.BorderBrush = Brushes.Black;

            if (config.CurrentBackground == "Yellow") BtnYellow.BorderBrush = Brushes.Gold;
            else BtnYellow.BorderBrush = Brushes.Black;
        }

        private void BuyBackground(string colorName, int cost)
        {
            if (!config.UnlockedBackgrounds.Contains(colorName))
            {
                if (config.TotalStars >= cost)
                {
                    config.TotalStars -= cost;
                    config.UnlockedBackgrounds.Add(colorName);
                }
                else
                {
                    MessageBox.Show("No tienes suficientes estrellas!");
                    return;
                }
            }

            config.CurrentBackground = colorName;
            ConfigManager.SaveConfig(config);
            UpdateUI();
        }

        private void BtnBlue_Click(object sender, RoutedEventArgs e) => BuyBackground("LightBlue", 10);
        private void BtnGreen_Click(object sender, RoutedEventArgs e) => BuyBackground("LightGreen", 10);
        private void BtnRed_Click(object sender, RoutedEventArgs e) => BuyBackground("LightCoral", 10);
        private void BtnPurple_Click(object sender, RoutedEventArgs e) => BuyBackground("Purple", 15);
        private void BtnYellow_Click(object sender, RoutedEventArgs e) => BuyBackground("Yellow", 15);
    }
}
