using MediatR;

namespace CodeAnal.Application.Solution.Commands
{
    public class SelectSolutionsFromFolder : IRequest<IEnumerable<CodeAnal.Domain.Entities.Solution>>
    {
        public string FolderPath { get; set; }
    }

    public class SelectSolutionsFromFolderHandler : IRequestHandler<SelectSolutionsFromFolder, IEnumerable<CodeAnal.Domain.Entities.Solution>>
    {
        private IMediator Mediator { get; set; }

        public SelectSolutionsFromFolderHandler(IMediator mediator)
        {
            Mediator = mediator;
        }
        public async Task<IEnumerable<CodeAnal.Domain.Entities.Solution>> Handle(SelectSolutionsFromFolder request, CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(request.FolderPath);
            if ((files == null) || (!files.Any()))
            {
                throw new CodeAnal.Domain.Exceptions.SolutionNotFoundException($"Solution folder hane not any files in folder {request.FolderPath}");
            }

            var solutionFiles = files
                .Where(m => Path.GetExtension(m).Equals("sln"))
                .ToList(); 

            if ((solutionFiles == null) || (!solutionFiles.Any()))
            {
                throw new CodeAnal.Domain.Exceptions.SolutionNotFoundException($"Solution folder hane not any solution files in folder {request.FolderPath}");
            }



            return solutionFiles.Select(async m => await Mediator.Send(new SelectSolutionFromFile()
            {
                FilePath = m
            }))
            .Select(m => m.Result)
            .ToList();
        }
    }
}
