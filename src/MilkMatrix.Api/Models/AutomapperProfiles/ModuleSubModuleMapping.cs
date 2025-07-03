using AutoMapper;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Module;
using MilkMatrix.Admin.Models.Admin.Requests.SubModule;
using MilkMatrix.Api.Models.Request.Admin.Module;
using MilkMatrix.Api.Models.Request.Admin.SubModule;

namespace MilkMatrix.Api.Models.AutomapperProfiles;

public class ModuleSubModuleMapping : Profile
{
    public ModuleSubModuleMapping()
    {

        CreateMap<SubModuleUpsertModel, SubModuleUpdateRequest>()
                         .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsAcive))
                         .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<SubModuleUpsertModel, SubModuleInsertRequest>()
                  .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<ModuleUpsertModel, ModuleUpdateRequest>()
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<ModuleUpsertModel, ModuleInsertRequest>()
                  .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
    }
}
