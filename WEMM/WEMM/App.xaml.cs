using System.Configuration;
using System.Data;
using System.Windows;
using WEMMApi.Contexts;

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
