using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WEMMApi.Dtos;
using WEMMWpf.Windows;

namespace WEMMWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для WendingMachinesPage.xaml
    /// </summary>
    public partial class WendingMachinesPage : Page, INotifyPropertyChanged
    {
        private const int CommonPageSize = 5;

        private double _wendingMachineDockPanelX;
        private double _wendingMachineDockPanelY;
        private bool _isDragging;
        private Point _clickPosition;
        private List<MachineDto>? _machineDtos;
        private int _currentPage = 1;
        private int _pageSize = CommonPageSize;
        private int _totalMachines = 0;

        private HttpClient _client = new();
        private string? _filter;

        public WendingMachinesPage()
        {
            InitializeComponent();

            DataContext = this;
        }

        public double WendingMachineDockPanelX
        {
            get => _wendingMachineDockPanelX;
            set
            {
                _wendingMachineDockPanelX = value;
                OnPropertyChanged();
            }
        }

        public double WendingMachineDockPanelY
        {
            get => _wendingMachineDockPanelY;
            set
            {
                _wendingMachineDockPanelY = value;
                OnPropertyChanged();
            }
        }

        public List<MachineDto>? MachineDtos
        {
            get => _machineDtos;
            set
            {
                _machineDtos = value;
                OnPropertyChanged();
            }
        }

        private void SetClientProperties()
        {
            _client.BaseAddress = new Uri("http://localhost:5076/api/Machine/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Instance.Jwt);
        }

        private void ChangeHideButtonImage(string imageName)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();

            image.UriSource = new Uri($"pack://application:,,,/Images/{imageName}", UriKind.Absolute);

            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            HideButtonImage.Source = image;
        }

        private void UpdateUi(MashineResponse result)
        {
            List<MachineDto> machines = UpdateTable(result);
            UpdateRecordsInfo(machines);

            LockPaginationButtons();
        }

        private List<MachineDto> UpdateTable(MashineResponse result)
        {
            MachineDtos?.Clear();
            var machines = result.MachineDtos;
            _totalMachines = result.TotalCount;

            MachineDtos = machines;
            return machines;
        }

        private void UpdateRecordsInfo(List<MachineDto> machines)
        {
            SearchResultTextBlock.Text = $"Всего найдено {(machines?.Count is null ? 0 : machines.Count)} шт";
            PageTextBox.Text = _currentPage.ToString();
            RecordsTextBlock.Text = $"Записи с {((_currentPage - 1) * _pageSize) + 1} по {_currentPage * _pageSize} из {_totalMachines} записей";
        }

        private async Task<MashineResponse> GetMachinesAsync()
        {
            try
            {
                var result = await _client.GetFromJsonAsync<MashineResponse>($"Table?currentPage={_currentPage}&pageSize={_pageSize}&filter={_filter}");
                return result;
            }
            catch (HttpRequestException ex)
            when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Треюуется повторный вход");
                return new();
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return new();
            }
        }

        private void ApplyDefaultPageSize()
        {
            PageSizeTextBlock.Text = CommonPageSize.ToString();
            _pageSize = CommonPageSize;
        }

        private void LockPaginationButtons()
        {
            BackButton.IsEnabled = !(_currentPage == 1);

            int totalPages = (int)Math.Ceiling((double)_totalMachines / _pageSize);

            NextButton.IsEnabled = _currentPage < totalPages;
        }

        private static void OpenRedactor()
        {
            var window = new MachineRedactorWindow();
            window.ShowDialog();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void WendingMachinesDockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            var element = sender as FrameworkElement;
            _clickPosition = e.GetPosition(element);

            element.CaptureMouse();
        }

        private void WendingMachinesDockPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            (sender as FrameworkElement).ReleaseMouseCapture();
        }

        private void WendingMachinesDockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                WendingMachineDockPanelX = e.GetPosition(this).X - _clickPosition.X;
                WendingMachineDockPanelY = e.GetPosition(this).Y - _clickPosition.Y;
            }
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            if (MachinesDockPanel.Visibility != Visibility.Visible)
            {
                MachinesDockPanel.Visibility = Visibility.Visible;
                ChangeHideButtonImage("blueEye.png");
                return;
            }
            ChangeHideButtonImage("Eyent.png");
            MachinesDockPanel.Visibility = Visibility.Collapsed;
        }

        private async void MachinePage_Loaded(object sender, RoutedEventArgs e)
        {
            SetClientProperties();

            var result = await GetMachinesAsync();
            UpdateUi(result);
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            var result = await GetMachinesAsync();
            UpdateUi(result);
        }

        private async void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filter = FilterTextBox.Text;
            var result = await GetMachinesAsync();
            UpdateUi(result);
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            OpenRedactor();

            var result = await GetMachinesAsync();
            UpdateUi(result);
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage--;
            var result = await GetMachinesAsync();
            UpdateUi(result);
        }

        private async void PageSizeTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            MashineResponse result = new();

            if (!Int32.TryParse(PageSizeTextBlock.Text, out int pageSize))
            {
                ApplyDefaultPageSize();
                result = await GetMachinesAsync();
                UpdateUi(result);
                return;
            }

            if (pageSize <= 0)
            {
                ApplyDefaultPageSize();
                result = await GetMachinesAsync();
                UpdateUi(result);
                return;
            }

            _pageSize = pageSize;
            result = await GetMachinesAsync();
            UpdateUi(result);
        }
    }
}
