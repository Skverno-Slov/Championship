using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using WEMMApi.Contexts;
using WEMMApi.Dtos;
using WEMMApi.Services;
using WEMMWpf;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;

namespace WEMM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void BurgerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeBurgerMenuVisibility(sender);
        }

        private void AdministrateButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdministrateStackPanel.Visibility == Visibility.Collapsed)
            {
                AdministrateStackPanel.Visibility = Visibility.Visible;
                AdministrateArrowTextBlock.Text = "△";
                return;
            }
            AdministrateStackPanel.Visibility = Visibility.Collapsed;
            AdministrateArrowTextBlock.Text = "▽";
        }

        private void ChangeBurgerMenuVisibility(object sender)
        {
            var control = sender as Control;
            if (control.Name == "BurgerMenuButtonHidden")
            {
                BurgerMenuButtonHidden.Visibility = Visibility.Collapsed;
                BurgerMenuDockPanel.Visibility = Visibility.Visible;
                return;
            }
            BurgerMenuButtonHidden.Visibility = Visibility.Visible;
            BurgerMenuDockPanel.Visibility = Visibility.Collapsed;
        }

        private void ProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfileComboBox.SelectedIndex = 0;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5076/api/User/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Instance.Jwt);

            var result = await client.GetFromJsonAsync<UserMiniProfileDto>("MiniProfile");

            FullNameTextBlock.Text = result.FullName;
            RoleTextBlock.Text = result.RoleName;
            AvatarImage.Source = LoadImageFromBase64(result.Image);
        }

        public BitmapImage LoadImageFromBase64(string base64String)
        {
            // 1. Убираем префикс "data:image/png;base64,", если он есть
            string cleanBase64 = base64String.Contains(",")
                ? base64String.Split(',')[1]
                : base64String;

            byte[] binaryData = Convert.FromBase64String(cleanBase64);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binaryData);
            bi.CacheOption = BitmapCacheOption.OnLoad; // Важно для освобождения потока
            bi.EndInit();
            bi.Freeze(); // Позволяет использовать картинку в разных потоках UI

            return bi;
        }
    }
}