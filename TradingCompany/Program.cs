using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingCompany.DALEF.Concrete;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TradingCompany.DALEF.MapperProfiles;
using TradingCompany.DTO;
using TradingCompany.DALEF.Interfaces;
using System.IO;
using System.Reflection;

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
                .AddJsonFile("appsetting.json", optional: true, reloadOnChange: true)
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

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddScoped<IUserDAL>(provider => new UserDAL(_connString, provider.GetRequiredService<IMapper>()));
            services.AddScoped<ILogDAL>(provider => new LogDAL(_connString, provider.GetRequiredService<IMapper>()));

            services.AddScoped<IStatusDAL>(provider => new StatusDAL(_connString, provider.GetRequiredService<IMapper>()));
            services.AddScoped<IOrderDAL>(provider => new OrderDAL(_connString, provider.GetRequiredService<IMapper>()));
            services.AddScoped<IShipmentDAL>(provider => new ShipmentDAL(_connString, provider.GetRequiredService<IMapper>()));

            var provider = services.BuildServiceProvider();

            _mapper = provider.GetRequiredService<IMapper>();

            Console.WriteLine("Welcome to TradingCompany!");

            while (true)
            {
                Console.WriteLine("\nSelect tables:");
                Console.WriteLine("1 - Status table");
                Console.WriteLine("2 - Log table");
                Console.WriteLine("3 - Shipment table");
                Console.WriteLine("4 - Order table");
                Console.WriteLine("5 - User table");
                Console.WriteLine("q - Quit");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);

                switch (c)
                {
                    case '1':
                        StatusMenu(provider);
                        break;
                    case '2':
                        LogMenu(provider);
                        break;
                    case '3':
                        ShipmentMenu(provider);
                        break;
                    case '4':
                        OrderMenu(provider);
                        break;
                    case '5':
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

        private static void StatusMenu(ServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("\n--- STATUS MENU ---");
                Console.WriteLine("1 - Get all Status");
                Console.WriteLine("2 - Add a Status");
                Console.WriteLine("3 - Update Status");
                Console.WriteLine("4 - Delete Status");
                Console.WriteLine("q - Back to Main Menu");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);

                switch (c)
                {
                    case '1':
                        using (var scope = provider.CreateScope())
                        {
                            var statusDAL = scope.ServiceProvider.GetRequiredService<IStatusDAL>();
                            var statuses = statusDAL.GetAll();
                            foreach (var status in statuses)
                            {
                                Console.WriteLine($"StatusID: {status.StatusId}, StatusName: {status.StatusName}");
                            }
                        }
                        break;
                    case '2':
                        using (var scope = provider.CreateScope())
                        {
                            var statusDal = scope.ServiceProvider.GetRequiredService<IStatusDAL>();
                            Console.WriteLine("Enter statusname:");
                            var statusname = Console.ReadLine();
                            var statusDto = new StatusDTO { StatusName = statusname };
                            var addStatus = statusDal.Create(statusDto);
                            Console.WriteLine($"Status created with ID: {addStatus.StatusId}");
                        }
                        break;
                    case '3':
                        using (var scope = provider.CreateScope())
                        {
                            var statusDal = scope.ServiceProvider.GetRequiredService<IStatusDAL>();
                            Console.WriteLine("Enter ID to update:");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                Console.WriteLine("Enter new statusname:");
                                var statusname = Console.ReadLine();
                                var statusDto = new StatusDTO { StatusId = id, StatusName = statusname };
                                statusDal.Update(statusDto);
                                Console.WriteLine($"Status updated.");
                            }
                        }
                        break;
                    case '4':
                        using (var scope = provider.CreateScope())
                        {
                            var statusDal = scope.ServiceProvider.GetRequiredService<IStatusDAL>();
                            Console.WriteLine("Enter statusid to delete:");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                statusDal.Delete(id);
                                Console.WriteLine("Status deleted.");
                            }
                        }
                        break;
                    case 'q':
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private static void LogMenu(ServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("\n--- LOG MENU ---");
                Console.WriteLine("1 - Get all Logs");
                Console.WriteLine("2 - Add a Log");
                Console.WriteLine("3 - Update Log");
                Console.WriteLine("4 - Delete Log");
                Console.WriteLine("q - Back to Main Menu");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);

                switch (c)
                {
                    case '1':
                        using (var scope = provider.CreateScope())
                        {
                            var logDAL = scope.ServiceProvider.GetRequiredService<ILogDAL>();
                            var logs = logDAL.GetAll();
                            foreach (var log in logs)
                            {
                                Console.WriteLine($"ID: {log.LogId}, UserID: {log.User?.UserId}, Action: {log.Action}, Created: {log.CreatedAt}");
                            }
                        }
                        break;
                    case '2':
                        using (var scope = provider.CreateScope())
                        {
                            try
                            {
                                var logDal = scope.ServiceProvider.GetRequiredService<ILogDAL>();
                                Console.WriteLine("Enter user_id:");
                                int user_id = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter Action: ");
                                var action = Console.ReadLine();

                                var logDto = new LogDTO
                                {
                                    User = new UserDTO { UserId = user_id },
                                    Action = action,
                                };
                                var addLog = logDal.Create(logDto);
                                Console.WriteLine($"Log created with ID: {addLog.LogId}");
                            }
                            catch (DbUpdateException ex)
                            {
                                Console.WriteLine($"Database Error: Could not create log. Verify that User ID exists. Details: {ex.InnerException?.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        break;
                    case '3':
                        using (var scope = provider.CreateScope())
                        {
                            try
                            {
                                var logDal = scope.ServiceProvider.GetRequiredService<ILogDAL>();
                                Console.WriteLine("Enter log id");
                                var inputid = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter new user id");
                                var newuserid = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter new action");
                                var action = Console.ReadLine();

                                var updatedlog = new LogDTO
                                {
                                    User = new UserDTO { UserId = newuserid },
                                    Action = action,
                                };
                                logDal.Update(updatedlog, inputid);
                                Console.WriteLine("Log updated.");
                            }
                            catch (DbUpdateException ex)
                            {
                                Console.WriteLine($"Database Error: Could not update log. Verify that User ID exists. Details: {ex.InnerException?.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        break;
                    case '4':
                        using (var scope = provider.CreateScope())
                        {
                            try
                            {
                                var logDal = scope.ServiceProvider.GetRequiredService<ILogDAL>();
                                Console.WriteLine("Enter log id");
                                var inputid = Convert.ToInt32(Console.ReadLine());
                                logDal.Delete(inputid);
                                Console.WriteLine("Log deleted.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        break;
                    case 'q':
                        return;
                }
            }
        }

        private static void ShipmentMenu(ServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("\n--- SHIPMENT MENU ---");
                Console.WriteLine("1 - Get all Shipments");
                Console.WriteLine("2 - Create Shipment");
                Console.WriteLine("3 - Update Shipment");
                Console.WriteLine("4 - Delete Shipment");
                Console.WriteLine("q - Back to Main Menu");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);

                switch (c)
                {
                    case '1':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IShipmentDAL>();
                            var items = dal.GetAll();
                            foreach (var item in items)
                            {
                                Console.WriteLine($"ID: {item.ShipmentId}, OrderID: {item.Order?.OrderId}, Confirmed: {item.Confirmed}");
                            }
                        }
                        break;
                    case '2':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IShipmentDAL>();
                            Console.Write("Enter Order ID for this shipment: ");
                            if (int.TryParse(Console.ReadLine(), out int orderId))
                            {
                                Console.Write("Is Confirmed? (true/false): ");
                                bool.TryParse(Console.ReadLine(), out bool isConfirmed);

                                var dto = new ShipmentDTO
                                {
                                    Order = new OrderDTO { OrderId = orderId },
                                    Confirmed = isConfirmed
                                };
                                var created = dal.Create(dto);
                                Console.WriteLine($"Shipment created with ID: {created.ShipmentId}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Order ID format.");
                            }
                        }
                        break;
                    case '3':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IShipmentDAL>();
                            Console.Write("Enter Shipment ID to update: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                Console.Write("Enter New Order ID: ");
                                int.TryParse(Console.ReadLine(), out int orderId);
                                Console.Write("Is Confirmed? (true/false): ");
                                bool.TryParse(Console.ReadLine(), out bool isConfirmed);

                                var dto = new ShipmentDTO
                                {
                                    ShipmentId = id,
                                    Order = new OrderDTO { OrderId = orderId },
                                    Confirmed = isConfirmed
                                };
                                dal.Update(dto);
                                Console.WriteLine("Shipment updated.");
                            }
                        }
                        break;
                    case '4':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IShipmentDAL>();
                            Console.Write("Enter Shipment ID to delete: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                dal.Delete(id);
                                Console.WriteLine("Shipment deleted.");
                            }
                        }
                        break;
                    case 'q':
                        return;
                }
            }
        }

        private static void OrderMenu(ServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("\n--- ORDER MENU ---");
                Console.WriteLine("1 - Get all Orders");
                Console.WriteLine("2 - Create Order");
                Console.WriteLine("3 - Update Order");
                Console.WriteLine("4 - Delete Order");
                Console.WriteLine("q - Back to Main Menu");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);

                switch (c)
                {
                    case '1':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IOrderDAL>();
                            var items = dal.GetAll();
                            foreach (var item in items)
                            {
                                var statusName = item.Status != null ? item.Status.StatusName : "No Status";
                                Console.WriteLine($"ID: {item.OrderId}, Customer: {item.CustomerName}, Phone: {item.Phone}, Status: {statusName}");
                            }
                        }
                        break;
                    case '2':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IOrderDAL>();

                            Console.Write("Enter Customer Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter Address: ");
                            string address = Console.ReadLine();

                            Console.Write("Enter Phone: ");
                            string phone = Console.ReadLine();

                            Console.Write("Enter Status ID: ");
                            int.TryParse(Console.ReadLine(), out int statusId);

                            var dto = new OrderDTO
                            {
                                CustomerName = name,
                                Address = address,
                                Phone = phone,
                                Status = new StatusDTO { StatusId = statusId }
                            };

                            var created = dal.Create(dto);
                            Console.WriteLine($"Order created with ID: {created.OrderId}");
                        }
                        break;
                    case '3':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IOrderDAL>();
                            Console.Write("Enter Order ID to update: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                Console.Write("Enter New Customer Name: ");
                                string name = Console.ReadLine();
                                Console.Write("Enter New Phone: ");
                                string phone = Console.ReadLine();
                                Console.Write("Enter New Address: ");
                                string address = Console.ReadLine();
                                Console.Write("Enter New Status ID: ");
                                int.TryParse(Console.ReadLine(), out int statusId);

                                var dto = new OrderDTO
                                {
                                    OrderId = id,
                                    CustomerName = name,
                                    Phone = phone,
                                    Address = address,
                                    Status = new StatusDTO { StatusId = statusId }
                                };
                                dal.Update(dto);
                                Console.WriteLine("Order updated.");
                            }
                        }
                        break;
                    case '4':
                        using (var scope = provider.CreateScope())
                        {
                            var dal = scope.ServiceProvider.GetRequiredService<IOrderDAL>();
                            Console.Write("Enter Order ID to delete: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                dal.Delete(id);
                                Console.WriteLine("Order deleted.");
                            }
                        }
                        break;
                    case 'q':
                        return;
                }
            }
        }

        private static void UserMenu(ServiceProvider provider)
        {
            while (true)
            {
                Console.WriteLine("\n--- USER MENU ---");
                Console.WriteLine("1 - Get all Users");
                Console.WriteLine("2 - Create a User");
                Console.WriteLine("3 - Update User ");
                Console.WriteLine("4 - Delete User ");
                Console.WriteLine("q - Back to Main Menu");
                Console.Write("Your choice: ");

                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var c = char.ToLowerInvariant(line[0]);

                switch (c)
                {
                    case '1':
                        using (var scope = provider.CreateScope())
                        {
                            var userDAL = scope.ServiceProvider.GetRequiredService<IUserDAL>();
                            try
                            {
                                var users = userDAL.GetAll();
                                foreach (var user in users)
                                {
                                    Console.WriteLine($"UserID: {user.UserId}, Login: {user.Login}, Email: {user.Email}");
                                }
                            }
                            catch (NotImplementedException)
                            {
                                Console.WriteLine("Error: Method not implemented in DAL.");
                            }
                        }
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
                    case 'q':
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}