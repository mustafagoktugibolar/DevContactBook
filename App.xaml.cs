using DevContactBook.Services;
using DevContactBook.ViewModels;
using DevContactBook.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DevContactBook
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register services
            services.AddSingleton<ICustomDialogService, CustomDialogService>();

            // Register view models
            services.AddSingleton<ContactViewModel>();
            services.AddSingleton<AddUpdateContactViewModel>();

            // Register views
            services.AddTransient<MainWindow>();
            services.AddTransient<AddUpdateContactView>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
