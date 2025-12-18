using AutoMapper;
using TradingCompany.BLL.Concreate;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DALEF.Interfaces;
using TradingCompany.DALEF.MapperProfiles;
using TradingCompany.DTO;

namespace TradingCompany.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddLog4Net("log4net.xml");
            });

            builder.Services.AddSingleton<IMapper>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.ConstructServicesUsing(sp.GetService);
                    cfg.AddMaps(typeof(MappingProfile).Assembly);
                }, loggerFactory);

                return config.CreateMapper();
            });

            string connStr = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddTransient<IOrderDAL>(sp => new OrderDAL (connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<IStatusDAL>(sp => new StatusDAL (connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<IOrderManager, OrderManager>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
