using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Uploader;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using static MilkMatrix.Core.Entities.Common.Constants;

namespace MilkMatrix.Api.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UploadController : ControllerBase
{

    private readonly ILogging logging;
    private IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;
    private readonly FileConfig fileConfig;
    private readonly IFileUploader fileUploader;
    public UploadController(ILogging logging, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFileUploader fileUploader, IOptions<FileConfig> fileConfig)
    {
        this.logging = logging.ForContext("ServiceName", nameof(UploadController));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.httpContextAccessor = httpContextAccessor;
        this.fileUploader = fileUploader;
        this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig));
    }

    [HttpPost]
    [Route("files")]
    public async Task<ActionResult> UploadFile([FromForm] UploadRequest fileRequest)
    {
        try
        {
            if (!fileRequest.FormFile.Any())
            {
                logging.LogError(ErrorMessage.NothingSelectedToUpload);
                return BadRequest(new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = ErrorMessage.NothingSelectedToUpload });
            }

            if (fileRequest.FormFile.Count() > fileConfig.MaxFilesAllowedToUpload)
            {
                logging.LogError(ErrorMessage.MaxFileAllowedExceeded);
                return BadRequest(new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = ErrorMessage.MaxFileAllowedExceeded });
            }
            var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value ?? "";
            var fileResponse = await fileUploader.UploadFileWithName<UploadRequest, UploadResponse>(fileRequest, userId);

            if (!fileResponse.Any())
            {
                return BadRequest(new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = ErrorMessage.NoFileError });
            }
            else
            {

                var response = await fileUploader.SaveFileResponse(fileResponse.ToList());

                return response.Count() > 0
                    ? Ok(response)
                    : BadRequest(new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = ErrorMessage.FileUploadError });
            }
        }

        catch (Exception ex)
        {
            logging.LogError(ex.Message, ex);
            return BadRequest(new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = ex.Message }); ;
        }
    }
}
