﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models.Project;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IProjectService
  {
    Task<List<Guid>> CheckFilesAsync(List<Guid> filesIds, List<string> errors = null);

    Task<List<ProjectUserData>> GetProjectUsersAsync(List<Guid> usersIds);

    Task<(ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole)> GetProjectUserRole(Guid projectId, Guid userId);
  }
}
