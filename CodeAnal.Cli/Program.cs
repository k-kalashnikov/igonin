using CodeAnal.Application.Attributes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeAnal.Cli
{
    public static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serviceProvider = ServiceConfigurate();
                var services = serviceProvider.GetServices<IInvoker>();
                foreach (var item in services)
                {
                    Console.WriteLine($"Service type = {item.GetType().Name}");
                    item.Invoke(args);
                    item.OutputToConsole();
                }
            }
            catch (ArgumentException ex)
            {
                ArgumentExceptionHandler(ex);
            }
        }

        static IServiceProvider ServiceConfigurate()
        {
            var serviceProvider = new ServiceCollection()
                .AddMediatR(typeof(CliCommandAttribute).Assembly)
                .AddSingleton<IInvoker, CommandInvoker>()
                .BuildServiceProvider();

            return serviceProvider;
        }

        static void ArgumentExceptionHandler(ArgumentException exception)
        {
            var commandList = typeof(CliCommandAttribute)
                .Assembly
                .GetTypes()
                .Where(m => (m.GetCustomAttribute<CliCommandAttribute>() != null))
                .Select(m => new {
                    CommandName = m.GetCustomAttribute<CliCommandAttribute>().Name,
                    CommandType = m
                });

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
            Console.ResetColor();

            Console.WriteLine($"Posible commands:\n" +
                    $"{string.Join("\n", commandList.Select(m => $"Name - {m.CommandName}, Arguments - {string.Join(" ", m.CommandType.GetProperties().Select(p => $"{p.Name} {p.PropertyType.Name}"))}"))}");
        }
    }
}