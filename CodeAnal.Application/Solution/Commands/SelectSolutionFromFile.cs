using MediatR;

namespace CodeAnal.Application.Solution.Commands
{
    public class SelectSolutionFromFile : IRequest<CodeAnal.Domain.Entities.Solution>
    {
        public string FilePath { get; set; }
    }

    public class SelectSolutionFromFileHandler : IRequestHandler<SelectSolutionFromFile, CodeAnal.Domain.Entities.Solution>
    {

        public async Task<CodeAnal.Domain.Entities.Solution> Handle(SelectSolutionFromFile request, CancellationToken cancellationToken)
        {
            Microsoft.Build.BuildEngine.Engine.GlobalEngine.BuildEnabled = true;
            Console.WriteLine($"Build Engine version is {Microsoft.Build.BuildEngine.Engine.GlobalEngine.DefaultToolsVersion}");
            var solution = new Microsoft.Build.BuildEngine.Project(Microsoft.Build.BuildEngine.Engine.GlobalEngine);
            solution.BuildEnabled = true;
            solution.Load(request.FilePath);

            solution.Build();
            return null;
        }
    }
}
