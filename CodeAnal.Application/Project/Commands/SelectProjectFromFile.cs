using CodeAnal.Application.Attributes;
using MediatR;
using System.Diagnostics;
using System.Reflection;

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
            buildProcessStartInfo.Arguments = $"--output \".\\output\" \"{request.FilePath}\"";
            buildProcessStartInfo.RedirectStandardOutput = true;
            buildProcessStartInfo.RedirectStandardError = true;

            buildProcess.StartInfo = buildProcessStartInfo;

            buildProcess.OutputDataReceived += OutputDataReceived;
            buildProcess.ErrorDataReceived += OutputDataReceived;

            Console.WriteLine($"{nameof(SelectProjectFromFileHandler)} - start process {buildProcess.StartInfo.FileName} {buildProcess.StartInfo.Arguments}");

            buildProcess.Start();
            buildProcess.WaitForExit();

            var assembly = Assembly.LoadFile($"\".\\output{Path.GetFileNameWithoutExtension(request.FilePath)}.dll\"");
            var result = new CodeAnal.Domain.Entities.Project();
            var classes = new List<Domain.Entities.Class>();

            foreach (var item in assembly.GetTypes())
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
