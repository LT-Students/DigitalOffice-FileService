using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models.Project;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IProjectService
  {
    Task<List<Guid>> CheckFilesAsync(List<Guid> filesIds, List<string> errors);

    Task<List<ProjectUserData>> GetProjectUsersAsync(List<Guid> usersIds);

    Task<(ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole)> CheckProjectAndUserExistenceAsync(Guid projectId, Guid userId);
  }
}
