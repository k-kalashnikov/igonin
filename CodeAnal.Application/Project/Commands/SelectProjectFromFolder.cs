using CodeAnal.Application.Attributes;
using MediatR;

namespace CodeAnal.Application.Project.Commands
{
    [CliCommand(Name = "selectFromFolder")]
    public class SelectProjectsFromFolder : IRequest<IEnumerable<CodeAnal.Domain.Entities.Project>>
    {
        public string FolderPath { get; set; }
    }

    public class SelectProjectsFromFolderHandler : IRequestHandler<SelectProjectsFromFolder, IEnumerable<CodeAnal.Domain.Entities.Project>>
    {
        private IMediator Mediator { get; set; }

        public SelectProjectsFromFolderHandler(IMediator mediator)
        {
            Mediator = mediator;
        }
        public async Task<IEnumerable<CodeAnal.Domain.Entities.Project>> Handle(SelectProjectsFromFolder request, CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(request.FolderPath);
            if ((files == null) || (!files.Any()))
            {
                throw new CodeAnal.Domain.Exceptions.ProjectNotFoundException($"Project folder hane not any files in folder {request.FolderPath}");
            }

            var ProjectFiles = files
                .Where(m => Path.GetExtension(m).Equals("csproj"))
                .ToList(); 

            if ((ProjectFiles == null) || (!ProjectFiles.Any()))
            {
                throw new CodeAnal.Domain.Exceptions.ProjectNotFoundException($"Project folder hane not any Project files in folder {request.FolderPath}");
            }



            return ProjectFiles.Select(async m => await Mediator.Send(new SelectProjectFromFile()
            {
                FilePath = m
            }))
            .Select(m => m.Result)
            .ToList();
        }
    }
}
