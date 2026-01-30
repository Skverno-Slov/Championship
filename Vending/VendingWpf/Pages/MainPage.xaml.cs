using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private bool _isDragging;
        private Point _clickPosition;

        public MainPage()
        {
            InitializeComponent();

            DataContext = this;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetStartPositions();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
