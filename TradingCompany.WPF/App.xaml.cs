using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.MapperProfiles;
using TradingCompany.WPF.Services.Concrete;
using TradingCompany.WPF.Services.Interfaces;
using TradingCompany.WPF.ViewModels;

namespace TradingCompany.WPF
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
            string connectionString = "Server=.;Database=TradingCompanyDB;Integrated Security=True;TrustServerCertificate=True";

            services.AddDbContext<TradingCompanyContex>(opt =>
              opt.UseSqlServer(connectionString)
                 .EnableSensitiveDataLogging());

            services.AddLogging(builder =>
            {
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            services.AddSingleton<IUserDAL>(p => new UserDAL(connectionString, p.GetRequiredService<IMapper>()));
            services.AddSingleton<IOrderDAL>(p => new OrderDAL(connectionString, p.GetRequiredService<IMapper>()));
            services.AddSingleton<IShipmentDAL>(p => new ShipmentDAL(connectionString, p.GetRequiredService<IMapper>()));
            services.AddSingleton<IStatusDAL>(p => new StatusDAL(connectionString, p.GetRequiredService<IMapper>()));
            services.AddSingleton<ILogDAL>(p => new LogDAL(connectionString, p.GetRequiredService<IMapper>()));
            services.AddSingleton<IUserRoleDAL>(p => new UserRoleDAL(connectionString, p.GetRequiredService<IMapper>()));

            services.AddSingleton(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                }, loggerFactory);

                return config.CreateMapper(provider.GetService);
            });

            services.AddSingleton<IAuthentication, Authentication>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<AdminHomeViewModel>(); 
            services.AddTransient<UserHomeViewModel>();  
            services.AddTransient<AdminHomeViewModel>();
            services.AddTransient<UserHomeViewModel>();
            services.AddTransient<OrderEditorViewModel>();
            services.AddTransient<ShipmentEditorViewModel>();
            services.AddTransient<StatusEditorViewModel>();
            services.AddTransient<UserEditorViewModel>();; 
            services.AddTransient<LogEditorViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected void OnStartup(object sender ,StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}