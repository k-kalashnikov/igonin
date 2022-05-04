using CodeAnal.Application.Attributes;
using MediatR;
using System.Reflection;

namespace CodeAnal.Cli
{
    public class CommandInvoker : IInvoker
    {
        private string[] Args { get; set; }
        private IMediator Mediator { get; set; }
        private object Result { get; set; }

        public CommandInvoker(IMediator mediatR)
        {
            Mediator = mediatR;
        }

        public async void Invoke(string[] args)
        {
            Args = args;
            var commandType = GetCommandType();
            var command = Activator.CreateInstance(commandType);
            var index = 1;
            foreach (var item in commandType.GetProperties())
            {
                item.SetValue(command, Args[index]);
                index++;
            }

            Result = await Mediator.Send(command);
        }
        public void OutputToConsole()
        {
            Console.WriteLine(Result?.ToString() ?? "");
        }

        public void OutputToFile()
        {

        }

        private Type GetCommandType()
        {
            var commandList = typeof(CliCommandAttribute)
                .Assembly
                .GetTypes()
                .Where(m => (m.GetCustomAttribute<CliCommandAttribute>() != null))
                .Select(m => new {
                    CommandName = m.GetCustomAttribute<CliCommandAttribute>().Name,
                    CommandType = m
                });

            if (Args?.Length < 1)
            {
                throw new ArgumentException("Enter command name");
            }

            if (Args?.Length < 2)
            {
                throw new ArgumentException("Enter command argument");
            }

            if (!commandList.Any(m => m.CommandName.ToLower().Equals(Args[0].ToLower())))
            {

                throw new ArgumentException($"Command with name {Args[0]} not found");
            }

            return commandList.First(m => m.CommandName.ToLower().Equals(Args[0].ToLower())).CommandType;
        }


    }
}
