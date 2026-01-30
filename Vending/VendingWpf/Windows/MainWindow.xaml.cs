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
using WEMMWpf.Windows;
using System.Net;

namespace WEMM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _client = new();
        public MainWindow()
        {
            InitializeComponent();
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

        private void ChangeAdministratePanelVisibility()
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

        private void SetClientProperties()
        {
            _client.BaseAddress = new Uri("http://localhost:5076/api/User/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Instance.Jwt);
        }

        public BitmapImage LoadImageFromBase64(string base64String)
        {
            string cleanBase64 = base64String.Contains(",")
                ? base64String.Split(',')[1]
                : base64String;

            byte[] binaryData = Convert.FromBase64String(cleanBase64);

            BitmapImage bi = new();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binaryData);
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();
            bi.Freeze();

            return bi;
        }

        private void BurgerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeBurgerMenuVisibility(sender);
        }

        private void AdministrateButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAdministratePanelVisibility();
        }

        private void ProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfileComboBox.SelectedIndex = 0;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetClientProperties();

                var result = await _client.GetFromJsonAsync<UserMiniProfileDto>("MiniProfile");

                FullNameTextBlock.Text = result.FullName;
                RoleTextBlock.Text = result.RoleName;
                AvatarImage.Source = LoadImageFromBase64(result.Image);

                MainFrame.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
            }
            catch (HttpRequestException ex)
            when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Треюуется повторный вход");
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Instance.Jwt = null;
            AuthorizationWindow window = new();
            window.Show();
            Close();
        }

        private void WendingMachinesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/WendingMachinesPage.xaml", UriKind.Relative));
            RouteTextBlock.Text = "Администрирование/Торговые автоматы";
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
            RouteTextBlock.Text = "Главная";
        }
    }
}