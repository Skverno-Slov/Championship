using System.Windows;
using WEMMApi.Controllers;
using WEMMApi.Contexts;
using WEMMApi.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using System.Net.Http;
using WEMMApi.Services;
using System.Net.Http.Json;
using Azure.Identity;
using WEMM;

namespace WEMMWpf.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
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

                using HttpClient client = new();
                client.BaseAddress = new Uri("http://localhost:5076/api/Authorization/");

                var request = new LoginRequest()
                {
                    Login = email,
                    Password = password
                };

                var response = await client.PostAsJsonAsync("Token", request);
                if (!response.IsSuccessStatusCode)
                    throw new AuthenticationFailedException("Неверный логи или пароль");

                var result = await response.Content.ReadFromJsonAsync<TokenResponce>();

                UserSession.Instance.Jwt = result.JwtToken;

                var window = new MainWindow();
                window.Show();
                Close();
            }
            catch (BadHttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (AuthenticationFailedException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch 
            {
                MessageBox.Show("Непредвиденная ошибка");
            }
        }
    }
}
