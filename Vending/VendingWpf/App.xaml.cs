using System.Configuration;
using System.Data;
using System.Windows;
using VendingApi.Contexts;

namespace WEMM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppDbContext context = new();
    }

}
