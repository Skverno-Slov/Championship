using ClosedXML.Excel;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using VendingApi.Dtos;
using WEMMWpf;
using Excel = Microsoft.Office.Interop.Excel;

namespace VendingWpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для MonitorMachines.xaml
    /// </summary>
    public partial class MonitorMachinesPage : Page, INotifyPropertyChanged
    {
        HttpClient _client = new();

        List<string> _generalFilters = new List<string>();
        List<string> _offerFilters = new List<string>();

        List<MachineMonitorDto> _machines = new List<MachineMonitorDto>();

        public List<MachineMonitorDto> Machines
        {
            get => _machines;
            set
            {
                _machines = value;
                OnPropertyChanged();
            }
        }

        public MonitorMachinesPage()
        {
            InitializeComponent();

            DataContext = this;

            _client.BaseAddress = new Uri("http://localhost:5076/api/Machine/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Instance.Jwt);
        }

        private async Task UpdateUi()
        {
            var request = new MonitorRequest()
            {
                GeneralFilters = _generalFilters,
                OfferFilters = _offerFilters,
            };

            var response = await _client.PostAsJsonAsync("Monitor", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<MonitorResponce>();

                Machines = result.Machines;

                if (Machines.Count == 0)
                {
                    MachinesDataGrid.Visibility = Visibility.Collapsed;
                    NotFoundTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    MachinesDataGrid.Visibility = Visibility.Visible;
                    NotFoundTextBlock.Visibility = Visibility.Collapsed;
                }

                TotalMachinestextBlock.Text = result.TotalMachines.ToString();
                BrokenTextBlock.Text = result.TotalBroken.ToString();
                ServicedTextBlock.Text = result.TotalServing.ToString();
                WorkingtextBlock.Text = result.TotalWorking.ToString();

                TotalMoneyTextBlock.Text = result.TotalContributedMoney.ToString();
                ChangeTextBlock.Text = result.TotalChange.ToString();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private async void MonitorMachinesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateUi();
        }

        private async void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            await UpdateUi();
        }

        private void GeneralFilterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _generalFilters.Add((sender as ToggleButton).Tag.ToString());
        }

        private void GeneralFilterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _generalFilters.Remove((sender as ToggleButton).Tag.ToString());
        }

        private void OfferFilterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _offerFilters.Add((sender as ToggleButton).Tag.ToString());
        }

        private void OfferFilterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _offerFilters.Remove((sender as ToggleButton).Tag.ToString());
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Таблица Excel|*.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Machines");

                        worksheet.Cell(1, 1).Value = "FullName";
                        worksheet.Cell(1, 2).Value = "FullDescription";
                        worksheet.Cell(1, 3).Value = "ContributedMoney";
                        worksheet.Cell(1, 4).Value = "CoinsChange";
                        worksheet.Cell(1, 5).Value = "BillsChange";
                        worksheet.Cell(1, 6).Value = "LastSale";

                        int row = 2;
                        foreach (var machine in Machines)
                        {
                            worksheet.Cell(row, 1).Value = machine.FullName;
                            worksheet.Cell(row, 2).Value = machine.FullDescription;
                            worksheet.Cell(row, 3).Value = machine.ContributedMoney;
                            worksheet.Cell(row, 4).Value = machine.CoinsChange;
                            worksheet.Cell(row, 5).Value = machine.BillsChange;
                            worksheet.Cell(row, 6).Value = machine.LastSale;
                            row++;
                        }

                        worksheet.Columns().AdjustToContents(); 
                        workbook.SaveAs(saveFileDialog.FileName);
                    }
                    MessageBox.Show("Экспорт завершен", "Успех");
                }
                catch
                {
                    MessageBox.Show($"Ошибка экспорта", "Ошибка");
                }
            }
        }
    }
}
