using AutoMapper;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Uploader.Contracts.Response;
using MilkMatrix.Uploader.Models.Request;
using MilkMatrix.Uploader.Models.Response;

namespace MilkMatrix.Infrastructure.Models.AutomapperProfiles;

public class UploaderProfileMapping : Profile
{
    public UploaderProfileMapping()
    {
        
        CreateMap<UploadRequest, FileRequest>()
           .ForMember(dest => dest.FolderType, opt => opt.MapFrom(src => src.FolderType))
           .ForMember(dest => dest.FormFile, opt => opt.MapFrom(src => src.FormFile));
        CreateMap<IFileResponse, IUploadResponse>();
        CreateMap<IUploadResponse, IFileResponse>();
        CreateMap<UploadResponse, FileResponse>();
        CreateMap<FileResponse, UploadResponse>();
    }
}
