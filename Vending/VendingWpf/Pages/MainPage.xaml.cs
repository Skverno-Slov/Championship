using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VendingApi.Dtos;
using VendingApi.Models;
using Colors = ScottPlot.Colors;

namespace WEMMWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private double _effishesyDockPanelX;
        private double _effishesyDockPanelY;
        private double _stateDockPanelX;
        private double _stateDockPanelY;
        private double _summaryDockPanelX;
        private double _summaryDockPanelY;
        private double _newsDockPanelX;
        private double _newsDockPanelY;
        private double _salesDockPanelX;
        private double _salesDockPanelY;
        private List<News> _news;
        private List<Sale> _sales;
        private SalesDto _salesToday;

        private HttpClient _client = new();
        private EfficiencyResponceDto _efficiencyResponce;

        private bool _isDragging;
        private Point _clickPosition;
        private int _totalMachines;

        public MainPage()
        {
            InitializeComponent();

            DataContext = this;
        }

        public SalesDto SaleToday
        {
            get => _salesToday;
            set
            {
                _salesToday = value;
                OnPropertyChanged();
            }
        }

        public List<News> News
        {
            get => _news;
            set
            {
                _news = value;
                OnPropertyChanged();
            }
        }

        public int TotalMachines
        {
            get => _totalMachines;
            set
            {
                _totalMachines = value;
                OnPropertyChanged();
            }
        }

        public double EffishesyDockPanelX
        {
            get => _effishesyDockPanelX;
            set
            {
                _effishesyDockPanelX = value;
                OnPropertyChanged();
            }
        }

        public double EffishesyDockPanelY
        {
            get => _effishesyDockPanelY;
            set
            {
                _effishesyDockPanelY = value;
                OnPropertyChanged();
            }
        }

        public double StateDockPanelX
        {
            get => _stateDockPanelX;
            set
            {
                _stateDockPanelX = value;
                OnPropertyChanged();
            }
        }

        public double StateDockPanelY
        {
            get => _stateDockPanelY;
            set
            {
                _stateDockPanelY = value;
                OnPropertyChanged();
            }
        }

        public double SummaryDockPanelX
        {
            get => _summaryDockPanelX;
            set
            {
                _summaryDockPanelX = value;
                OnPropertyChanged();
            }
        }

        public double SummaryDockPanelY
        {
            get => _summaryDockPanelY;
            set
            {
                _summaryDockPanelY = value;
                OnPropertyChanged();
            }
        }

        public double NewsDockPanelX
        {
            get => _newsDockPanelX;
            set
            {
                _newsDockPanelX = value;
                OnPropertyChanged();
            }
        }

        public double NewsDockPanelY
        {
            get => _newsDockPanelY;
            set
            {
                _newsDockPanelY = value;
                OnPropertyChanged();
            }
        }

        public double SalesDockPanelX
        {
            get => _salesDockPanelX;
            set
            {
                _salesDockPanelX = value;
                OnPropertyChanged();
            }
        }

        public double SalesDockPanelY
        {
            get => _salesDockPanelY;
            set
            {
                _salesDockPanelY = value;
                OnPropertyChanged();
            }
        }

        private void SetStartPositions()
        {
            StateDockPanelX = 410;
            SummaryDockPanelX = 820;
            SalesDockPanelY = 300;
            NewsDockPanelX = 820;
            NewsDockPanelY = 300;
        }

        private void TargetDockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            var element = sender as FrameworkElement;
            _clickPosition = e.GetPosition(element);

            element.CaptureMouse();
        }

        private void EffishesyDockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                EffishesyDockPanelX = e.GetPosition(this).X - _clickPosition.X;
                EffishesyDockPanelY = e.GetPosition(this).Y - _clickPosition.Y;
            }
        }

        private void TargetDockPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            (sender as FrameworkElement).ReleaseMouseCapture();
        }

        private void StateDockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                StateDockPanelX = e.GetPosition(this).X - _clickPosition.X;
                StateDockPanelY = e.GetPosition(this).Y - _clickPosition.Y;
            }
        }

        private void SummaryDockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                SummaryDockPanelX = e.GetPosition(this).X - _clickPosition.X;
                SummaryDockPanelY = e.GetPosition(this).Y - _clickPosition.Y;
            }
        }

        private void NewsDockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                NewsDockPanelX = e.GetPosition(this).X - _clickPosition.X;
                NewsDockPanelY = e.GetPosition(this).Y - _clickPosition.Y;
            }
        }

        private void SalesDockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                SalesDockPanelX = e.GetPosition(this).X - _clickPosition.X;
                SalesDockPanelY = e.GetPosition(this).Y - _clickPosition.Y;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetStartPositions();
            SetClientProperties();

            await LoadGaugeEfficiencyChart();
            await LoadGaugeStateChart();

            News = await _client.GetFromJsonAsync<List<News>>("News");
            SaleToday = await _client.GetFromJsonAsync<SalesDto>("Sales/Today");

            _sales = await _client.GetFromJsonAsync<List<Sale>>("Sales");
            LoadSalesChart();
        }

        private async Task<double> CalculateEfficiencyPercentageAsync()
        {
            _efficiencyResponce = await _client.GetFromJsonAsync<EfficiencyResponceDto>("Efficiency");

            TotalMachines = _efficiencyResponce.AllMashines;

            if (_efficiencyResponce.AllMashines == 0)
                return 0;

            return (double)_efficiencyResponce.WorkingMashines / (double)_efficiencyResponce.AllMashines * 100.0;
        }

        private void LoadSalesChart()
        {
            var daym9 = CalculateTotallByDay(-9);
            var daym8 = CalculateTotallByDay(-8);
            var daym7 = CalculateTotallByDay(-7);
            var daym6 = CalculateTotallByDay(-6);
            var daym5 = CalculateTotallByDay(-5);
            var daym4 = CalculateTotallByDay(-4);
            var daym3 = CalculateTotallByDay(-3);
            var daym2 = CalculateTotallByDay(-2);
            var daym1 = CalculateTotallByDay(-1);
            var daym0 = CalculateTotallByDay();

            decimal[] sales =
            {
                CalculateTotallByDay(-9),
                CalculateTotallByDay(-8),
                CalculateTotallByDay(-7),
                CalculateTotallByDay(-6),
                CalculateTotallByDay(-5),
                CalculateTotallByDay(-4),
                CalculateTotallByDay(-3),
                CalculateTotallByDay(-2),
                CalculateTotallByDay(-1),
                CalculateTotallByDay()
            };
            string[] dates =
            {
                DateTime.UtcNow.AddDays(-9).Date.ToString("d"), 
                DateTime.UtcNow.AddDays(-8).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-7).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-6).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-5).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-4).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-3).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-2).Date.ToString("d"),
                DateTime.UtcNow.AddDays(-1).Date.ToString("d"),
                DateTime.UtcNow.Date.ToString("d"),
            };
            double[] positions = ScottPlot.Generate.Consecutive(sales.Length);

            var barPlot = SalesPlot.Plot.Add.Bars(positions);

            var leftTotalAxes = 8;
            var bottomTotalAxes = 10;
            var leftValue = sales.Max() / leftTotalAxes;
            var scale = 0.00273321;

            var barPosition = 0;
            foreach (var bar in barPlot.Bars)
            {
                bar.Value = (double)sales[barPosition] * scale;
                barPosition++;
            }

            SalesPlot.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < leftTotalAxes; i++)
            {
                ((ScottPlot.TickGenerators.NumericManual)SalesPlot.Plot.Axes.Left.TickGenerator).AddMajor(positions[i], Convert.ToString(Math.Round(leftValue * (i), 4)));
            }

            SalesPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual();
            for (int i = 0; i < bottomTotalAxes; i++)
            {
                ((ScottPlot.TickGenerators.NumericManual)SalesPlot.Plot.Axes.Bottom.TickGenerator).AddMajor(positions[i], dates[i]);
            }

            SalesPlot.Plot.Axes.Bottom.TickLabelStyle.Rotation = 25;

            DatesTextBlock.Text = $"Данные по продажам с {DateTime.UtcNow.AddDays(-9).Date} по {DateTime.UtcNow.Date}";

            SalesPlot.Refresh();
        }

        private decimal CalculateTotallByDay(int day = 0)
        {
            return Math.Round(_sales.Where(s => s.Timestamp.Day == DateTime.UtcNow.AddDays(day).Day).Sum(s => s.TotalPrice), 4);
        }

        public async Task LoadGaugeEfficiencyChart()
        {
            double efficiencyValue = await CalculateEfficiencyPercentageAsync();

            var plot = EfficiencyPlot.Plot;

            var values = new double[] { 0, 100 };
            if (efficiencyValue > 0)
            {
                values = new double[] { efficiencyValue, 100 - efficiencyValue };

                EfficiencyTextBlock.Text = $"Работающих автоматов - {efficiencyValue}%";
            }

            var gauge = plot.Add.RadialGaugePlot(values);

            gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            gauge.MaximumAngle = 180;
            gauge.StartingAngle = 180;
            gauge.Colors = new[] { Colors.DarkGreen, Colors.DarkRed };
            gauge.ShowLevels = true;       

            plot.Layout.Frameless();          
            plot.DataBackground.Color = Colors.White; 

            EfficiencyPlot.Refresh();
        }

        public async Task LoadGaugeStateChart()
        {

            var plot = StatePlot.Plot;

            var values = new double[] { 0 };

            if (_efficiencyResponce.BrokenMashines > 0 || _efficiencyResponce.ServedMashines > 0 || _efficiencyResponce.WorkingMashines > 0)
            {
                values = new double[] { _efficiencyResponce.WorkingMashines, _efficiencyResponce.ServedMashines, _efficiencyResponce.BrokenMashines };

                StateTextBlock.Text = $"Под вопросом - {_efficiencyResponce.ServedMashines}";
            }

            var gauge = plot.Add.RadialGaugePlot(values);

            gauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            gauge.MaximumAngle = 360;
            gauge.StartingAngle = 270;
            gauge.Colors = new[] { Colors.DarkGreen, Colors.DarkOrange, Colors.DarkRed };
            gauge.ShowLevels = true;

            plot.Layout.Frameless();
            plot.DataBackground.Color = Colors.White;

            StatePlot.Refresh();
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

        private void EffishesyHideButton_Click(object sender, RoutedEventArgs e)
        {
            if (EfficiencyTextBlock.Visibility != Visibility.Visible)
            {
                EfficiencyTextBlock.Visibility = Visibility.Visible;
                EfficiencyPlot.Visibility = Visibility.Visible;
                HideEffishesyImage.Source = ChangeHideButtonImage("blueEye.png");
                return;
            }
            HideEffishesyImage.Source = ChangeHideButtonImage("Eyent.png");
            EfficiencyTextBlock.Visibility = Visibility.Collapsed;
            EfficiencyPlot.Visibility = Visibility.Collapsed;
        }

        private void StateHideButton_Click(object sender, RoutedEventArgs e)
        {
            if (StateTextBlock.Visibility != Visibility.Visible)
            {
                StateTextBlock.Visibility = Visibility.Visible;
                StatePlot.Visibility = Visibility.Visible;
                HideStateImage.Source = ChangeHideButtonImage("blueEye.png");
                return;
            }
            HideStateImage.Source = ChangeHideButtonImage("Eyent.png");
            StateTextBlock.Visibility = Visibility.Collapsed;
            StatePlot.Visibility = Visibility.Collapsed;
        }

        private void SummaryHideButton_Click(object sender, RoutedEventArgs e)
        {
            if (SummaryDodyStackPanel.Visibility != Visibility.Visible)
            {
                SummaryDodyStackPanel.Visibility = Visibility.Visible;
                HideStateImage.Source = ChangeHideButtonImage("blueEye.png");
                return;
            }
            HideSummaryImage.Source = ChangeHideButtonImage("Eyent.png");
            SummaryDodyStackPanel.Visibility = Visibility.Collapsed;
        }

        private void NewsHideButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewsDataGrid.Visibility != Visibility.Visible)
            {
                NewsDataGrid.Visibility = Visibility.Visible;
                HideStateImage.Source = ChangeHideButtonImage("blueEye.png");
                return;
            }
            HideNewsImage.Source = ChangeHideButtonImage("Eyent.png");
            NewsDataGrid.Visibility = Visibility.Collapsed;
        }

        private void SalesHideButton_Click(object sender, RoutedEventArgs e)
        {
            if (SalesPlot.Visibility != Visibility.Visible)
            {
                SalesPlot.Visibility = Visibility.Visible;
                DatesTextBlock.Visibility = Visibility.Visible;
                SalesVariationsStackPanel.Visibility = Visibility.Visible;
                HideSalesImage.Source = ChangeHideButtonImage("blueEye.png");
                return;
            }
            HideSalesImage.Source = ChangeHideButtonImage("Eyent.png");
            SalesPlot.Visibility = Visibility.Collapsed;
            DatesTextBlock.Visibility = Visibility.Collapsed;
            SalesVariationsStackPanel.Visibility = Visibility.Collapsed;
        }

        private BitmapImage ChangeHideButtonImage(string imageName)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();

            image.UriSource = new Uri($"pack://application:,,,/Images/{imageName}", UriKind.Absolute);

            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            return image;
        }
    }
}
