using AutoMapper;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Api.Models.Request.Admin.Page;
using MilkMatrix.Api.Models.Request.Admin.Role;
using MilkMatrix.Api.Models.Request.Admin.RolePage;

namespace MilkMatrix.Api.Models.AutomapperProfiles;

public class RolePagesPermissionsProfile : Profile
{
    public RolePagesPermissionsProfile()
    {
        CreateMap<RoleUpsertModel, RoleUpdateRequest>()
                         .ForMember(x => x.RoleId, opt => opt.MapFrom(src => src.Id))
                         .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                         .ForMember(x => x.RoleName, opt => opt.MapFrom(src => src.Name))
                         .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.Status))
                         .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<RoleUpsertModel, RoleInsertRequest>()
                  .ForMember(x => x.RoleName, opt => opt.MapFrom(src => src.Name))
                  .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                  .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<PageUpsertModel, PageUpdateRequest>()
                 .ForMember(x => x.PageId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(x => x.PageUrl, opt => opt.MapFrom(src => src.Url))
                 .ForMember(x => x.PageName, opt => opt.MapFrom(src => src.Name))
                 .ForMember(x => x.ModuleId, opt => opt.MapFrom(src => src.Module))
                 .ForMember(x => x.SubModuleId, opt => opt.MapFrom(src => src.SubModule))
                 .ForMember(x => x.PageOrder, opt => opt.MapFrom(src => src.Order))
                 .ForMember(x => x.PageIcon, opt => opt.MapFrom(src => src.Icon))
                 .ForMember(x => x.IsMenu, opt => opt.MapFrom(src => src.IsMenu))
                 .ForMember(x => x.ActionDetails, opt => opt.MapFrom(src => src.ActionDetails))
                 .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.Status))
                 .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<PageUpsertModel, PageInsertRequest>()
                 .ForMember(x => x.PageUrl, opt => opt.MapFrom(src => src.Url))
                 .ForMember(x => x.PageName, opt => opt.MapFrom(src => src.Name))
                 .ForMember(x => x.ModuleId, opt => opt.MapFrom(src => src.Module))
                 .ForMember(x => x.SubModuleId, opt => opt.MapFrom(src => src.SubModule))
                 .ForMember(x => x.PageOrder, opt => opt.MapFrom(src => src.Order))
                 .ForMember(x => x.PageIcon, opt => opt.MapFrom(src => src.Icon))
                 .ForMember(x => x.IsMenu, opt => opt.MapFrom(src => src.IsMenu))
                 .ForMember(x => x.ActionDetails, opt => opt.MapFrom(src => src.ActionDetails))
                 .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<RolePageUpsertModel, RolePageUpdateRequest>()
         .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<RolePageUpsertModel, RolePageInsertRequest>()
                 .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
    }
}
