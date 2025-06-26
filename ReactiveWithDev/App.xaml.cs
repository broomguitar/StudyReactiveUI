using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static IServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            serviceProvider = new ServiceCollection().AddSingleton<Test2>().AddSingleton<Test>().BuildServiceProvider();
            base.OnStartup(e);
        }

        public static T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }
    }
}