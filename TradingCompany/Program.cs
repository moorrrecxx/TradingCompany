using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingCompany.DALEF.Concrete;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TradingCompany.DALEF.MapperProfiles;
using TradingCompany.DTO;
using TradingCompany.DALEF.Interfaces;

namespace TradingCompany
{
    internal class Program
    {
        static string _connString;
        static IMapper _mapper;

        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsetting.json")
               .Build();
            _connString = configuration.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddLogging(b =>
            {
                b.ClearProviders();
                b.AddConsole();
                b.SetMinimumLevel(LogLevel.Information);
            });

            services.AddDbContext<TradingCompanyContex>(opt =>
                opt.UseSqlServer(_connString)
                   .EnableSensitiveDataLogging());

            services.AddScoped<IUserDAL>(provider => new UserDAL(_connString, _mapper)
                );

            services.AddSingleton(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                }, loggerFactory);

                return config.CreateMapper(provider.GetService);
            });

            var provider = services.BuildServiceProvider();

            Console.WriteLine("Welcome to TradingCompany!");
            while (true)
            {
                Console.WriteLine("\nSelect tables:");
                Console.WriteLine("1 - Prodoucts table");
                Console.WriteLine("2 - Actions table");
                Console.WriteLine("3 - ActionProduct table");
                Console.WriteLine("4 - Category table");
                Console.WriteLine("5 - Status table");
                Console.WriteLine("6- User table");
                Console.WriteLine("q - Quit");
                Console.Write("Your choice: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);
                switch (c)
                {
                    case '1':

                        break;
                    case '2':
                        ;
                        break;
                    case '3':

                        break;
                    case '4':

                        break;
                    case '5':

                        break;
                    case '6':
                        UserMenu(provider);
                        break;
                    case 'q':
                        Console.WriteLine("Bye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }

        }

        private static void UserMenu(ServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("\nType:");
                Console.WriteLine("1 - Get all Users");
                Console.WriteLine("2 - Insert a User");
                Console.WriteLine("3-  Update User");
                Console.WriteLine("4 - Delete User");
                Console.WriteLine("q - Quit");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);


                switch (c)
                {
                    case '1':
                        
                        break;

                    case '2':
                        using (var scope = provider.CreateScope())
                        {
                            var userDal = scope.ServiceProvider.GetRequiredService<IUserDAL>();

                            Console.WriteLine("Enter Login:");
                            var login = Console.ReadLine();
                            Console.Write("Enter Email: ");
                            var email = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            var password = Console.ReadLine();
                            var userDto = new UserDTO
                            {
                                Login = login,
                                Email = email
                            };
                            var createdUser = userDal.Create(userDto, password);
                            Console.WriteLine($"User created with ID: {createdUser.UserId}");
                        }
                        break;

                    case '3':
                        
                        break;

                    case '4':
                        
                        break;

                    case 'q':
                        Console.WriteLine("Bye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
