using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.ProjectService.Broker.Requests
{
  public class ProjectService : IProjectService
  {
    private readonly ILogger<ProjectService> _logger;
    private readonly IRequestClient<ICheckProjectFilesAccessesRequest> _rcCheckFiles;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectService(
      ILogger<ProjectService> logger,
      IRequestClient<ICheckProjectFilesAccessesRequest> rcCheckFiles,
      IHttpContextAccessor httpContextAccessor)
    {
      _logger = logger;
      _rcCheckFiles = rcCheckFiles;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Guid>> CheckFilesAsync(List<Guid> filesIds, List<string> errors)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      return await RequestHandler
        .ProcessRequest<ICheckProjectFilesAccessesRequest, List<Guid>>(
          _rcCheckFiles,
          ICheckProjectFilesAccessesRequest.CreateObj(_httpContextAccessor.HttpContext.GetUserId(), filesIds),
          errors,
          _logger);
    }
  }
}
