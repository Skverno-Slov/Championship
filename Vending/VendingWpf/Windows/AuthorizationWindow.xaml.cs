using Azure.Identity;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using WEMM;
using WEMMApi.Dtos;

namespace WEMMWpf.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private HttpClient _client = new();

        private Notifier _notifier;

        public AuthorizationWindow()
        {
            InitializeComponent();

            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(10),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void OpenMainWindow()
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private static async Task SendRequest(HttpResponseMessage response)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponce>();

            UserSession.Instance.Jwt = result.JwtToken;
        }

        private async Task<HttpResponseMessage> CreateResponce(string email, string password)
        {
            var request = new LoginRequest()
            {
                Login = email,
                Password = password
            };

            var response = await _client.PostAsJsonAsync("Login", request);
            return response;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;
            try
            {
                if (String.IsNullOrWhiteSpace(email))
                    throw new BadHttpRequestException("Введите логин");

                if (String.IsNullOrWhiteSpace(password))
                    throw new BadHttpRequestException("Введите пароль");

                HttpResponseMessage response = await CreateResponce(email, password);

                if (!response.IsSuccessStatusCode)
                    throw new AuthenticationFailedException("Неверный логи или пароль");

                await SendRequest(response);
                OpenMainWindow();
            }
            catch (BadHttpRequestException ex)
            {
                _notifier.ShowError(ex.Message);
            }
            catch (AuthenticationFailedException ex)
            {
                _notifier.ShowError(ex.Message);
            }
            catch
            {
                _notifier.ShowError("Непредвиденная ошибка");
            }
        }

        private void AuthorizationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _client.BaseAddress = new Uri("http://localhost:5076/api/Authorization/");
        }
    }
}
