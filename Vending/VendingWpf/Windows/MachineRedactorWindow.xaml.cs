using Microsoft.AspNetCore.Server.HttpSys;
using System.Net.Http;
using System.Security.Authentication;
using System.Windows;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WEMMApi.Dtos;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WEMMApi.Models;
using System.Threading.Tasks;
using System.Net;

namespace WEMMWpf.Windows
{
    /// <summary>
    /// Логика взаимодействия для MachineRedactorWindow.xaml
    /// </summary>
    public partial class MachineRedactorWindow : Window, INotifyPropertyChanged
    {
        private HttpClient _client = new();

        private Dictionary<int, string> _timeZones;
        private List<WorkMode> _workModes;
        private List<Model> _models;
        private List<Template> _templates;
        private List<MachinePlace> _machinePlaces;
        private List<UserListDto> _users;
        private List<WorkerListDto> _managers;
        private List<WorkerListDto> _enginers;
        private List<WorkerListDto> _technics;
        private List<ServicePriority> _servicePriorities;
        private VendingMachine _vendingMachine;

        public MachineRedactorWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public VendingMachine VendingMachine
        {
            get => _vendingMachine;
            set
            {
                _vendingMachine = value;
                OnPropertyChanged();
            }
        }

        public List<MachinePlace> MachinePlaces
        {
            get => _machinePlaces;
            set
            {
                _machinePlaces = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<int, string> TimeZones
        {
            get => _timeZones;
            set
            {
                _timeZones = value;
                OnPropertyChanged();
            }
        }

        public List<WorkMode> WorkModes 
        { 
            get => _workModes;
            set
            {
                _workModes = value;
                OnPropertyChanged();
            }
        }

        public List<Model> Models
        {
            get => _models;
            set
            {
                _models = value;
                OnPropertyChanged();
            }
        }

        public List<Template> Templates
        {
            get => _templates;
            set
            {
                _templates = value;
                OnPropertyChanged();
            }
        }

        public List<UserListDto> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public List<WorkerListDto> Managers
        {
            get => _managers;
            set
            {
                _managers = value;
                OnPropertyChanged();
            }
        }

        public List<WorkerListDto> Enginers
        {
            get => _enginers;
            set
            {
                _enginers = value;
                OnPropertyChanged();
            }
        }

        public List<WorkerListDto> Technics
        {
            get => _technics;
            set
            {
                _technics = value;
                OnPropertyChanged();
            }
        }

        public List<ServicePriority> ServicePriorities
        {
            get => _servicePriorities;
            set
            {
                _servicePriorities = value;
                OnPropertyChanged();
            }
        }

        private async void MachineRedactorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetClientProperties();
            await LoadDependences();

            VendingMachine = new();
        }

        private async Task LoadDependences()
        {
            try
            {
                var dependences = await _client.GetFromJsonAsync<DependencesDto>("Dependencies");

                WorkModes = dependences.WorkModes;
                Models = dependences.Models;
                Templates = dependences.Templates;
                Users = dependences.Users;
                Managers = dependences.Managers;
                Enginers = dependences.Enginers;
                Technics = dependences.Technics;
                MachinePlaces = dependences.MachinePlaces;
                ServicePriorities = dependences.ServicePriorities;

                var timeZones = new Dictionary<int, string>
            {
                { -12, "UTC-12" },
                { -11, "UTC-11" },
                { -10, "UTC-10" },
                { -9, "UTC-9" },
                { -8, "UTC-8" },
                { -7, "UTC-7" },
                { -6, "UTC-6" },
                { -5, "UTC-5" },
                { -4, "UTC-4" },
                { -3, "UTC-3" },
                { -2, "UTC-2" },
                { -1, "UTC-1" },
                { 0, "UTC+0" },
                { 1, "UTC+1" },
                { 2, "UTC+2" },
                { 3, "UTC+3" },
                { 4, "UTC+4" },
                { 5, "UTC+5" },
                { 6, "UTC+6" },
                { 7, "UTC+7" },
                { 8, "UTC+8" },
                { 9, "UTC+9" },
                { 10, "UTC+10" },
                { 11, "UTC+11" },
                { 12, "UTC+12" }
            };

                TimeZones = timeZones;
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

        private void SetClientProperties()
        {
            _client.BaseAddress = new Uri("http://localhost:5076/api/Machine/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Instance.Jwt);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
