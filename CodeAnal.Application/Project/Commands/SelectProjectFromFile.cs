using CodeAnal.Application.Attributes;
using MediatR;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace CodeAnal.Application.Project.Commands
{
    [CliCommand( Name = "selectFromFile")]
    public class SelectProjectFromFile : IRequest<CodeAnal.Domain.Entities.Project>
    {
        public string FilePath { get; set; }
    }

    public class SelectProjectFromFileHandler : IRequestHandler<SelectProjectFromFile, CodeAnal.Domain.Entities.Project>
    {

        public async Task<CodeAnal.Domain.Entities.Project> Handle(SelectProjectFromFile request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(SelectProjectFromFileHandler)} - started");
            Process buildProcess = new Process();
            ProcessStartInfo buildProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
            buildProcessStartInfo.FileName = "C:\\Program Files\\dotnet\\dotnet.exe";
            buildProcessStartInfo.Arguments = $"build --output \".\\output\" \"{request.FilePath}\"";
            buildProcess.StartInfo = buildProcessStartInfo;

            Console.WriteLine($"{nameof(SelectProjectFromFileHandler)} - start process {buildProcess.StartInfo.FileName} {buildProcess.StartInfo.Arguments}");

            buildProcess.Start();
            Thread.Sleep(TimeSpan.FromSeconds(15));

            var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), $"output\\{Path.GetFileNameWithoutExtension(request.FilePath)}.dll"));
            var result = new CodeAnal.Domain.Entities.Project();
            var classes = new List<Domain.Entities.Class>();
            var types = assembly.GetTypes();

            foreach (var item in types.Where(m => m.IsClass))
            {
                classes.Add(new Domain.Entities.Class()
                {
                    Name = item.Name,
                    SystemType = item
                });
            }
            result.Classes = classes;
            Console.WriteLine($"{nameof(SelectProjectFromFileHandler)} - ended");
            return result;
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
